using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour, IPlayerDamageable, IPlayerMoveable
{
    #region Player variables

    [Header("Moving")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float groundSpeedAceleration = 75f;
    [SerializeField] private float airSpeedAceleration = 100f;
    [SerializeField] private float stopRate = 0.2f;
    [Space(5)]

    [Header("Jumping")]
    [SerializeField] private float jumpPower = 20f;
    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;
    [SerializeField] private float jumpBufferTime = 0.1f;
    private float jumpBufferTimeCounter;
    [SerializeField] private float maximumFallSpeed = -40f;
    [Space(5)]

    [Header("Dashing")]
    private bool canDash = true;
    [SerializeField] private float dashPower = 20f;
    private float dashTime = 0.5f;
    [SerializeField] private float dashCooldown = 2f;
    private float dashStopRate = 5f;
    [SerializeField] private float dashStaminaCost = 30f;
    [Space(5)]

    [Header("Climbing")]
    [SerializeField] private float climbSpeed = 5f;
    private float centerLadderMoveSpeed = 10f;
    private bool isOnLadder;
    private Transform LadderCenterPosition;
    private Collider2D ladderCollider;
    [Space(5)]

    [Header("Wall Jumping")]
    [SerializeField] private float wallSlideSpeed = 1f;
    [SerializeField] private float wallSlideDuration = 0.5f;
    private float wallJumpingDirection;
    private float wallJumpingTime = 1f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.2f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(5f, 22f);
    [SerializeField] private float wallHangStaminaCost = 10f;    
    [Space(5)]

    [Header("Wall Jump Speed Boost")]
    private float originalMoveSpeed;
    [SerializeField] private float wallJumpSpeedBoost = 3f;
    [SerializeField] private float wallJumpSpeedBoostDuration = 2f;
    private bool isWallJumpSpeedBoostActive = false;
    private Coroutine wallJumpSpeedBoostCoroutine;
    [Space(5)]

    [Header("Stamina Regen")]
    [SerializeField] private float staminaRegenSpeed = 5f;
    [Space(5)]

    [Header("Ivincible")]
    private float timeSinceHit = 0;
    [SerializeField] private float invincibleTime = 2f;
    [Space(5)]

    [Header("Collect Item")]
    [SerializeField] private bool hasItemInRange;
    private Collectable Item;
    [Space(5)]

    [Header("Save Point")]
    [SerializeField] private bool isInSavePoint;
    private CheckPointManager CheckPointManager;
    [Space(5)]

    [Header("One Way Platform Movement")]
    private BoxCollider2D PlayerCollider;
    [Space(5)]

    [Header("Camera Shake")]
    [SerializeField] private float slowMotionDuration = 0.5f;
    [SerializeField] private float slowMotionFactor = 0.2f;
    [Space(5)]

    private Animator Animator;
    private TouchingDirections TouchingDirections;
    private GameObject CurrentOneWayPlatform;
    private CameraFollowObject CameraFollowObject;
    private GhostTrail GhostTrail;
    private HealthBar HealthBar;
    private CinemachineImpulseSource ImpulseSource;
    private PlayerEquipment PlayerEquipment;
    private float FallSpeedYDampingChangeThreshold;
    private float OriginalGravityScale;

    #endregion

    #region Player Equipment buffs

    [Header("Equipment Buffs")]
    public float damageBuff;
    public float defenseBuff;
    public float healthBuff;
    public float healthRegenBuff;
    public float staminaBuff;
    public float staminaRegenBuff;
    public float moveSpeedBuff;
    public float jumpPowerBuff;
    public float wallJumpPowerBuff;
    public float dashPowerBuff;

    #endregion

    #region Player animation properties

    public bool IsMoving
    {
        get
        {
            return Animator.GetBool(AnimationString.isMoving);
        }
        set
        {
            Animator.SetBool(AnimationString.isMoving, value);
        }
    }

    public bool IsJumping
    {
        get
        {
            return Animator.GetBool(AnimationString.isJumping);
        }
        set
        {
            Animator.SetBool(AnimationString.isJumping, value);
        }
    }

    public bool IsDashing
    {
        get
        {
            return Animator.GetBool(AnimationString.isDashing);
        }
        set
        {
            Animator.SetBool(AnimationString.isDashing, value);
        }
    }

    public bool IsDashed
    {
        get
        {
            return Animator.GetBool(AnimationString.isDashed);
        }
        set
        {
            Animator.SetBool(AnimationString.isDashed, value);
        }
    }

    public bool IsWallHanging
    {
        get
        {
            return Animator.GetBool(AnimationString.isWallHanging);
        }
        set
        {
            Animator.SetBool(AnimationString.isWallHanging, value);
        }
    }

    public bool IsWallJumping
    {
        get
        {
            return Animator.GetBool(AnimationString.isWallJumping);
        }
        set
        {
            Animator.SetBool(AnimationString.isWallJumping, value);
        }
    }

    public bool IsClimbing
    {
        get
        {
            return Animator.GetBool(AnimationString.isClimbing);
        }
        set
        {
            Animator.SetBool(AnimationString.isClimbing, value);
        }
    }

    #endregion

    #region IPlayerDamageable variables

    private float _maxHealth = 100f;
    public float MaxHealth 
    {
        get => _maxHealth + healthBuff;
        set
        {
            _maxHealth = value;
        }
    }

    private float _currentHealth;
    public float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;

            if (value <= 0)
            {
                IsAlive = false;
            }
        }
    }

    private float _maxStamina = 100f;
    public float MaxStamina 
    {
        get => _maxStamina + staminaBuff;
        set
        {
            _maxStamina = value;
        }
    }

    private float _currentStamina;
    public float CurrentStamina
    {
        get => _currentStamina;
        set
        {
            _currentStamina = value;
        }
    }

    public int MaxHealthPotion { get; set; } = 2;
    [field: SerializeField] public int CurrentHealthPotion { get; set; }
    [field: SerializeField] public float HealthRestore { get; set; } = 50f;

    [SerializeField] private bool _isAlive = true;
    public bool IsAlive
    {
        get => _isAlive;
        set
        {
            _isAlive = value;
            Animator.SetBool(AnimationString.isAlive, value);

            if (value == false)
            {
                DamageableDead.Invoke();
            }
        }
    }

    public bool IsInvincible 
    { 
        get 
        {
            return Animator.GetBool(AnimationString.isInvincible);
        } 
        set 
        { 
            Animator.SetBool(AnimationString.isInvincible, value); 
        } 
    }

    public bool WasHit { get; set; }

    public bool LockVelocity
    {
        get
        {
            return Animator.GetBool(AnimationString.lockVelocity);
        }
        set
        {
            Animator.SetBool(AnimationString.lockVelocity, value);
        }
    }

    #endregion

    #region IPlayerMoveable variables

    public Vector2 HorizontalInput { get; set; }
    public Vector2 VerticalInput { get; set; }
    public Rigidbody2D RB { get; set; }
    [field: SerializeField] public bool IsFacingRight { get; set; } = true;
    public bool CanMove { get => Animator.GetBool(AnimationString.canMove);}

    [SerializeField] private float _currentSpeed;
    public float CurrentSpeed
    {
        get
        {
            if (CanMove)
            {
                if (TouchingDirections.IsGrounded)
                {
                    // On ground move speed
                    _currentSpeed += groundSpeedAceleration * Time.deltaTime;
                    _currentSpeed = Mathf.Min(_currentSpeed, moveSpeed + moveSpeedBuff);
                    return _currentSpeed;
                }
                else
                {
                    // In air move speed
                    _currentSpeed += airSpeedAceleration * Time.deltaTime;
                    _currentSpeed = Mathf.Min(_currentSpeed, moveSpeed + moveSpeedBuff);
                    return _currentSpeed;
                }
            }
            else
            {
                // Movement lock
                return 0;
            }
        }
        set
        {
            _currentSpeed = value;
        }
    }

    #endregion

    #region Player events system

    [field: SerializeField] public UnityEvent<float, Vector2> DamageableHit { get; set; }
    [field: SerializeField] public UnityEvent DamageableDead { get; set; }

    #endregion

    #region Player Singleton implementation

    public static Player Instance { get; private set; }

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        RB = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        TouchingDirections = GetComponent<TouchingDirections>();
        GhostTrail = GetComponent<GhostTrail>();
        HealthBar = GetComponent<HealthBar>();
        ImpulseSource = GetComponent<CinemachineImpulseSource>();
        OriginalGravityScale = RB.gravityScale;
        PlayerEquipment = GetComponent<PlayerEquipment>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        SetFacingCheck();
        GroundCheck();
        FallCheck();
        LadderClimbCheck();
        OneWayCheck();
        YDampingCheck();
        WallHangCheck();
        HealthAndStaminaRegeneration();
        UpdateHealthBar();
        BeInvisible();
    }

    private void FixedUpdate()
    {
        Move();
        WallHang();
        LadderClimb();
    }

    #region IPlayerDamageable functions

    public void TakeDamage(float damage, Vector2 knockback, Vector2 hitDirection, int attackType)
    {
        if (IsAlive && !IsInvincible)
        {
            float damageTaken = damage - defenseBuff;
            CurrentHealth -= damageTaken;
            WasHit = true;

            CoroutineManager.Instance.StartCoroutineManager(ApplySlowMotion());
            CameraShakeManager.Instance.CameraShake(ImpulseSource);
            Animator.SetTrigger(AnimationString.hitTrigger);

            DamageableHit?.Invoke(damageTaken, knockback);
            CharacterEvent.characterDamaged.Invoke(gameObject, damageTaken);
            CharacterEvent.hitSplash.Invoke(gameObject, hitDirection, attackType);
        }
        else
        {
            return;
        }
    }

    public void BeInvisible()
    {
        if (WasHit)
        {
            IsInvincible = true;
            timeSinceHit += Time.deltaTime;

            if (timeSinceHit > invincibleTime)
            {
                // Remove invincible after a short time
                IsInvincible = false;
                WasHit = false;
                timeSinceHit = 0;
            }
        }
    }

    #endregion

    #region IPlayerMoveable functions

    public void TurnPlayer()
    {
        CameraFollowObject = GameObject.Find("CameraFollowObject").GetComponent<CameraFollowObject>();

        if (IsFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

            //turn the camera follow object
            CameraFollowObject.CallTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

            //turn the camera follow object
            CameraFollowObject.CallTurn();
        }
    }

    public void SetFacing(Vector2 moveInput)
    {
        if (!IsWallJumping && !IsWallHanging && !IsDashing && !IsClimbing)
        {
            if (moveInput.x > 0 && !IsFacingRight)
            {
                TurnPlayer();

            }
            else if (moveInput.x < 0 && IsFacingRight)
            {
                TurnPlayer();
            }
        }
    }

    #endregion

    #region UI health bar functions

    public void HealthAndStaminaRegeneration()
    {
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }

        if (CurrentStamina > MaxStamina)
        {
            CurrentStamina = MaxStamina;
        }

        if (CurrentStamina < MaxStamina)    
        {
            CurrentStamina += ((staminaRegenSpeed + staminaBuff) * Time.deltaTime);
            CurrentStamina = Mathf.Min(CurrentStamina, MaxStamina);
        }
    }

    public void UpdateHealthBar()
    {
        HealthBar.healthSlider.maxValue = MaxHealth;
        HealthBar.staminaSlider.maxValue = MaxStamina;
        HealthBar.SetHealth(CurrentHealth);
        HealthBar.SetStamina(CurrentStamina);
        HealthBar.SetHealthPotions(CurrentHealthPotion);
    }

    #endregion

    #region Movement functions

    private void Move()
    {
        if (!UIManager.Instance.menuActivated)
        {
            if (!LockVelocity && !IsWallJumping && !IsWallHanging && !IsClimbing)
            {
                if (HorizontalInput.x == 0 && !IsDashing)
                {
                    RB.velocity = new Vector2(Mathf.Lerp(RB.velocity.x, 0, stopRate), RB.velocity.y);
                    _currentSpeed = 0;
                }
                else if (HorizontalInput.x != 0 && !IsDashing)
                {
                    RB.velocity = new Vector2(HorizontalInput.x * CurrentSpeed, RB.velocity.y);
                }
            }
        }
    }

    private void LadderClimb()
    {
        if (!UIManager.Instance.menuActivated)
        {
            if (IsClimbing)
            {
                RB.gravityScale = 0f;
                RB.velocity = new Vector2(0f, VerticalInput.y * climbSpeed);

                if (ladderCollider != null)
                {
                    Vector3 targetPosition = new Vector3(ladderCollider.bounds.center.x, transform.position.y, transform.position.z);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, centerLadderMoveSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void WallHang()
    {
        if (!UIManager.Instance.menuActivated)
        {
            if (IsWallHanging)
            {
                IsWallJumping = false;
                float jumpDirection = IsFacingRight ? 1f : -1f;
                wallJumpingDirection = -jumpDirection;
                wallJumpingCounter = wallJumpingTime;
            }
            else
            {
                wallJumpingCounter -= Time.deltaTime;
            }
        }
    }

    #endregion

    #region Player animation event functions

    public void UseHealthPotion()
    {
        if (IsAlive && !LockVelocity)
        {
            float maxHeal = Mathf.Max(MaxHealth - CurrentHealth, 0);
            float actualHeal = Mathf.Min(maxHeal, HealthRestore + healthRegenBuff);
            CurrentHealth += actualHeal;
            CurrentStamina = MaxStamina;
            CurrentHealthPotion -= 1;
            CharacterEvent.characterHealed(gameObject, actualHeal);
        }
        else
        {
            return;
        }
    }

    public void RestoreFullStats()
    {
        CurrentHealth = MaxHealth;
        CurrentStamina = MaxStamina;
        CurrentHealthPotion = MaxHealthPotion;
    }

    public void ActivateSavePoint()
    {
        if (CheckPointManager != null)
        {
            CheckPointManager.ActivateCheckPoint();
            CheckPointManager.SaveCheckPoint();
            CheckPointManager.RespawnEnemiesAfterSpawn();
        }
    }

    public void Respawn()
    {
        GameManager.Instance.RespawnPlayer();
    }

    public void CollectItem()
    {
        if (hasItemInRange && Item != null)
        {
            Item.CollectItem();
        }
    }

    #endregion

    #region Player envent system functions

    public void OnHit(float damage, Vector2 knockback)
    {
        CoroutineManager.Instance.StartCoroutineManager(Knockback(knockback));
    }

    public void OnDeath()
    {

    }

    #endregion

    #region Player checking functions

    public void SetFacingCheck()
    {
        if (IsMoving && CanMove)
        {
            SetFacing(HorizontalInput);
        }
    }

    public void GroundCheck()
    {
        if (TouchingDirections.IsGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            Animator.SetFloat(AnimationString.yVelocity, 0);
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            Animator.SetFloat(AnimationString.yVelocity, RB.velocity.y);
        }
    }

    public void FallCheck()
    {
        if (!TouchingDirections.IsGrounded && RB.velocity.y < maximumFallSpeed)
        {
            FallSpeedYDampingChangeThreshold = CameraManger.Instance._fallSpeedYDampingChangeThreshold;
            RB.velocity = new Vector2(RB.velocity.x, maximumFallSpeed);
        }
    }

    public void LadderClimbCheck()
    {
        if (isOnLadder && !LockVelocity && VerticalInput.y != 0 && RB.velocity.y <= 1)
        {
            IsClimbing = true;
            RB.velocity = new Vector2(0f, RB.velocity.y);
        }
        if (!isOnLadder)
        {
            IsClimbing = false;
        }
    }

    public void WallHangCheck()
    {
        if (!TouchingDirections.IsGrabWallDetected || IsWallJumping || Animator.GetBool(AnimationString.hitTrigger))
        {
            IsWallHanging = false;
            RB.gravityScale = OriginalGravityScale;
        }
    }


    public void OneWayCheck()
    {
        if (VerticalInput.y < 0)
        {
            if (CurrentOneWayPlatform != null)
            {
                CoroutineManager.Instance.StartCoroutineManager(DisableCollision());
            }
        }
    }

    public void YDampingCheck()
    {
        //if player falling past the certain speed threshold
        if (RB.velocity.y < FallSpeedYDampingChangeThreshold && !CameraManger.Instance.IsLerpingYDamping && !CameraManger.Instance.LerpedFromPlayerFalling)
        {
            CameraManger.Instance.LerpYDamping(true);
        }

        //if player are standing or moving
        if (RB.velocity.y >= 0f && !CameraManger.Instance.IsLerpingYDamping && CameraManger.Instance.LerpedFromPlayerFalling)
        {
            //rest so it can be called again
            CameraManger.Instance.LerpedFromPlayerFalling = false;

            CameraManger.Instance.LerpYDamping(false);
        }
    }

    public void InputCheck()
    {
        Animator.SetBool(AnimationString.upInput, VerticalInput.y > 0);
    }

    #endregion

    #region Player Input functions

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!UIManager.Instance.menuActivated)
        {
            HorizontalInput = context.ReadValue<Vector2>();
            if (IsAlive)
            {
                IsMoving = HorizontalInput != Vector2.zero;
            }
            else
            {
                IsMoving = false;
            }
        }
        else
        {
            HorizontalInput = Vector2.zero;
            IsMoving = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!UIManager.Instance.menuActivated)
        {
            if (context.started && IsClimbing)
            {
                IsClimbing = false;
                RB.gravityScale = OriginalGravityScale;
                RB.velocity = new Vector2(RB.velocity.x, jumpPower + jumpPowerBuff);
            }
            else if (context.started)
            {
                jumpBufferTimeCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferTimeCounter -= Time.deltaTime;
            }

            if (context.started && wallJumpingCounter > 0f && IsWallHanging)
            {
                CoroutineManager.Instance.StartCoroutineManager(WallJumping());
            }

            if (jumpBufferTimeCounter > 0f && coyoteTimeCounter > 0f && CanMove)
            {
                IsDashing = false;
                RB.velocity = new Vector2(RB.velocity.x, jumpPower + jumpPowerBuff);
                jumpBufferTimeCounter = 0f;
                IsJumping = true;
            }

            if (context.canceled && RB.velocity.y > 0)
            {
                coyoteTimeCounter = 0f;
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!UIManager.Instance.menuActivated)
        {
            if (context.started && IsAlive && !IsWallHanging && !IsDashing && !IsClimbing)
            {
                Animator.SetTrigger(AnimationString.attackTrigger);
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!UIManager.Instance.menuActivated)
        {
            if (context.started && canDash && CanMove && TouchingDirections.IsGrounded && !IsDashing && !IsClimbing && CurrentStamina >= dashStaminaCost)
            {
                CoroutineManager.Instance.StartCoroutineManager(Dashing());
            }
        }
    }

    public void OnClimb(InputAction.CallbackContext context)
    {
        if (!UIManager.Instance.menuActivated)
        {
            VerticalInput = context.ReadValue<Vector2>();
            Animator.SetBool(AnimationString.upInput, VerticalInput.y > 0);
        }
        else
        {
            VerticalInput = Vector2.zero;
        }
    }

    public void OnWallHang(InputAction.CallbackContext context)
    {
        if (!UIManager.Instance.menuActivated)
        {
            if (context.started && CanMove && TouchingDirections.IsGrabWallDetected && !TouchingDirections.IsGrounded && !IsWallHanging && CurrentStamina >= wallHangStaminaCost)
            {
                Animator.SetTrigger(AnimationString.wallHangTrigger);
                CoroutineManager.Instance.StartCoroutineManager(WallHanging());
            }
        }
    }

    public void OnHeal(InputAction.CallbackContext context)
    {
        if (!UIManager.Instance.menuActivated)
        {
            if (context.started && IsAlive && CanMove && TouchingDirections.IsGrounded && CurrentHealthPotion > 0)
            {
                Animator.SetTrigger(AnimationString.healTrigger);
            }
        }
    }

    public void OnSavePoint(InputAction.CallbackContext context)
    {
        if (!UIManager.Instance.menuActivated)
        {
            if (context.started && IsAlive && CanMove && TouchingDirections.IsGrounded && isInSavePoint)
            {
                Animator.SetTrigger(AnimationString.saveTrigger);
            }
        }
    }

    public void OnCollectItem(InputAction.CallbackContext context)
    {
        if (!UIManager.Instance.menuActivated)
        {
            if (context.started && hasItemInRange && Item.IsOnFloor)
            {
                Animator.SetTrigger(AnimationString.collectFloorTrigger);
            }
            else if (context.started && hasItemInRange && !Item.IsOnFloor)
            {
                Animator.SetTrigger(AnimationString.collectHalfTrigger);
            }
        }
    }

    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (context.started && !UIManager.Instance.menuActivated)
        {
            Time.timeScale = 0;
            UIManager.Instance.inventoryMenu.SetActive(true);
            UIManager.Instance.menuActivated = true;
        }
    }

    public void OnCloseInventory(InputAction.CallbackContext context)
    {
        if (context.started && UIManager.Instance.menuActivated)
        {
            Time.timeScale = 1;
            UIManager.Instance.inventoryMenu.SetActive(false);
            UIManager.Instance.menuActivated = false;
        }
    }

    public void OnPerformPrayer(InputAction.CallbackContext context)
    {
        if (context.started && PlayerEquipment != null)
        {
            PlayerEquipment.PerformPrayer();
        }
    }

    #endregion

    #region Player Coroutines

    private IEnumerator DisableCollision()
    {
        CompositeCollider2D PlatformCollider = CurrentOneWayPlatform.GetComponent<CompositeCollider2D>();
        Physics2D.IgnoreCollision(PlayerCollider, PlatformCollider);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(PlayerCollider, PlatformCollider, false);
    }

    private IEnumerator Knockback(Vector2 knockback)
    {
        // Apply knockback velocity
        LockVelocity = true;
        RB.velocity = new Vector2(knockback.x, RB.velocity.y + knockback.y);
        yield return new WaitForSeconds(0.5f);
        LockVelocity = false;
    }

    private IEnumerator ApplySlowMotion()
    {
        Time.timeScale = slowMotionFactor;
        yield return new WaitForSecondsRealtime(slowMotionDuration);
        Time.timeScale = 1f;
    }

    private IEnumerator Dashing()
    {
        _currentStamina -= dashStaminaCost;
        IsInvincible = true;
        IsDashing = true;
        canDash = false;
        float originalGravity = RB.gravityScale;
        RB.gravityScale = 0;
        GhostTrail.StartGhostTrail();

        float dashDirection = IsFacingRight ? 1f : -1f;
        float dashEndTime = Time.time + dashTime;
        float dashDelay = 0.15f;
        float afterDashTime = 0.5f;

        yield return new WaitForSeconds(dashDelay);
        RB.velocity = new Vector2(dashDirection * (dashPower + dashPowerBuff), 0f);

        // Check for ground while dashing
        while (Time.time < dashEndTime)
        {
            if (!TouchingDirections.IsGrounded)
            {
                GhostTrail.StopGhostTrail();
                IsDashing = false;
                RB.gravityScale = originalGravity;
                IsDashed = true;

                yield return new WaitForSeconds(afterDashTime);
                IsInvincible = false;
                IsDashed = false;

                yield return new WaitForSeconds(dashCooldown);
                canDash = true;

                yield break;
            }
            yield return null;
        }

        if (HorizontalInput.x == 0 && IsDashing)
        {
            RB.velocity = new Vector2(Mathf.Lerp(RB.velocity.x, 0, dashStopRate), RB.velocity.y);
        }

        GhostTrail.StopGhostTrail();
        IsDashing = false;
        IsInvincible = false;
        RB.gravityScale = originalGravity;
        IsDashed = true;

        yield return new WaitForSeconds(afterDashTime);
        IsDashed = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator WallJumping()
    {
        IsWallJumping = true;
        RB.velocity = new Vector2(wallJumpingDirection * (wallJumpingPower.x + wallJumpPowerBuff), wallJumpingPower.y + wallJumpPowerBuff);
        wallJumpingCounter = 0f;

        float jumpDirection = IsFacingRight ? 1f : -1f;

        if (jumpDirection != wallJumpingDirection)
        {
            TurnPlayer();
        }

        // Start or restart the speed boost coroutine
        if (wallJumpSpeedBoostCoroutine != null)
        {
            CoroutineManager.Instance.StopCoroutineManager(wallJumpSpeedBoostCoroutine);
        }
        wallJumpSpeedBoostCoroutine = CoroutineManager.Instance.StartCoroutineManager(WallJumpSpeedBoost());

        yield return new WaitForSeconds(wallJumpingDuration);
        IsWallJumping = false;
    }

    private IEnumerator WallHanging()
    {
        _currentStamina -= wallHangStaminaCost;
        IsWallHanging = true;
        RB.gravityScale = 0;

        // Determine the direction to the wall
        Vector2 directionToWall = IsFacingRight ? Vector2.right : Vector2.left;

        // Move the player towards the wall
        float moveToWallSpeed = 100f;
        while (!TouchingDirections.IsOnWall && IsWallHanging)
        {
            if (TouchingDirections.IsGrabWallDetected && !IsWallJumping)
            {
                RB.velocity = new Vector2(directionToWall.x * moveToWallSpeed, 0);
            }
            yield return null;
        }

        // Slide down the wall a little
        float startTime = Time.time;
        while (Time.time < startTime + wallSlideDuration && IsWallHanging)
        {
            float t = (Time.time - startTime) / wallSlideDuration;
            float slideSpeed = Mathf.Lerp(wallSlideSpeed, 0, t);
            RB.velocity = new Vector2(0, -slideSpeed);
            yield return null;
        }

        // Stop slide down
        while (IsWallHanging)
        {
            RB.velocity = Vector2.zero;
            yield return null;
        }
    }

    private IEnumerator WallJumpSpeedBoost()
    {
        // Reset move speed to original if boost was already active
        if (isWallJumpSpeedBoostActive)
        {
            moveSpeed = originalMoveSpeed;
        }
        else
        {
            originalMoveSpeed = moveSpeed;
        }

        moveSpeed += wallJumpSpeedBoost;
        isWallJumpSpeedBoostActive = true;

        yield return new WaitForSeconds(wallJumpSpeedBoostDuration); // Speed boost duration

        float elapsedTime = 0f;
        float decreaseDuration = 1f; // Duration to smoothly decrease the speed boost
        float initialBoost = wallJumpSpeedBoost;

        while (elapsedTime < decreaseDuration)
        {
            elapsedTime += Time.deltaTime;
            moveSpeed -= initialBoost * (Time.deltaTime / decreaseDuration);
            yield return null;
        }

        moveSpeed = originalMoveSpeed; // Ensure the boost is completely removed
        isWallJumpSpeedBoostActive = false;
    }

    #endregion

    #region Player Collision check

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = true;
            ladderCollider = collision;
        }

        if (collision.CompareTag("SavePoint"))
        {
            isInSavePoint = true;
            CheckPointManager = collision.GetComponent<CheckPointManager>();
        }

        if (collision.CompareTag("Item"))
        {
            hasItemInRange = true;
            Item = collision.GetComponent<Collectable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = false;
            IsClimbing = false;
            RB.gravityScale = OriginalGravityScale;
            ladderCollider = null;
        }

        if (collision.CompareTag("SavePoint"))
        {
            isInSavePoint = false;
            CheckPointManager = null;
        }

        if (collision.CompareTag("Item"))
        {
            hasItemInRange = false;
            Item = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            CurrentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            CurrentOneWayPlatform = null;
        }
    }

    #endregion

}
