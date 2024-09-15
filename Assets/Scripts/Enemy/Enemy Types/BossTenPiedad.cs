using Cinemachine;
using System.Collections;
using UnityEngine;

public class BossTenPiedad : Enemy
{
    [SerializeField] private Collider2D bodyHitCollider;

    private BossHealthBarManager BossHealthBarManager;

    protected override void AwakeSetup()
    {
        base.AwakeSetup();

        EnemyIdleBaseInstance = Instantiate(EnemyIdleBase);
        EnemyChaseBaseInstance = Instantiate(EnemyChaseBase);
        EnemyAttackBaseInstance = Instantiate(EnemyAttackBase);

        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
    }

    protected override void StartSetup()
    {
        base.StartSetup();

        CurrentHealth = MaxHealth;

        RB = GetComponent<Rigidbody2D>();
        TouchingDirections = GetComponent<TouchingDirections>();
        Animator = GetComponent<Animator>();
        ImpulseSource = GetComponent<CinemachineImpulseSource>();
        SR = RB.GetComponent<SpriteRenderer>();
        BossHealthBarManager = GetComponent<BossHealthBarManager>();

        EnemyIdleBaseInstance.Initialize(gameObject, this);
        EnemyChaseBaseInstance.Initialize(gameObject, this);
        EnemyAttackBaseInstance.Initialize(gameObject, this);

        StateMachine.Initialize(IdleState);

        SetBossMaxHealthBar();
    }

    protected override void UpdateSetup()
    {
        base.UpdateSetup();

        StateMachine.CurrentEnemyState.FrameUpdate();

        SetBossCurrentHealthBar();
    }

    protected override void FixedUpdateSetup()
    {
        base.FixedUpdateSetup();

        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    public override void OnDead()
    {
        base.OnDead();

        bodyHitCollider.enabled = false;

        Player.Instance.PlayerInput.enabled = false;
    }

    private void SetBossMaxHealthBar()
    {
        BossHealthBarManager.SetMaxHealth(MaxHealth);
    }

    private void SetBossCurrentHealthBar()
    {
        BossHealthBarManager.SetHealth(CurrentHealth);
    }
}
