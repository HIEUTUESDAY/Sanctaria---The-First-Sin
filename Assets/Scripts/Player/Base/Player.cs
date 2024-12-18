using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IPlayerDamageable, IPlayerMoveable
{
    #region Player variables

    [Header("Moving")]
    [SerializeField] public float moveSpeed = 10f;
    [SerializeField] private float groundSpeedAceleration = 75f;
    [SerializeField] private float airSpeedAceleration = 100f;
    [SerializeField] private float stopRate = 0.2f;
    [Space(5)]

    [Header("Jumping")]
    [SerializeField] private bool canJump = true;
    [SerializeField] public float jumpPower = 20f;
    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;
    [SerializeField] private float jumpBufferTime = 0.1f;
    private float jumpBufferTimeCounter;
    [SerializeField] private float maximumFallSpeed = -40f;
    [Space(5)]

    [Header("Dashing")]
    [SerializeField] private bool canDash = true;
    [SerializeField] public float dashPower = 20f;
    private float dashTime = 0.5f;
    [SerializeField] private float dashCooldown = 2f;
    private float dashStopRate = 5f;
    [Space(5)]

    [Header("Ladder Climbing")]
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private bool isOnLadder;
    private Collider2D LadderCollider;
    [SerializeField] private Collider2D LadderPlatformCollider;
    [Space(5)]

    [Header("Wall Hanging And Jumping")]
    [SerializeField] private bool canWallHang = true;
    [SerializeField] private float wallSlideSpeed = 1f;
    [SerializeField] private float wallSlideDuration = 0.5f;
    private float wallJumpingDirection;
    private float wallJumpingTime = 1f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.2f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(5f, 22f);
    [Space(5)]

    [Header("Wall Jump Speed Boost")]
    [SerializeField] private float wallJumpSpeedBoost = 3f;
    [SerializeField] private float wallJumpSpeedBoostDuration = 2f;
    private bool isWallJumpSpeedBoostActive = false;
    private Coroutine wallJumpSpeedBoostCoroutine;
    private float originalMoveSpeed;
    [Space(5)]

    [Header("Mana Regen")]
    [SerializeField] public float manaRegen = 0.5f;
    [Space(5)]

    [Header("Ivincible")]
    [SerializeField] private float invincibleTime = 2f;
    private float timeSinceHit = 0;
    [Space(5)]

    [Header("Collect Item")]
    [SerializeField] private bool hasItemInRange;
    private ItemCollectable Item;
    [Space(5)]

    [Header("CheckPoint")]
    [SerializeField] private bool isInCheckpoint;
    [SerializeField] public bool isKneelInCheckpoint;
    [SerializeField] private CheckpointManager Checkpoint;
    [Space(5)]

    [Header("NPCs")]
    [SerializeField] private bool isAtNPC;
    [SerializeField] private NPC NPC;
    [SerializeField] public bool isInteractWithNPC = false;
    [Space(5)]

    [Header("Shop")]
    [SerializeField] private bool isAtShop;
    [SerializeField] private ShopDoorSceneChanger ShopDoorSceneChanger;
    [Space(5)]

    [Header("Buy Item")]
    [SerializeField] private bool canBuyItem;
    [SerializeField] public ItemBuyable ItemBuyable;
    [Space(5)]

    [Header("Camera Shake")]
    [SerializeField] private float slowMotionDuration = 0.5f;
    [SerializeField] private float slowMotionFactor = 0.2f;
    [Space(5)]

    [Header("Status title")]
    public GameObject playerDeathTitle;
    public GameObject defeatBossTitle;
    [Space(5)]

    [Header("Slopes handle")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float slopeCheckDistance;
    [SerializeField] private float maxSlopeAngle;
    private bool isOnSlope;
    private bool canWalkOnSlope;
    private Vector2 colliderSize;
    private Vector2 slopeNormalPerp; 
    private float slopeDownAngle;
    private float slopeDownAngleOld;
    private float slopeSideAngle;
    [SerializeField] private PhysicsMaterial2D nonFriction;
    [SerializeField] private PhysicsMaterial2D fullFriction;
    [Space(5)]

    [Header("Boss Fight")]
    [SerializeField] public bool isDefeatedBoss = false;
    [Space(5)]

    [Header("Hit Flash Material")]
    [SerializeField] private Material hitFlashMaterial;
    private SpriteRenderer SR;
    private Material originalMaterial;
    private Coroutine hitFlashCoroutine;
    [Space(5)]

    [SerializeField] private float fallSpeedYDampingChangeThreshold;
    [SerializeField] private float originalGravityScale;
    public Animator Animator;
    public PlayerInput PlayerInput;
    public TouchingDirections TouchingDirections;
    public CapsuleCollider2D PlayerCollider;
    public CameraFollowObject CameraFollowObject;
    public GhostTrail GhostTrail;
    public PlayerHealthBarManager HealthBar;
    public CinemachineImpulseSource ImpulseSource;
    public PlayerEquipment PlayerEquipment;

    #endregion

    #region Player Equipment

    [Header("Heart Buffs")]
    public float damageBuff = 0f;
    public float defenseBuff = 0f;
    public float healthBuff = 0f;
    public float healthRegenBuff = 0f;
    public float manaBuff = 0f;
    public float manaRegenBuff = 0f;
    public float moveSpeedBuff = 0f;
    public float jumpPowerBuff = 0f;
    public float wallJumpPowerBuff = 0f;
    public float dashPowerBuff = 0f;
    [Space(5)]

    [Header("Prayer Mana Cost")]
    public float prayerManaCost = 0f;
    public float prayerCooldownTime = 5f;
    public float prayerCooldown = 0f;

    #endregion

    #region Player animation properties

    public bool IsWaitForEnter
    {
        get
        {
            return Animator.GetBool(AnimationString.isWaitForEnter);
        }
        set
        {
            Animator.SetBool(AnimationString.isWaitForEnter, value);
        }
    }

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

    public bool IsKilledBySpikes
    {
        get
        {
            return Animator.GetBool(AnimationString.isKilledBySpikes);
        }
        set
        {
            Animator.SetBool(AnimationString.isKilledBySpikes, value);
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

    private float _maxMana = 100f;
    public float MaxMana 
    {
        get => _maxMana + manaBuff;
        set
        {
            _maxMana = value;
        }
    }

    private float _currentMana;
    public float CurrentMana
    {
        get => _currentMana;
        set
        {
            _currentMana = value;
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
    public bool CanMove 
    {
        get
        {
            return Animator.GetBool(AnimationString.canMove);
        }
        set
        {
            Animator.SetBool(AnimationString.canMove, value);
        }
    }

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

    #region Player events

    [field: SerializeField] public UnityEvent<float, Vector2> DamageableHit { get; set; }
    [field: SerializeField] public UnityEvent DamageableDead { get; set; }

    #endregion

    #region Player Singleton implementation

    public static Player Instance;

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
        SR = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        PlayerInput = GetComponent<PlayerInput>();
        TouchingDirections = GetComponent<TouchingDirections>();
        PlayerCollider = GetComponent<CapsuleCollider2D>();
        GhostTrail = GetComponent<GhostTrail>();
        ImpulseSource = GetComponent<CinemachineImpulseSource>();
        PlayerEquipment = GetComponent<PlayerEquipment>();
    }

    private void Start()
    {
        originalMaterial = SR.material;
        originalGravityScale = RB.gravityScale;
        colliderSize = PlayerCollider.size;
    }

    private void Update()
    {
        SetFacingCheck();
        GroundCheck();
        SlopeCheck();
        FallCheck();
        LadderClimbCheck();
        YDampingCheck();
        WallHangCheck();
        SetMaxHealthAndMana();
        UpdateHealthBar();
        UpdatePrayerCooldown();
        BeInvisible();
        InteractCheck();
    }

    private void FixedUpdate()
    {
        Move();
        WallHang();
        LadderClimb();
        EnterLadder();
    }

    #region IPlayerDamageable functions

    public void TakeDamage(float damage, Vector2 knockback, Vector2 hitDirection, int attackType)
    {
        if (IsAlive && !IsInvincible)
        {
            float damageTaken = damage - defenseBuff;
            CurrentHealth -= damageTaken;
            WasHit = true;

            if(hitFlashCoroutine != null)
            {
                StopCoroutine(hitFlashCoroutine);
            }
            hitFlashCoroutine = StartCoroutine(HitFlash());

            StartCoroutine(ApplySlowMotion());
            CameraShakeManager.Instance.CameraShake(ImpulseSource);
            Animator.SetTrigger(AnimationString.hitTrigger);

            PlayHitSound();

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
        if (IsFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

            CameraFollowObject.CallTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

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
                StartCoroutine(DisableWallHang());

            }
            else if (moveInput.x < 0 && IsFacingRight)
            {
                TurnPlayer();
                StartCoroutine(DisableWallHang());

            }
        }
    }

    #endregion

    #region UI health bar functions

    public void SetMaxHealthAndMana()
    {
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }

        if (CurrentMana > MaxMana)
        {
            CurrentMana = MaxMana;
        }
    }

    public void UpdateHealthBar()
    {
        HealthBar.SetMaxHealth(MaxHealth);
        HealthBar.SetMaxMana(MaxMana);
        HealthBar.SetHealth(CurrentHealth);
        HealthBar.SetMana(CurrentMana);
        HealthBar.SetHealthPotions(CurrentHealthPotion);
    }

    private void UpdatePrayerCooldown()
    {
        if (prayerCooldown > 0)
        {
            prayerCooldown -= Time.deltaTime;
        }
        else if (prayerCooldown <= 0)
        {
            prayerCooldown = 0;
        }
    }

    #endregion

    #region Player movement functions

    private void Move()
    {
        if (!UIManager.Instance.menuActivated && !SceneLoadManager.Instance.IsLoading && !GameManager.Instance.isRespawn)
        {
            if (!LockVelocity && !IsWallJumping && !IsWallHanging && !IsClimbing)
            {
                if (!IsMoving && !IsDashing)
                {
                    RB.velocity = new Vector2(Mathf.Lerp(RB.velocity.x, 0, stopRate), RB.velocity.y);
                    _currentSpeed = 0;
                }
                else if (IsMoving && !IsDashing)
                {
                    if (TouchingDirections.IsGrounded && !IsJumping && !isOnSlope)
                    {
                        RB.velocity = new Vector2(HorizontalInput.x * CurrentSpeed, 0f);
                    }
                    else if (TouchingDirections.IsGrounded && !IsJumping && isOnSlope && canWalkOnSlope)
                    {
                        RB.velocity = new Vector2(-HorizontalInput.x * CurrentSpeed * slopeNormalPerp.x, -HorizontalInput.x * CurrentSpeed * slopeNormalPerp.y);
                    }
                    else if (!TouchingDirections.IsGrounded)
                    {
                        RB.velocity = new Vector2(HorizontalInput.x * CurrentSpeed, RB.velocity.y);
                    }
                }
            }
        }
    }

    private void LadderClimb()
    {
        if (!UIManager.Instance.menuActivated)
        {
            if (IsClimbing && !LockVelocity)
            {
                RB.gravityScale = 0f;
                RB.velocity = new Vector2(0f, VerticalInput.y * climbSpeed);

                if (LadderCollider != null)
                {
                    Vector3 targetPosition = new Vector3(LadderCollider.bounds.center.x, transform.position.y, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, 10f * Time.deltaTime);
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

    #region Player animation sound effects

    public void PlayFootstepSound()
    {
        SoundFXManager.Instance.Play2DRandomSoundFXClip(SoundFXManager.Instance.playerWalkSoundClips, transform, 1f);
    }

    public void PlayAttackSound()
    {
        SoundFXManager.Instance.Play2DRandomSoundFXClip(SoundFXManager.Instance.playerAttackSoundClip, transform, 1f);
    }

    public void PlayLadderClimbSound()
    {
        SoundFXManager.Instance.Play2DRandomSoundFXClip(SoundFXManager.Instance.playerLadderClimbSoundClips, transform, 1f);
    }

    public void PlaySpawnSound()
    {
        SoundFXManager.Instance.Play2DSoundFXClip(SoundFXManager.Instance.playerSpawnSoundClip, transform, 1f);
    }

    public void PlaySpikeDeathSound()
    {
        SoundFXManager.Instance.Play2DSoundFXClip(SoundFXManager.Instance.playerSpikeDeathSoundClip, transform, 1f);
    }

    public void PlayDeathSound()
    {
        SoundFXManager.Instance.Play2DSoundFXClip(SoundFXManager.Instance.playerDeathSoundClip, transform, 1f);
    }

    public void PlayJumpSound()
    {
        SoundFXManager.Instance.Play2DSoundFXClip(SoundFXManager.Instance.playerJumpAndLandingSoundClip, transform, 1f);
    }

    public void PlayHitSound()
    {
        SoundFXManager.Instance.Play2DSoundFXClip(SoundFXManager.Instance.playerHitSoundClips, transform, 1f);
    }

    public void PlayUsePrayerSound()
    {
        SoundFXManager.Instance.Play2DSoundFXClip(SoundFXManager.Instance.playerUseSpellClip, transform, 1f);
    }

    public void PlayDashSound()
    {
        SoundFXManager.Instance.Play2DSoundFXClip(SoundFXManager.Instance.playerDashSoundClip, transform, 1f);
    }

    public void PlayHealingSound()
    {
        SoundFXManager.Instance.Play2DSoundFXClip(SoundFXManager.Instance.playerHealingClip, transform, 1f);
    }

    public void PlayActiveCheckpointSound()
    {
        SoundFXManager.Instance.Play2DSoundFXClip(SoundFXManager.Instance.playerActiveCheckpointClip, transform, 1f);
    }

    public void PlayHealthRestoreSound()
    {
        SoundFXManager.Instance.Play2DSoundFXClip(SoundFXManager.Instance.playerHealthRestoreSoundClip, transform, 1f);
    }

    public void PlayWallGrabSound()
    {
        SoundFXManager.Instance.Play2DSoundFXClip(SoundFXManager.Instance.playerWallGrabSoundClip, transform, 1f);
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
            CurrentHealthPotion -= 1;
            CharacterEvent.characterHealed(gameObject, actualHeal);
        }
        else
        {
            return;
        }
    }

    public void RestoreHealthAndPotion()
    {
        PlayHealthRestoreSound();
        CurrentHealth = MaxHealth;
        CurrentHealthPotion = MaxHealthPotion;
    }

    public void ActivateSavePoint()
    {
        if (Checkpoint != null)
        {
            Checkpoint.ActiveCheckpoint();
        }
    }

    public void ShowDeathTitle()
    {
        DeathFadeInTitle deathFadeIn = playerDeathTitle.GetComponent<DeathFadeInTitle>();

        if (deathFadeIn != null)
        {
            deathFadeIn.StartFadeIn();
        }
    }

    public void CollectItem()
    {
        if (hasItemInRange && Item != null)
        {
            Item.CollectItem();
        }
    }

    public void PerformPrayer()
    {
        if(PlayerEquipment.equippedPrayer.itemName != null)
        {
            PlayerEquipment.PerformPrayer();
            _currentMana -= prayerManaCost;
        }
    }

    public void EnterLadderFromGround()
    {
        if (LadderPlatformCollider != null)
        {
            Transform ladderTransform = LadderPlatformCollider.transform.parent;
            if (ladderTransform != null)
            {
                Animator.SetBool(AnimationString.enterLadderTrigger, false);
                LadderCollider = ladderTransform.GetComponentInChildren<Collider2D>();
                Vector3 targetPosition = new Vector3(LadderCollider.bounds.center.x, LadderPlatformCollider.bounds.min.y - (PlayerCollider.size.y / 4), transform.position.z);
                transform.position = targetPosition;
                isOnLadder = true;
                IsClimbing = true;
            }
        }
    }

    public void OpenCheckpointMenu()
    {
        if (!UIManager.Instance.menuActivated && !UIManager.Instance.checkpointMenu.activeSelf)
        {
            Time.timeScale = 0;
            UIManager.Instance.checkpointMenu.SetActive(true);
            UIManager.Instance.menuActivated = true;
        }
    }

    public void EnterShop()
    {
        if (ShopDoorSceneChanger != null)
        {
            ShopDoorSceneChanger.ChangeScene();
        }
    }

    #endregion

    #region Tutorials functions

    public void UseHealthPotionsTutorial()
    {
        if (!TutorialManager.Instance.healthPotionTutor)
        {
            Time.timeScale = 0;
            UIManager.Instance.healthPotionsTutorHUD.SetActive(true);
            UIManager.Instance.menuActivated = true;
            TutorialManager.Instance.healthPotionTutor = true;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
    }

    public void InventoryTutorial()
    {
        if (!TutorialManager.Instance.inventoryTutor)
        {
            Time.timeScale = 0;
            UIManager.Instance.inventoryTutorHUD.SetActive(true);
            UIManager.Instance.menuActivated = true;
            TutorialManager.Instance.inventoryTutor = true;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
    }

    public void AttackTutorial()
    {
        if (!TutorialManager.Instance.attackTutor)
        {
            Time.timeScale = 0;
            UIManager.Instance.attackTutorHUD.SetActive(true);
            UIManager.Instance.menuActivated = true;
            TutorialManager.Instance.attackTutor = true;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
    }

    public void MapTutorial()
    {
        if (!TutorialManager.Instance.mapTutor)
        {
            Time.timeScale = 0;
            UIManager.Instance.mapTutorHUD.SetActive(true);
            UIManager.Instance.menuActivated = true;
            TutorialManager.Instance.mapTutor = true;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
    }

    public void PrayerTutorial()
    {
        if (!TutorialManager.Instance.prayerTutor)
        {
            Time.timeScale = 0;
            UIManager.Instance.prayerTutorHUD.SetActive(true);
            UIManager.Instance.menuActivated = true;
            TutorialManager.Instance.prayerTutor = true;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
    }

    public void HeartTutorial()
    {
        if (!TutorialManager.Instance.heartTutor)
        {
            Time.timeScale = 0;
            UIManager.Instance.heartTutorHUD.SetActive(true);
            UIManager.Instance.menuActivated = true;
            TutorialManager.Instance.heartTutor = true;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
    }

    public void DashTutorial()
    {
        if (!TutorialManager.Instance.dashTutor)
        {
            Time.timeScale = 0;
            UIManager.Instance.dashTutorHUD.SetActive(true);
            UIManager.Instance.menuActivated = true;
            TutorialManager.Instance.dashTutor = true;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
    }

    public void WallClimbTutorial()
    {
        if (!TutorialManager.Instance.wallClimbTutor)
        {
            Time.timeScale = 0;
            UIManager.Instance.wallClimbTutorHUD.SetActive(true);
            UIManager.Instance.menuActivated = true;
            TutorialManager.Instance.wallClimbTutor = true;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
    }

    public void CheckpointTutorial()
    {
        if (!TutorialManager.Instance.checkpointTutor)
        {
            Time.timeScale = 0;
            UIManager.Instance.checkpointTutorHUD.SetActive(true);
            UIManager.Instance.menuActivated = true;
            TutorialManager.Instance.checkpointTutor = true;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
    }

    #endregion

    #region Player envent system functions

    public void OnHit(float damage, Vector2 knockback)
    {
        StartCoroutine(Knockback(knockback));
    }

    public void OnDeath()
    {

    }

    #endregion

    #region Player checking functions

    private void SetFacingCheck()
    {
        CameraFollowObject = FindObjectOfType<CameraFollowObject>();

        if (IsMoving && CanMove)
        {
            SetFacing(HorizontalInput);
        }
    }

    private void GroundCheck()
    {
        if (TouchingDirections.IsGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void FallCheck()
    {
        if (!TouchingDirections.IsGrounded && RB.velocity.y < maximumFallSpeed)
        {
            fallSpeedYDampingChangeThreshold = CameraManger.Instance._fallSpeedYDampingChangeThreshold;
            RB.velocity = new Vector2(RB.velocity.x, maximumFallSpeed);
        }

        if (IsJumping && !TouchingDirections.IsGrounded && RB.velocity.y < 0f)
        {
            IsJumping = false;
        }
    }

    private void LadderClimbCheck()
    {
        if (isOnLadder && !IsJumping && !LockVelocity && VerticalInput.y != 0f && RB.velocity.y <= 0f)
        {
            IsClimbing = true;
        }

        if (IsClimbing && isOnLadder && LadderPlatformCollider != null)
        {
            StartCoroutine(DisableLadderPlatformCollision());
        }

        if (IsClimbing && isOnLadder && VerticalInput.y == 0f)
        {
            Animator.speed = 0f;
        }
        else if (IsClimbing && isOnLadder && VerticalInput.y != 0f)
        {
            Animator.speed = 1f;
        }
        else
        {
            Animator.speed = 1f;
        }
        
        if (!isOnLadder)
        {
            IsClimbing = false;
        }
    }

    private void EnterLadder()
    {
        if (TouchingDirections.IsGrounded && !IsClimbing && !isOnLadder && LadderPlatformCollider != null && VerticalInput.y < 0f)
        {
            Transform ladderTransform = LadderPlatformCollider.transform.parent;
            if (ladderTransform != null)
            {
                LadderCollider = ladderTransform.GetComponentInChildren<Collider2D>();
                transform.position = new Vector3(LadderCollider.bounds.center.x, transform.position.y, transform.position.z);
                Animator.SetTrigger(AnimationString.enterLadderTrigger);
            }
        }
    }

    private void WallHangCheck()
    {
        if (!TouchingDirections.IsGrabWallDetected || IsWallJumping || Animator.GetBool(AnimationString.hitTrigger))
        {
            IsWallHanging = false;
            RB.gravityScale = originalGravityScale;
        }
    }

    private void YDampingCheck()
    {
        Animator.SetFloat(AnimationString.yVelocity, RB.velocity.y);

        //if player falling past the certain speed threshold
        if (RB.velocity.y < fallSpeedYDampingChangeThreshold && !CameraManger.Instance.IsLerpingYDamping && !CameraManger.Instance.LerpedFromPlayerFalling)
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

    private void SlopeCheck()
    {
        Vector2 checkPosition =  transform.position - new Vector3 (0.0f, colliderSize.y / 2);

        SlopeCheckHorizontal(checkPosition);
        SlopeCheckVertical(checkPosition);
    }

    private void SlopeCheckHorizontal(Vector2 checkPosition)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPosition, transform.right, slopeCheckDistance, groundLayer);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPosition, -transform.right, slopeCheckDistance, groundLayer);

        if (slopeHitFront)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, transform.up);
        }
        else if (slopeHitBack)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, transform.up);
        }
        else
        {
            isOnSlope = false;
            slopeSideAngle = 0f;
        }
    }

    private void SlopeCheckVertical(Vector2 checkPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, -transform.up, slopeCheckDistance, groundLayer);

        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;
            slopeDownAngle = Vector2.Angle(hit.normal, transform.up);

            if (slopeDownAngle != slopeDownAngleOld)
            {
                isOnSlope = true;
            }

            slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        if (isOnSlope && HorizontalInput.x == 0f && TouchingDirections.IsGrounded && canWalkOnSlope)
        {
            RB.sharedMaterial = fullFriction;
        }
        else
        {
            RB.sharedMaterial = nonFriction;
        }
    }

    private void InteractCheck()
    {
        if (isInteractWithNPC)
        {
            PlayerInput.enabled = false;
        }
        else
        {
            PlayerInput.enabled = true;
        }
    }

    #endregion

    #region Player Input functions

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!UIManager.Instance.menuActivated)
        {
            if (IsAlive)
            {
                HorizontalInput = context.ReadValue<Vector2>();
                IsMoving = HorizontalInput != Vector2.zero;
            }
        }
        else
        {
            IsMoving = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!UIManager.Instance.menuActivated)
        {
            if (context.started && IsClimbing && !IsJumping)
            {
                PlayJumpSound();
                IsClimbing = false;
                IsJumping = true;
                RB.gravityScale = originalGravityScale;
                RB.velocity = new Vector2(RB.velocity.x, jumpPower + jumpPowerBuff);
            }
            else if (context.started && !IsClimbing && !IsJumping && canJump)
            {
                jumpBufferTimeCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferTimeCounter -= Time.deltaTime;
            }

            if (context.started && wallJumpingCounter > 0f && IsWallHanging)
            {
                PlayJumpSound();
                StartCoroutine(WallJumping());
            }

            if (jumpBufferTimeCounter > 0f && coyoteTimeCounter > 0f && CanMove && slopeDownAngle <= maxSlopeAngle)
            {
                PlayJumpSound();
                IsDashing = false;
                IsJumping = true;
                RB.velocity = new Vector2(RB.velocity.x, jumpPower + jumpPowerBuff);
                jumpBufferTimeCounter = 0f;
            }

            if (context.canceled && RB.velocity.y > 0)
            {
                coyoteTimeCounter = 0f;
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!SceneLoadManager.Instance.IsFadingIn && !SceneLoadManager.Instance.IsFadingOut && !SceneLoadManager.Instance.IsLoading)
        {
            if (!UIManager.Instance.menuActivated)
            {
                if (context.started && IsAlive && !IsWallHanging && !IsDashing && !IsClimbing)
                {
                    Animator.SetTrigger(AnimationString.attackTrigger);
                }
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!UIManager.Instance.menuActivated)
        {
            if (context.started && IsAlive && canDash && CanMove && TouchingDirections.IsGrounded && !IsDashing && !IsClimbing)
            {
                StartCoroutine(Dashing());
            }
        }
    }

    public void OnClimb(InputAction.CallbackContext context)
    {
        if (!UIManager.Instance.menuActivated)
        {
            if (IsAlive)
            {
                VerticalInput = context.ReadValue<Vector2>();
                Animator.SetBool(AnimationString.upInput, VerticalInput.y > 0);
            }
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
            if (context.started && IsAlive && CanMove && TouchingDirections.IsGrabWallDetected && !TouchingDirections.IsGrounded && !IsWallHanging)
            {
                if (canWallHang)
                {
                    Animator.SetTrigger(AnimationString.wallHangTrigger);
                    StartCoroutine(WallHanging());
                }
            }
        }
    }

    public void OnHeal(InputAction.CallbackContext context)
    {
        if (!SceneLoadManager.Instance.IsFadingIn && !SceneLoadManager.Instance.IsFadingOut && !SceneLoadManager.Instance.IsLoading)
        {
            if (!UIManager.Instance.menuActivated)
            {
                if (context.started && IsAlive && CanMove && !IsDashing && TouchingDirections.IsGrounded && CurrentHealthPotion > 0)
                {
                    Animator.SetTrigger(AnimationString.healTrigger);
                }
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!SceneLoadManager.Instance.IsFadingIn && !SceneLoadManager.Instance.IsFadingOut && !SceneLoadManager.Instance.IsLoading)
        {
            if (!UIManager.Instance.menuActivated)
            {
                if (context.started && hasItemInRange && Item.isOnFloor && TouchingDirections.IsGrounded)
                {
                    transform.position = new Vector3(Item.transform.position.x, transform.position.y, transform.position.z);
                    Animator.SetTrigger(AnimationString.collectFloorTrigger);
                }
                else if (context.started && hasItemInRange && !Item.isOnFloor && TouchingDirections.IsGrounded)
                {
                    Animator.SetTrigger(AnimationString.collectHalfTrigger);
                }
                else if (context.started && IsWaitForEnter)
                {
                    IsWaitForEnter = false;
                    Animator.SetTrigger(AnimationString.risingTrigger);
                    if (!TutorialManager.Instance.enterPlayerTutor)
                    {
                        TutorialManager.Instance.enterPlayerTutor = true;
                    }

                    // save game and start auto save after enter player
                    Player player = this;
                    InventoryManager inventoryManager = InventoryManager.Instance;
                    MapRoomManager mapRoomManager = MapRoomManager.Instance;
                    TutorialManager tutorialManager = TutorialManager.Instance;
                    SceneDataManager sceneDataManager = SceneDataManager.Instance;
                    Transform newGamePosition = GameObject.Find("NewGamePosition").GetComponent<Transform>();

                    if (player != null && inventoryManager != null && mapRoomManager != null && sceneDataManager != null && tutorialManager != null)
                    {
                        PlayerData playerData = new PlayerData(player);
                        PlayerTutorialData playerTutorialData = tutorialManager.GetTutorialData();
                        PlayerCheckpointData playerCheckpointData = new PlayerCheckpointData("Brother Tower", SceneManager.GetActiveScene().name, newGamePosition.position);
                        PlayerInventoryData playerInventoryData = new PlayerInventoryData
                        (
                            inventoryManager.GetTearsAmount(),
                            inventoryManager.GetQuestItemsInventory(),
                            inventoryManager.GetHeartsInventory(),
                            inventoryManager.GetPrayersInventory(),
                            inventoryManager.GetHeartEquipment(),
                            inventoryManager.GetPrayerEquipment()
                        );
                        PlayerMapData playerMapData = new PlayerMapData(mapRoomManager.GetMaps());
                        PlayerSceneData playerSceneData = new PlayerSceneData(sceneDataManager.GetSceneDataList());

                        GameManager.Instance.SaveGame(playerData, playerTutorialData, playerCheckpointData, playerInventoryData, playerMapData, playerSceneData);
                        GameManager.Instance.StartAutoSave();
                    }
                }
                else if (context.started && isAtNPC && TouchingDirections.IsGrounded)
                {
                    isInteractWithNPC = true;
                }
                else if (context.started && isInCheckpoint && TouchingDirections.IsGrounded)
                {
                    if (IsFacingRight)
                    {
                        transform.position = new Vector3(Checkpoint.transform.position.x - 1f, transform.position.y, transform.position.z);
                    }
                    else
                    {
                        transform.position = new Vector3(Checkpoint.transform.position.x + 1f, transform.position.y, transform.position.z);
                    }

                    Animator.SetTrigger(AnimationString.saveTrigger);
                }
                else if (context.started && isAtShop && TouchingDirections.IsGrounded)
                {
                    transform.position = new Vector3(ShopDoorSceneChanger.transform.position.x, transform.position.y, transform.position.z);
                    Animator.SetTrigger(AnimationString.enterDoorTrigger);
                }
                else if (context.started && canBuyItem && TouchingDirections.IsGrounded)
                {
                    transform.position = new Vector3(ItemBuyable.transform.position.x, transform.position.y, transform.position.z);
                    RB.velocity = Vector3.zero;
                    ItemBuyable.InspectItem();
                }
            }
        }
    }

    public void OnOpenInventoryMenu(InputAction.CallbackContext context)
    {
        if (!SceneLoadManager.Instance.IsFadingIn && !SceneLoadManager.Instance.IsFadingOut && !SceneLoadManager.Instance.IsLoading)
        {
            if (context.started && IsAlive && !UIManager.Instance.menuActivated && !UIManager.Instance.inventoryMenu.activeSelf && !IsWaitForEnter && !SceneLoadManager.Instance.IsFadingIn && !SceneLoadManager.Instance.IsFadingOut && !SceneLoadManager.Instance.IsLoading)
            {
                Time.timeScale = 0;
                UIManager.Instance.inventoryMenu.SetActive(true);
                UIManager.Instance.menuActivated = true;
                SoundFXManager.Instance.PlayChangeTabSound();
            }
            else if (context.started && IsAlive && UIManager.Instance.menuActivated && !UIManager.Instance.inventoryMenu.activeSelf && UIManager.Instance.checkpointMenu.activeSelf)
            {
                isKneelInCheckpoint = true;
                UIManager.Instance.checkpointMenu.SetActive(false);
                UIManager.Instance.inventoryMenu.SetActive(true);
                SoundFXManager.Instance.PlayChangeTabSound();

                for (int i = 0; i < InventoryManager.Instance.Inventories.Length; i++)
                {
                    if (InventoryManager.Instance.Inventories[i].name == "Hearts")
                    {
                        InventoryManager.Instance.Inventories[i].SetActive(true);
                        InventoryManager.Instance.currentInventoryIndex = i;
                    }
                    else
                    {
                        InventoryManager.Instance.Inventories[i].SetActive(false);
                    }
                }
            }
        }
    }

    public void OnOpenMapMenu(InputAction.CallbackContext context)
    {
        if (!SceneLoadManager.Instance.IsFadingIn && !SceneLoadManager.Instance.IsFadingOut && !SceneLoadManager.Instance.IsLoading)
        {

            if (context.started && IsAlive && !UIManager.Instance.menuActivated && !UIManager.Instance.mapMenu.activeSelf && !IsWaitForEnter)
            {
                Time.timeScale = 0;
                UIManager.Instance.mapMenu.SetActive(true);
                UIManager.Instance.menuActivated = true;
                MapRoomManager.Instance.selectTeleportSlot.SetActive(false);
                MapRoomManager.Instance.teleportHUD.SetActive(false);
                if (!MapRoomManager.Instance.isInHiddenRoom)
                {
                    MapRoomManager.Instance.mapCenterPoint.SetActive(true);
                }
                else
                {
                    MapRoomManager.Instance.mapCenterPoint.SetActive(false);
                }
                MapRoomManager.Instance.mapHUD.SetActive(true);
                MapCenterPoint.Instance.SetCenterPoint();
                SoundFXManager.Instance.PlayChangeTabSound();
            }
            else if (context.started && IsAlive && UIManager.Instance.menuActivated && !UIManager.Instance.mapMenu.activeSelf && UIManager.Instance.checkpointMenu.activeSelf)
            {
                isKneelInCheckpoint = true;
                UIManager.Instance.checkpointMenu.SetActive(false);
                UIManager.Instance.mapMenu.SetActive(true);
                MapRoomManager.Instance.mapCenterPoint.SetActive(false);
                MapRoomManager.Instance.mapHUD.SetActive(false);
                MapRoomManager.Instance.selectTeleportSlot.SetActive(true);
                MapRoomManager.Instance.teleportHUD.SetActive(true);
                SelectTeleportSlot.Instance.SetCurrentRoomSelected();
                SoundFXManager.Instance.PlayChangeTabSound();
            }
        }
    }

    public void OnOpenOptionsMenu(InputAction.CallbackContext context)
    {
        if (!SceneLoadManager.Instance.IsFadingIn && !SceneLoadManager.Instance.IsFadingOut && !SceneLoadManager.Instance.IsLoading)
        {
            if (context.started && IsAlive && UIManager.Instance.menuActivated && UIManager.Instance.mapMenu.activeSelf && !IsWaitForEnter && !isKneelInCheckpoint)
            {
                Time.timeScale = 0;
                UIManager.Instance.mapMenu.SetActive(false);
                UIManager.Instance.optionsMenu.SetActive(true);
                UIManager.Instance.menuActivated = true;
                SoundFXManager.Instance.PlayChangeTabSound();
            }
        }
    }

    public void OnCloseMenus(InputAction.CallbackContext context)
    {
        if (context.started && UIManager.Instance.menuActivated && UIManager.Instance.inventoryMenu.activeSelf)
        {
            isKneelInCheckpoint = false;
            Time.timeScale = 1;
            UIManager.Instance.inventoryMenu.SetActive(false);
            UIManager.Instance.menuActivated = false;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
        else if (context.started && UIManager.Instance.menuActivated && UIManager.Instance.mapMenu.activeSelf)
        {
            isKneelInCheckpoint = false;
            Time.timeScale = 1;
            UIManager.Instance.mapMenu.SetActive(false);
            UIManager.Instance.menuActivated = false;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
        else if (context.started && UIManager.Instance.menuActivated && UIManager.Instance.optionsMenu.activeSelf)
        {
            Time.timeScale = 0;
            UIManager.Instance.optionsMenu.SetActive(false);
            UIManager.Instance.mapMenu.SetActive(true);
            UIManager.Instance.menuActivated = true;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
        else if (context.started && UIManager.Instance.menuActivated && UIManager.Instance.settingsMenu.activeSelf)
        {
            Time.timeScale = 0;
            UIManager.Instance.settingsMenu.SetActive(false);
            UIManager.Instance.optionsMenu.SetActive(true);
            UIManager.Instance.menuActivated = true;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
        else if (context.started && UIManager.Instance.menuActivated && UIManager.Instance.tipsMenu.activeSelf)
        {
            Time.timeScale = 0;
            UIManager.Instance.tipsMenu.SetActive(false);
            UIManager.Instance.optionsMenu.SetActive(true);
            UIManager.Instance.menuActivated = true;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
        else if (context.started && UIManager.Instance.menuActivated && UIManager.Instance.checkpointMenu.activeSelf)
        {
            Time.timeScale = 1;
            UIManager.Instance.checkpointMenu.SetActive(false);
            UIManager.Instance.menuActivated = false;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
        else if (context.started && UIManager.Instance.menuActivated && UIManager.Instance.attackTutorHUD.activeSelf)
        {
            Time.timeScale = 1;
            UIManager.Instance.attackTutorHUD.SetActive(false);
            UIManager.Instance.menuActivated = false;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
        else if (context.started && UIManager.Instance.menuActivated && UIManager.Instance.dashTutorHUD.activeSelf)
        {
            Time.timeScale = 1;
            UIManager.Instance.dashTutorHUD.SetActive(false);
            UIManager.Instance.menuActivated = false;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
        else if (context.started && UIManager.Instance.menuActivated && UIManager.Instance.wallClimbTutorHUD.activeSelf)
        {
            Time.timeScale = 1;
            UIManager.Instance.wallClimbTutorHUD.SetActive(false);
            UIManager.Instance.menuActivated = false;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
        else if (context.started && UIManager.Instance.menuActivated && UIManager.Instance.checkpointTutorHUD.activeSelf)
        {
            Time.timeScale = 1;
            UIManager.Instance.checkpointTutorHUD.SetActive(false);
            UIManager.Instance.menuActivated = false;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
        else if (context.started && UIManager.Instance.menuActivated && UIManager.Instance.healthPotionsTutorHUD.activeSelf)
        {
            Time.timeScale = 1;
            UIManager.Instance.healthPotionsTutorHUD.SetActive(false);
            UIManager.Instance.menuActivated = false;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
        else if (context.started && UIManager.Instance.menuActivated && UIManager.Instance.prayerTutorHUD.activeSelf)
        {
            Time.timeScale = 1;
            UIManager.Instance.prayerTutorHUD.SetActive(false);
            UIManager.Instance.menuActivated = false;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
        else if (context.started && UIManager.Instance.menuActivated && UIManager.Instance.heartTutorHUD.activeSelf)
        {
            Time.timeScale = 1;
            UIManager.Instance.heartTutorHUD.SetActive(false);
            UIManager.Instance.menuActivated = false;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
        else if (context.started && UIManager.Instance.menuActivated && UIManager.Instance.inventoryTutorHUD.activeSelf)
        {
            Time.timeScale = 1;
            UIManager.Instance.inventoryTutorHUD.SetActive(false);
            UIManager.Instance.menuActivated = false;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
        else if (context.started && UIManager.Instance.menuActivated && UIManager.Instance.mapTutorHUD.activeSelf)
        {
            Time.timeScale = 1;
            UIManager.Instance.mapTutorHUD.SetActive(false);
            UIManager.Instance.menuActivated = false;
            SoundFXManager.Instance.PlayChangeTabSound();
        }
    }

    public void OnPerformPrayer(InputAction.CallbackContext context)
    {
        if (!SceneLoadManager.Instance.IsFadingIn && !SceneLoadManager.Instance.IsFadingOut && !SceneLoadManager.Instance.IsLoading)
        {
            if (!UIManager.Instance.menuActivated)
            {
                if (context.started && IsAlive && CanMove && TouchingDirections.IsGrounded && prayerManaCost > 0f && CurrentMana >= prayerManaCost)
                {
                    if (prayerCooldown <= 0)
                    {
                        Animator.SetTrigger(AnimationString.prayerTrigger);
                    }
                }
            }
        }
    }

    #endregion

    #region Player Coroutines

    private IEnumerator HitFlash()
    {
        SR.material = hitFlashMaterial;

        yield return new WaitForSeconds(0.03f);

        SR.material = originalMaterial;

        hitFlashCoroutine = null;
    }

    private IEnumerator DisableLadderPlatformCollision()
    {
        Collider2D PlatformCollider = LadderPlatformCollider.GetComponent<Collider2D>();

        if (PlatformCollider != null)
        {
            Physics2D.IgnoreCollision(PlayerCollider, PlatformCollider, true);
        }
        else
        {
            yield break;
        }

        while (IsClimbing && isOnLadder)
        {
            yield return null;
        }

        if (PlatformCollider != null)
        {
            Physics2D.IgnoreCollision(PlayerCollider, PlatformCollider, false);
        }
        else
        {
            yield break;
        }
    }

    private IEnumerator DisableWallHang()
    {
        canWallHang = false;
        yield return new WaitForSeconds(0.1f);
        canWallHang = true;
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
        IsInvincible = true;
        IsDashing = true;
        canDash = false;
        canJump = false;
        float originalGravity = RB.gravityScale;
        RB.gravityScale = 0;
        GhostTrail.StartGhostTrail();

        float dashDirection = IsFacingRight ? 1f : -1f;
        float dashEndTime = Time.time + dashTime;
        float dashDelay = 0.15f;
        float afterDashTime = 0.5f;

        yield return new WaitForSeconds(dashDelay);
        RB.velocity = new Vector2(dashDirection * (dashPower + dashPowerBuff), 0f);
        canJump = true;

        // Check for ground while dashing
        while (Time.time < dashEndTime)
        {
            if (!TouchingDirections.IsGrounded)
            {
                GhostTrail.StopGhostTrail();
                IsDashing = false;
                IsInvincible = false;
                RB.gravityScale = originalGravity;
                IsDashed = true;

                yield return new WaitForSeconds(afterDashTime);
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

        if (wallJumpSpeedBoostCoroutine != null)
        {
            StopCoroutine(wallJumpSpeedBoostCoroutine);
        }
        wallJumpSpeedBoostCoroutine = StartCoroutine(WallJumpSpeedBoost());

        yield return new WaitForSeconds(wallJumpingDuration);
        IsWallJumping = false;
    }

    private IEnumerator WallHanging()
    {
        IsWallHanging = true;
        RB.gravityScale = 0;
        Vector2 directionToWall = IsFacingRight ? Vector2.right : Vector2.left;

        float moveToWallSpeed = 100f;
        while (!TouchingDirections.IsOnWall && IsWallHanging)
        {
            if (TouchingDirections.IsGrabWallDetected && !IsWallJumping)
            {
                RB.velocity = new Vector2(directionToWall.x * moveToWallSpeed, 0);
            }
            yield return null;
        }

        float startTime = Time.time;
        while (Time.time < startTime + wallSlideDuration && IsWallHanging)
        {
            float t = (Time.time - startTime) / wallSlideDuration;
            float slideSpeed = Mathf.Lerp(wallSlideSpeed, 0, t);
            RB.velocity = new Vector2(0, -slideSpeed);
            yield return null;
        }

        while (IsWallHanging)
        {
            RB.velocity = Vector2.zero;
            yield return null;
        }
    }

    private IEnumerator WallJumpSpeedBoost()
    {
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

        yield return new WaitForSeconds(wallJumpSpeedBoostDuration);

        float elapsedTime = 0f;
        float decreaseDuration = 1f;
        float initialBoost = wallJumpSpeedBoost;

        while (elapsedTime < decreaseDuration)
        {
            elapsedTime += Time.deltaTime;
            moveSpeed -= initialBoost * (Time.deltaTime / decreaseDuration);
            yield return null;
        }

        moveSpeed = originalMoveSpeed;
        isWallJumpSpeedBoostActive = false;
    }

    #endregion

    #region Player Collision check

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = true;
            LadderCollider = collision;
        }

        if (collision.CompareTag("SavePoint"))
        {
            isInCheckpoint = true;
            Checkpoint = collision.GetComponent<CheckpointManager>();
        }

        if (collision.CompareTag("Item"))
        {
            hasItemInRange = true;
            Item = collision.GetComponent<ItemCollectable>();
        }

        if (collision.CompareTag("NPC"))
        {
            isAtNPC = true;
            NPC = collision.GetComponentInParent<NPC>();
        }

        if (collision.CompareTag("Shop"))
        {
            isAtShop = true;
            ShopDoorSceneChanger = collision.GetComponent<ShopDoorSceneChanger>();
        }

        if (collision.CompareTag("ItemBuyable"))
        {
            canBuyItem = true;
            ItemBuyable = collision.GetComponentInParent<ItemBuyable>();
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = false;
            IsClimbing = false;
            RB.gravityScale = originalGravityScale;
            LadderCollider = null;
        }

        if (collision.CompareTag("SavePoint"))
        {
            isInCheckpoint = false;
            Checkpoint = null;
        }

        if (collision.CompareTag("Item"))
        {
            hasItemInRange = false;
            Item = null;
        }

        if (collision.CompareTag("NPC"))
        {
            isAtNPC = false;
            NPC = null;
        }

        if (collision.CompareTag("Shop"))
        {
            isAtShop = false;
            ShopDoorSceneChanger = null;
        }

        if (collision.CompareTag("ItemBuyable"))
        {
            canBuyItem = false;
            ItemBuyable = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LadderPlatform"))
        {
            LadderPlatformCollider = collision.collider;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LadderPlatform"))
        {
            LadderPlatformCollider = null;
        }
    }

    #endregion

    #region Respawn Player functions

    public void RespawnPlayer()
    {
        ResetPlayerAnimation();
        IsAlive = true;
        CurrentHealth = MaxHealth;
        CurrentMana = CurrentMana;
        CurrentHealthPotion = MaxHealthPotion;
        HorizontalInput = Vector2.zero;
        Animator.SetTrigger(AnimationString.spawnTrigger);
    }

    #endregion

    #region Reset player animation

    public void ResetPlayerAnimation()
    {
        HorizontalInput = Vector2.zero;
        VerticalInput = Vector2.zero;
        IsMoving = false;
        IsJumping = false;
        IsDashing = false;
        IsDashed = false;
        IsWallHanging = false;
        IsWallJumping = false;
        IsClimbing = false;
        IsKilledBySpikes = false;
        CanMove = false;
    }

    #endregion
}
