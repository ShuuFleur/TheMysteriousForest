using System.Collections;
using Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public float moveSpeed = 1.2f;

    public PlayerStates state = PlayerStates.Idle;

    public Vector2 movementInput;
    Rigidbody2D rb;
    public Vector2 facingDirection = Vector2.down;



    public FixedStopwatch AttackCD = new FixedStopwatch();
    [SerializeField] private InputActionReference actionShootReference;
    [SerializeField] private InputActionReference actionDodgeReference;
    public GameObject projectile;


    public float rollSpeed = 2.4f;

    public bool isMoving { get; private set; } = false;


    private bool bcanAttack = true;
    private bool bcanMove = true;
    private bool bcanRoll = true;
    private bool bcanRotate = true;

    private Animator _animator;

    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (bcanRotate)
        {
            switch (movementInput)
            {
                case Vector2 v when v.Equals(Vector2.up):
                    facingDirection = new Vector2(0, 1);
                    break;
                case Vector2 v when v.Equals(Vector2.down):
                    facingDirection = new Vector2(0, -1);
                    break;
                case Vector2 v when v.Equals(Vector2.left):
                    facingDirection = new Vector2(-1, 0);
                    break;
                case Vector2 v when v.Equals(Vector2.right):
                    facingDirection = new Vector2(1, 0);
                    break;
            }
        }

        switch (state)
        {
            case PlayerStates.Movement:
                UpdateMovementState();
                //print("MOVE!");
                break;
            case PlayerStates.Sword:
                UpdateSwordState();
                //print("ATTACK!");
                break;
            case PlayerStates.Roll:
                UpdateRollState();
                break;
            case PlayerStates.Shoot:
                UpdateShootState();
                break;
        }

    }

    #region States

    void UpdateMovementState()
    {
        if (!bcanMove) return;
        
        bcanAttack = true;
        bcanRoll = true;
        bcanRotate = true;

        if (movementInput != Vector2.zero)
        {
            movementInput.Normalize();
            rb.velocity = movementInput * moveSpeed;

        }
        else
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
            state = PlayerStates.Idle;
        }
    }

    void EnterAttackState()
    {

        if (!bcanAttack) return;
        AttackCD.Split();
        state = PlayerStates.Sword;
        bcanAttack = false;
        bcanRotate = false;

    }

    void UpdateSwordState()
    {
        
        _animator.SetTrigger("Attacking");
        state = PlayerStates.Movement;
    }

    void EnterRollState()
    {
        if (!bcanRoll) return;
        
        state = PlayerStates.Roll;
        bcanRoll = false;
        bcanAttack = false;
        bcanRotate = false;
        
        _animator.SetTrigger("Rolling");
    }

    void UpdateRollState()
    {
        rb.velocity = facingDirection * rollSpeed;
        print(facingDirection);
    }

    void EnterShootState()
    {
        
        state = PlayerStates.Shoot;

    }

    void UpdateShootState()
    {
        actionShootReference.action.performed += context =>
        {
            if (context.interaction is PressInteraction)
            {
                print("Pressing");
                _animator.SetBool("ShootHolding", true);
            }
        };
        actionShootReference.action.canceled += context =>
        {
            print("Release");
            _animator.SetBool("ShootHolding", false);
            state = PlayerStates.Movement;
        };
    }

    #endregion

    #region Events

    void OnMove(InputValue movementValue)
    {
        if(!bcanRoll) return;
        state = PlayerStates.Movement;
        movementInput = movementValue.Get<Vector2>();
    }

    void OnSword()
    {
        //if (!AttackCD.IsReady) return;
        EnterAttackState();
    }

    void OnRoll()
    {
        EnterRollState();
    }

    void OnShoot()
    {
        EnterShootState();
    }


    #endregion

    public void LockMovement()
    {
        bcanMove = false;
        bcanRoll = false;
        rb.velocity = Vector2.zero;
    }
    
    public void UnlockMovement()
    {
        bcanMove = true;
        bcanRoll = true;
        bcanRotate = true;
        rb.velocity = Vector2.zero;
    }

    public void endRoll()
    {
        UnlockMovement();
        bcanAttack = true;
        state = PlayerStates.Movement;
    }

    public void endAttack()
    {
        UnlockMovement();
        bcanAttack = true;
        state = PlayerStates.Movement;
    }

    Vector3 ChooseArrowDirection()
    {
        float temp = Mathf.Atan2(_animator.GetFloat("Y"), _animator.GetFloat("X")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

    public void ShootProjectile() 
    {

        Vector2 temp = new Vector2(_animator.GetFloat("X"), _animator.GetFloat("Y"));
        Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<ArrowController>().Setup(facingDirection, ChooseArrowDirection());

    }

}
