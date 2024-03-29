using System.Collections;
using Common;
using System.Collections.Generic;
using System.Threading;
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
    
    [SerializeField] private AudioClip[] Voices;
    [SerializeField] private AudioClip[] StepSound;
    AudioClip lastClip;
    [SerializeField] private AudioClip RollSound;
    [SerializeField] private AudioClip BowPull;
    [SerializeField] private AudioClip WooshSound;
    [SerializeField] AudioSource voiceSource;
    [SerializeField] AudioSource effectSource;
    
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

    void EnterMovementState()
    {
        rb.velocity = Vector2.zero;
        state = PlayerStates.Movement;
    }

    void UpdateMovementState()
    {
        if (!bcanMove) return;
        
        bcanAttack = true;
        bcanRoll = true;
        bcanRotate = true;

        print(movementInput.magnitude);

        if (movementInput != Vector2.zero)
        {
            movementInput.Normalize();
            rb.velocity = Vector2.zero;
            rb.velocity = movementInput * moveSpeed;
            PlayStepSound();
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

    public void OnMove(InputAction.CallbackContext context)
    {
        if(!bcanRoll) return;
        EnterMovementState();
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnSword(InputAction.CallbackContext context)
    {
        //if (!AttackCD.IsReady) return;
        EnterAttackState();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        
        context.action.performed += input =>
        {
            if (input.interaction is TapInteraction)
            {
                EnterRollState();
            }
            
            if (input.interaction is HoldInteraction)
            {
                EnterRollState();
            }
            
        };
        
        context.action.canceled += input =>
        {
            print("Roll End");
            endRoll();
        };
        
        
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
        movementInput = Vector2.zero;
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
        EnterMovementState();
    }

    public void endAttack()
    {
        UnlockMovement();
        bcanAttack = true;
        EnterMovementState();
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

    public void PlayEvadeSound()
    {
        effectSource.volume = 0.1f;
        effectSource.PlayOneShot(RollSound);
        voiceSource.PlayOneShot(RandomClip(Voices));
    }

    public void PlayStepSound()
    {
        if(!effectSource.isPlaying) 
        {
            effectSource.volume = 0.1f;
            effectSource.PlayOneShot(RandomClip(StepSound));
        }
    }

    public void PlayWooshSound()
    {
        effectSource.volume = 0.1f;
        effectSource.PlayOneShot(WooshSound);
        voiceSource.PlayOneShot(RandomClip(Voices));
    }
    
    public void PlayPullSound()
    {
        effectSource.volume = 0.1f;
        effectSource.PlayOneShot(BowPull);
    }

    AudioClip RandomClip(AudioClip[] clips)
    {
        int attempts = 3;
        AudioClip newClip = clips[Random.Range(0, clips.Length)];
        while (newClip == lastClip && attempts > 0) 
        {
        newClip = clips[Random.Range(0, clips.Length)];
        attempts--;
        }
        lastClip = newClip;
        return newClip;
    }

}
