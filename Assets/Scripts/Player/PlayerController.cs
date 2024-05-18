using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.TimelinePlaybackControls;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]

public class PlayerController : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float groundSpeedAceleration = 50f;
    [SerializeField] private float airSpeedAceleration = 70f;
    [SerializeField] private float walkStopRate = 0.2f;
    private float currentSpeed;
    [Space(5)]

    [Header("Jumping")]
    [SerializeField] private float jumpPower = 27f;
    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;
    [SerializeField] private float jumpBufferTime = 0.1f;
    private float jumpBufferTimeCounter;
    [SerializeField] private float maximumFallSpeed = -40f;
    private bool doubleJump;
    [Space(5)]

    [Header("Dashing")]
    [SerializeField] private bool canDash = true;
    [SerializeField] private float dashPower = 20f;
    private float dashTime = 0.2f;
    [SerializeField] private float afterDashTime = 0.1f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private float dashStopRate = 5f;
    [SerializeField] private TrailRenderer tr;
    [Space(5)]

    [Header("Climbing")]
    [SerializeField] private float climbSpeed = 5f;
    private bool isOnLadder;
    private bool isClimbing;
    [SerializeField] private Transform ladderCenterPosition;
    private Collider2D ladderCollider;
    [Space(5)]

    [Header("Walling")]
    [SerializeField] private bool isWallSliding;
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private bool isWallJumping;
    private float wallJumpingDirection;
    [SerializeField] private float wallJumpingTime = 1f;
    private float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuration = 0.25f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(15f, 25f);
    [Space(5)]

    [Header("OnWayPlatformMovement")]
    [SerializeField] private CapsuleCollider2D playerCollider;
    [Space(5)]

    [Header("Camera")]
    [SerializeField] private GameObject cameraFollowGO;
    [Space(5)]


    public Rigidbody2D rb;
    private Animator animator;
    private Vector2 horizontalInput;
    private Vector2 verticalInput;
    private TouchingDirections touchingDirections;
    private Damageable damageable;
    private GameObject currentOneWayPlatform;
    private CameraFollowObject cameraFollowObject;
    private float fallSpeedYDampingChangeThreshold;
    private float originalGravityScale;


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

    public bool IsInAir
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

    private bool _isDashing = false;

    public bool IsDashing
    {
        get
        {
            return _isDashing;
        }
        private set
        {
            _isDashing = value;
            animator.SetBool(AnimationString.isDashing, value);
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
                    if (touchingDirections.IsGrounded)
                    {
                        // On ground move speed
                        currentSpeed += groundSpeedAceleration * Time.deltaTime;
                        currentSpeed = Mathf.Min(currentSpeed, moveSpeed);
                        return currentSpeed;
                    }
                    else
                    {
                        // In air move speed
                        currentSpeed += airSpeedAceleration * Time.deltaTime;
                        currentSpeed = Mathf.Min(currentSpeed, moveSpeed);
                        return currentSpeed;
                    }
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
        originalGravityScale = rb.gravityScale;
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
        this.GroundCheck();
        this.FallCheck();
        this.LadderClimbCheck();
        this.OneWayCheck();
        this.YDampingCheck();
    }

    private void FixedUpdate()
    {
        if (!isClimbing)
        {
            Move();
        }
        LadderClimb();
        WallSlide();
        WallJump();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>();

        if (IsAlive && !isClimbing) // Disable horizontal movement while climbing
        {
            IsMoving = horizontalInput != Vector2.zero;
            SetFacingDirection(horizontalInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isClimbing)
        {
            isClimbing = false;
            rb.gravityScale = originalGravityScale;
            rb.velocity = new Vector2(rb.velocity.x, jumpPower); // Allow jumping while climbing
        }
        else
        {
            if (context.started)
            {
                jumpBufferTimeCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferTimeCounter -= Time.deltaTime;
            }

            if (context.started && wallJumpingCounter > 0f && isWallSliding)
            {
                CoroutineManager.Instance.StartManagedCoroutine(WallJumping());
            }

            if (jumpBufferTimeCounter > 0f && coyoteTimeCounter > 0f && CanMove && !IsDashing || doubleJump && CanMove)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                jumpBufferTimeCounter = 0f;
                doubleJump = false;
            }

            if (context.canceled && rb.velocity.y > 0)
            {
                coyoteTimeCounter = 0f;
            }
        }
      
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && canDash)
        {
            CoroutineManager.Instance.StartManagedCoroutine(Dashing());
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
        verticalInput = context.ReadValue<Vector2>();
    }

    private void Move()
    {
        if (!damageable.LockVelocity && !isWallJumping)
        {
            if (horizontalInput.x == 0 && !IsDashing)
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
                currentSpeed = 0;
            }
            else if (horizontalInput.x != 0 && !IsDashing)
            {
                rb.velocity = new Vector2(horizontalInput.x * CurrentSpeed, rb.velocity.y);
            }
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight && !isWallJumping)
        {
            this.Turn();

        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            this.Turn();
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

    private void LadderClimb()
    {
        if (isClimbing && !damageable.LockVelocity)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(0, verticalInput.y * climbSpeed); // Restrict horizontal movement

            if (ladderCollider != null)
            {
                transform.position = new Vector3(ladderCollider.bounds.center.x, transform.position.y, transform.position.z); // Move to the center of the ladder
            }
        }
    }

    private void WallSlide()
    {
        if (touchingDirections.IsOnJumpWall && !touchingDirections.IsGrounded && horizontalInput.x != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            float jumpDirection = IsFacingRight ? 1f : -1f;
            wallJumpingDirection = -jumpDirection;
            wallJumpingCounter = wallJumpingTime;
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
    }

    private void GroundCheck()
    {
        if (touchingDirections.IsGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            IsInAir = false;
            animator.SetFloat(AnimationString.yVelocity, 0);
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            IsInAir = true;
            animator.SetFloat(AnimationString.yVelocity, rb.velocity.y);
        }
    }

    private void FallCheck()
    {
        if (IsInAir && rb.velocity.y < maximumFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maximumFallSpeed);
        }
    }

    private void LadderClimbCheck()
    {
        if (isOnLadder && verticalInput.y != 0)
        {
            isClimbing = true;
        }
        if (!isOnLadder)
        {
            isClimbing = false;
            rb.gravityScale = originalGravityScale;
        }
    }


    private void OneWayCheck()
    {
        if (verticalInput.y < 0)
        {
            if (currentOneWayPlatform != null)
            {
                CoroutineManager.Instance.StartManagedCoroutine(DisableCollision());
            }
        }
    }

    private void YDampingCheck()
    {
        //if player falling past the certain speed threshold
        if (rb.velocity.y < fallSpeedYDampingChangeThreshold && !CameraManger.instance.IsLerpingYDamping && !CameraManger.instance.LerpedFromPlayerFalling)
        {
            CameraManger.instance.lerpYDamping(true);
        }

        //if player are standing or moving
        if (rb.velocity.y >= 0f && !CameraManger.instance.IsLerpingYDamping && CameraManger.instance.LerpedFromPlayerFalling)
        {
            //rest so it can be called again
            CameraManger.instance.LerpedFromPlayerFalling = false;

            CameraManger.instance.lerpYDamping(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = true;
            ladderCollider = collision; // Store reference to the ladder collider
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = false;
            isClimbing = false;
            rb.gravityScale = originalGravityScale;
            ladderCollider = null; // Clear reference to the ladder collider
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

    private IEnumerator Dashing()
    {
        damageable.IsInvincible = true;
        IsDashing = true;
        canDash = false;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        // Calculate the direction of the dash based on the player's facing direction
        float dashDirection = IsFacingRight ? 1f : -1f;

        // Apply dash velocity with direction
        rb.velocity = new Vector2(dashDirection * dashPower, 0f);
        tr.emitting = true;

        yield return new WaitForSeconds(dashTime);

        if (horizontalInput.x == 0 && IsDashing)
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, dashStopRate), rb.velocity.y);
        }

        tr.emitting = false;
        IsDashing = false;
        damageable.IsInvincible = false;
        rb.gravityScale = originalGravity;
        doubleJump = true;

        yield return new WaitForSeconds(afterDashTime);
        doubleJump = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator WallJumping()
    {
        isWallJumping = true;
        
        rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
        wallJumpingCounter = 0f;

        float jumpDirection = IsFacingRight ? 1f : -1f;

        if (jumpDirection != wallJumpingDirection)
        {
            this.Turn();
        }

        yield return new WaitForSeconds(wallJumpingDuration);
        isWallJumping = false;
        this.SetFacingDirection(horizontalInput);
    }
}
