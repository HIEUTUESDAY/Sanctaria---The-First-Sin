using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damned : Enemy
{
    [SerializeField] private Collider2D bodyHitCollider;
    [SerializeField] private GameObject rockProjectilePrefab;
    [SerializeField] private Transform spawnPosition;

    protected override void AwakeSetup()
    {
        base.AwakeSetup();

        EnemyIdleBaseInstance = Instantiate(EnemyIdleBase);
        EnemyAttackBaseInstance = Instantiate(EnemyAttackBase);

        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
    }

    protected override void StartSetup()
    {
        base.StartSetup();

        CurrentHealth = MaxHealth;

        RB = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        ImpulseSource = GetComponent<CinemachineImpulseSource>();
        SR = RB.GetComponent<SpriteRenderer>();

        EnemyIdleBaseInstance.Initialize(gameObject, this);
        EnemyAttackBaseInstance.Initialize(gameObject, this);

        StateMachine.Initialize(IdleState);
    }

    protected override void UpdateSetup()
    {
        base.UpdateSetup();

        StateMachine.CurrentEnemyState.FrameUpdate();
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
    }

    public override void RespawnSetup()
    {
        base.RespawnSetup();

        gameObject.SetActive(true);
        IsAlive = true;
        CurrentHealth = MaxHealth;
        bodyHitCollider.enabled = true;
        Color color = SR.color;
        color.a = 1f;
        SR.color = color;
    }

    public void PlayAttackSound()
    {
        
    }

    public void PlayDeathSound()
    {
        
    }

    public void SpawnRockProjectile()
    {
        Instantiate(rockProjectilePrefab, spawnPosition.position, spawnPosition.rotation);
    }
}
