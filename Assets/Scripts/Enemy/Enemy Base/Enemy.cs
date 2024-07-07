using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class Enemy : MonoBehaviour, IEnemyDamageable, IEnemyMoveable
{
    #region Enemy variables

    public SpriteRenderer SR { get; set; }
    public TouchingDirections TouchingDirections { get; set; }
    public Animator Animator { get; set; }
    public HitSplashEvent HitSplashEvent { get; set; }
    public CinemachineImpulseSource ImpulseSource { get; set; }
    [SerializeField] private float slowMotionDuration = 0.5f;
    [SerializeField] private float slowMotionFactor = 0.2f;

    #endregion

    #region IEnemyDamageable variables

    [field: SerializeField] public UnityEvent<float, Vector2> DamageableHit { get; set; }
    [field: SerializeField] public UnityEvent DamageableDead { get; set; }
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
                DamageableDead.Invoke();
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

    [field: SerializeField] public EnemyIdleSOBase EnemyIdleBase { get; set; }
    [field: SerializeField] public EnemyChaseSOBase EnemyChaseBase { get; set; }
    [field: SerializeField] public EnemyAttackSOBase EnemyAttackBase { get; set; }

    public EnemyIdleSOBase EnemyIdleBaseInstance { get; set; }
    public EnemyChaseSOBase EnemyChaseBaseInstance { get; set; }
    public EnemyAttackSOBase EnemyAttackBaseInstance { get; set; }

    #endregion

    public void Awake()
    {
        AwakeSetup();
    }

    protected virtual void AwakeSetup()
    {
        
    }

    public void Start()
    {
        StartSetup();
    }

    protected virtual void StartSetup()
    {
       
    }

    public void Update()
    {
        UpdateSetup();
    }

    protected virtual void UpdateSetup()
    {

    }

    public void FixedUpdate()
    {
        FixedUpdateSetup();
    }

    protected virtual void FixedUpdateSetup()
    {

    }

    #region Take damage function & coroutines

    public void TakeDamage(float damage, Vector2 knockback, Vector2 hitDirection, int attackType)
    {
        if (IsAlive)
        {
            // Beable to hit
            CurrentHealth -= damage;

            // Spawn Damage Particle with direction
            HitSplashEvent.ShowHitSplash(transform.position, hitDirection, attackType);
            CoroutineManager.Instance.StartCoroutineManager(ApplySlowMotion());
            CameraShakeManager.Instance.CameraShake(ImpulseSource);
            Animator.SetTrigger(AnimationString.hitTrigger);

            // Notify other subcribed components that damageable was hit to handle the knockback
            DamageableHit?.Invoke(damage, knockback);
            CharacterEvent.characterDamaged.Invoke(gameObject, damage);
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

    #region Respanwn functions

    public virtual void RespawnSetup()
    {

    }

    #endregion

    #region Animation Triggers

    private void AnimationTriggerEnvent(AniamtionTriggerType triggerType)
    {
        // TODO: fill in one StateManchine is created
        /*StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);*/
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