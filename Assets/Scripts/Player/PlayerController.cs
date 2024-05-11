using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]

public class PlayerController : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float walkAceleration = 4f;
    [SerializeField] private float walkStopRate = 0.2f;
    private float currentSpeed;
    [Space(5)]

    [Header("Jumping")]
    [SerializeField] private float jumpPower = 25f;
    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;
    [SerializeField] private float jumpBufferTime = 0.1f;
    private float jumpBufferTimeCounter;
    [Space(5)]

    [Header("Dashing")]
    [SerializeField] private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashPower = 20f;
    private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private float dashStopRate = 10f;
    [SerializeField] private TrailRenderer tr;
    [Space(5)]

    [Header("Climbing")]
    [SerializeField] private float climbSpeed = 5f;
    private bool isOnLadder;
    private bool isClimbing;
    [Space(5)]

    [Header("OnWayPlatformMovement")]
    [SerializeField] private CapsuleCollider2D playerCollider;
    [Space(5)]

    [Header("Camera")]
    [SerializeField] private GameObject cameraFollowGO;
    [Space(5)]

    public Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private Vector2 climbInput;
    private TouchingDirections touchingDirections;
    private Damageable damageable;
    private GameObject currentOneWayPlatform;
    private CameraFollowObject cameraFollowObject;
    private float fallSpeedYDampingChangeThreshold;


    private bool _isMoving = false;

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationString.isMoving, value);
        }
    }

    private bool _isInAir = false;

    public bool isInAir
    {
        get
        {
            return _isInAir;
        }
        private set
        {
            _isInAir = value;
            animator.SetBool(AnimationString.isInAir, value);
        }
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight 
    { 
        get 
        {
            return _isFacingRight; 
        }
        private set 
        { 
            _isFacingRight = value;
        } 
    }

    public float CurrentSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving)
                {
                    // On ground move speed
                    currentSpeed += walkAceleration * Time.deltaTime;
                    currentSpeed = Mathf.Min(currentSpeed, walkSpeed);
                    return currentSpeed;
                }
                else
                {
                    // Idle speed
                    return 0;
                }
            }
            else
            {
                // Movement lock
                return 0;
            }
        }
    }

    public bool CanMove 
    {
        get
        {
            return animator.GetBool(AnimationString.canMove);
        }
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationString.isAlive);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraFollowObject = cameraFollowGO.GetComponent<CameraFollowObject>();
        fallSpeedYDampingChangeThreshold = CameraManger.instance._fallSpeedYDampingChangeThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        this.DashCheck();
        this.FallCheck();
        this.ClimbCheck();
        this.OneWayCheck();
    }

    private void FixedUpdate()
    {
        this.DashCheck();
        this.Move();
        this.Climb();
        this.YDampingCheck();
    }

    private void FallCheck()
    {
        if (touchingDirections.IsGrounded)
        {
            isInAir = false;
            coyoteTimeCounter = coyoteTime;
            animator.SetFloat(AnimationString.yVelocity, 0);
        }
        else
        {
            isInAir = true;
            animator.SetFloat(AnimationString.yVelocity, rb.velocity.y);
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpBufferTimeCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferTimeCounter -= Time.deltaTime;
        }

        if (jumpBufferTimeCounter > 0f && coyoteTimeCounter > 0f && CanMove)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);

            jumpBufferTimeCounter = 0f;
        }

        if (context.canceled && rb.velocity.y > 0)
        {
            coyoteTimeCounter = 0f;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight)
        {
            Turn();
        }else if (moveInput.x < 0 && IsFacingRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        if (IsFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

            //turn the camera follow object
            cameraFollowObject.CallTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

            //turn the camera follow object
            cameraFollowObject.CallTurn();
        }
    }

    private void Move()
    {
        if (!damageable.LockVelocity)
        {
            if (moveInput.x == 0 && !isDashing)
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
                currentSpeed = 0;
            }
            else if(moveInput.x != 0 && !isDashing)
            {
                rb.velocity = new Vector2(moveInput.x * CurrentSpeed, rb.velocity.y);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            this.SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && canDash)
        {
            animator.SetTrigger(AnimationString.dashTrigger);
            StartCoroutine(Dashing());
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationString.attackTrigger);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnClimb(InputAction.CallbackContext context)
    {
        climbInput = context.ReadValue<Vector2>();
    }

    private void ClimbCheck()
    {
        if (isOnLadder && climbInput.y > 0)
        {
            isClimbing = true;
        }
    }

    private void Climb()
    {
        if (isClimbing && !damageable.LockVelocity)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, climbInput.y * climbSpeed);
        }
        else
        {
            rb.gravityScale = 6f;
        }
    }

    private void DashCheck()
    {
        if (isDashing)
        {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = false;
            isClimbing = false;
        }
    }

    private void OneWayCheck()
    {
        if (climbInput.y < 0)
        {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        CompositeCollider2D platformCollider = currentOneWayPlatform.GetComponent<CompositeCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }

    private void YDampingCheck()
    {
        //if player falling past the certain speed threshold
        if(rb.velocity.y < fallSpeedYDampingChangeThreshold && !CameraManger.instance.IsLerpingYDamping && !CameraManger.instance.LerpedFromPlayerFalling)
        {
            CameraManger.instance.lerpYDamping(true);
        }

        //if player are standing or moving
        if(rb.velocity.y >= 0f && !CameraManger.instance.IsLerpingYDamping && CameraManger.instance.LerpedFromPlayerFalling)
        {
            //rest so it can be called again
            CameraManger.instance.LerpedFromPlayerFalling = false;

            CameraManger.instance.lerpYDamping(false);
        }
    }

    private IEnumerator Dashing()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        // Calculate the direction of the dash based on the player's facing direction
        float dashDirection = IsFacingRight ? 1f : -1f;

        // Apply dash velocity with direction
        if (rb.velocity.y < 0 && !touchingDirections.IsGrounded || touchingDirections.IsGrounded)
        {
            rb.velocity = new Vector2(dashDirection * dashPower, 0f);
        }
        else
        {
            rb.velocity = new Vector2(dashDirection * dashPower, rb.velocity.y);
        }
        tr.emitting = true;

        yield return new WaitForSeconds(dashTime);

        if (moveInput.x == 0 && isDashing)
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, dashStopRate), rb.velocity.y);
        }
        
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

}
