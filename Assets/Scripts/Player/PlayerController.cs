using System.Collections;
using Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.2f;
    public float maxSpeed = 8f;
    public ContactFilter2D movementFilter;
    public float collisionsOffset = 0.05f;

    public PlayerStates state = PlayerStates.Movement;

    public Vector2 movementInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public Vector2 facingDirection = Vector2.down;

    public FixedStopwatch AttackCD = new FixedStopwatch();
    [SerializeField] private InputActionReference actionReference;

    public float rollSpeed = 2.4f;

    public bool isMoving { get; private set; } = false;
    public bool isAttacking { get; private set; } = false;


    private bool bcanAttack = true;
    private bool bcanMove = true;
    private bool bcanRoll = true;

    private Animator _animator;

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
        }

    }

    #region States

    void UpdateMovementState()
    {
        if (!bcanMove) return;
        bcanAttack = true;
        bcanRoll = true;

        if (movementInput != Vector2.zero)
        {
            movementInput.Normalize();

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

            rb.velocity = movementInput * moveSpeed;

        }
        else
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
        }
    }

    void EnterAttackState()
    {

        if (state != PlayerStates.Movement || !bcanAttack) return;
        //AttackCD.Split();
        state = PlayerStates.Sword;
        bcanAttack = false;

    }

    void UpdateSwordState()
    {
        
        _animator.SetTrigger("Attacking");
        state = PlayerStates.Movement;

    }

    void EnterRollState()
    {
        if (state != PlayerStates.Movement || !bcanRoll) return;
        state = PlayerStates.Roll;
        bcanRoll = false;
        bcanAttack = false;
        _animator.SetTrigger("Rolling");
        rollSpeed = 2.4f;
    }

    void UpdateRollState()
    {
        rb.velocity = facingDirection * rollSpeed;
    }

    #endregion

    #region Events

    void OnMove(InputValue movementValue)
    {
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
        actionReference.action.performed += context =>
        {
            if (context.interaction is HoldInteraction)
            {
                print("Holding");
            }
            else if (context.interaction is PressInteraction)
            {
                print("Pressing");
                _animator.SetBool("ShootHolding", true);
            }
        };
        actionReference.action.canceled += context =>
        {
            print("Release");
            _animator.SetBool("ShootHolding", false);
        };
    }


    #endregion

    void Test()
    {
        print("Oui");
    }

    public void LockMovement()
    {
        bcanMove = false;
        rb.velocity = Vector2.zero;
    }
    
    public void UnlockMovement()
    {
        bcanMove = true;
        rb.velocity = Vector2.zero;
    }

    public void endRoll()
    {
        rb.velocity = Vector2.zero;
        UnlockMovement();
        bcanAttack = true;
        state = PlayerStates.Movement;
    }

   
}
