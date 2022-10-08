using System.Collections;
using Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotion : MonoBehaviour
{
    public float moveSpeed = 1f;
    public ContactFilter2D movementFilter;
    public float collisionsOffset = 0.05f;

    public PlayerStates state = PlayerStates.Movement;

    public Vector2 movementInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    public FixedStopwatch AttackCD = new FixedStopwatch();

    public bool isMoving { get; private set; } = false;
    public bool isAttacking { get; private set; } = false;


    private bool bcanAttack = true;
    private bool bcanMove = true;

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
        }

    }

    #region States

    void UpdateMovementState()
    {
        if (!bcanMove) return;
        bcanAttack = true;
        if (movementInput != Vector2.zero)
        {
            movementInput.Normalize();
            bool success = TryMove(movementInput);

            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));
                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
            }

            isMoving = success;

        }
        else
        {
            isMoving = false;
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
    #endregion

    #region Events

    void OnMove(InputValue movementValue)
    {
        state = PlayerStates.Movement;
        movementInput = movementValue.Get<Vector2>();
    }

    void OnAction()
    {
        //if (!AttackCD.IsReady) return;
        EnterAttackState();
    }

    #endregion

    public void LockMovement()
    {
        bcanMove = false;
    }
    
    public void UnlockMovement()
    {
        bcanMove = true;
    }

    private bool TryMove(Vector2 direction)
    {
        direction.Normalize();
        int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionsOffset);

        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }
    }

   
}
