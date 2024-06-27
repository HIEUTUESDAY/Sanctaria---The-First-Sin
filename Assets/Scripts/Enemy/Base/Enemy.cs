using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class Enemy : MonoBehaviour, IEnemyDamageable, IEnemyMoveable
{
    #region Enemy variables

    public TouchingDirections TouchingDirections { get; set; }
    public Animator Animator { get; set; }
    public HitSplashEvent HitSplashEvent { get; set; }
    public CinemachineImpulseSource ImpulseSource { get; set; }
    [SerializeField] private float slowMotionDuration = 0.5f;
    [SerializeField] private float slowMotionFactor = 0.2f;

    #endregion

    #region IEnemyDamageable variables

    [field: SerializeField] public UnityEvent<float, Vector2> DamageableHit { get; set; }
    [field: SerializeField] public UnityEvent DamageableDie { get; set; }
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;

    [field: SerializeField]
    public float _currentHealth; 
    public float CurrentHealth 
    { 
        get => _currentHealth; 
        set 
        {
            _currentHealth = value;
            if (_currentHealth <= 0)
            {
                IsAlive = false;
            }
        } 
    }

    [SerializeField]
    public bool _isAlive = true;
    public bool IsAlive
    {
        get => _isAlive;
        set
        {
            _isAlive = value;
            Animator.SetBool(AnimationString.isAlive, value);

            if (value == false)
            {
                DamageableDie.Invoke();
            }
        }
    }

    #endregion

    #region IEnemyMoveable

    public Rigidbody2D RB { get; set; }
    [field: SerializeField] public bool IsFacingRight { get; set; } = true;

    #endregion

    #region State Machine variables

    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }

    #endregion

    #region ScriptableObject variables

    [SerializeField] private EnemyIdleSOBase EnemyIdleBase;
    [SerializeField] private EnemyChaseSOBase EnemyChaseBase;
    [SerializeField] private EnemyAttackSOBase EnemyAttackBase;

    public EnemyIdleSOBase EnemyIdleBaseInstance { get; set; }
    public EnemyChaseSOBase EnemyChaseBaseInstance { get; set; }
    public EnemyAttackSOBase EnemyAttackBaseInstance { get; set; }

    #endregion

    public void Awake()
    {
        EnemyIdleBaseInstance = Instantiate(EnemyIdleBase);
        EnemyChaseBaseInstance = Instantiate(EnemyChaseBase);
        EnemyAttackBaseInstance = Instantiate(EnemyAttackBase);

        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
    }

    public void Start()
    {
        CurrentHealth = MaxHealth;

        RB = GetComponent<Rigidbody2D>();
        TouchingDirections = GetComponent<TouchingDirections>();
        Animator = GetComponent<Animator>();
        HitSplashEvent = GetComponent<HitSplashEvent>();
        ImpulseSource = GetComponent<CinemachineImpulseSource>();

        EnemyIdleBaseInstance.Initialize(gameObject, this);
        EnemyChaseBaseInstance.Initialize(gameObject, this);
        EnemyAttackBaseInstance.Initialize(gameObject, this);

        StateMachine.Initialize(IdleState);
    }

    public void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    public void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    #region Hit function & coroutines

    public void TakeDamage(float damage, Vector2 knockback, Vector2 hitDirection, int attackType)
    {
        if (IsAlive)
        {
            // Beable to hit
            CurrentHealth -= damage;

            // Spawn Damage Particle with direction
            HitSplashEvent.ShowHitSplash(transform.position, hitDirection, attackType);
            StartCoroutine(ApplySlowMotion());
            CameraShakeManager.Instance.CameraShake(ImpulseSource);
            Animator.SetTrigger(AnimationString.hitTrigger);

            // Notify other subcribed components that damageable was hit to handle the knockback
            DamageableHit?.Invoke(damage, knockback);
            CharacterEvent.characterDamaged.Invoke(gameObject, damage);
        }
        else
        {
            return;
        }
    }

    private IEnumerator ApplySlowMotion()
    {
        Time.timeScale = slowMotionFactor;
        yield return new WaitForSecondsRealtime(slowMotionDuration);
        Time.timeScale = 1f;
    }

    #endregion

    #region Move & Facing check functions

    public void MoveEnemy(Vector2 velocity)
    {
        RB.velocity = velocity;
    }

    public void FlipEnemy()
    {
        if (IsFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
        else if (!IsFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
    }

    #endregion

    #region Animation Triggers

    private void AnimationTriggerEnvent(AniamtionTriggerType triggerType)
    {
        // TODO: fill in one StateManchine is created
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }

    public enum AniamtionTriggerType
    {
        EnemyIdle,
        EnemyChase,
        EnemyAttack,
        EnemyHit
    }

    #endregion
}
