using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingedFace : Enemy
{
    [SerializeField] private Collider2D bodyHitCollider;
    [SerializeField] private Collider2D deathCollider;

    protected override void AwakeSetup()
    {
        base.AwakeSetup();

        EnemyIdleBaseInstance = Instantiate(EnemyIdleBase);

        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);

    }

    protected override void StartSetup()
    {
        base.StartSetup();

        CurrentHealth = MaxHealth;

        RB = GetComponent<Rigidbody2D>();
        TouchingDirections = GetComponent<TouchingDirections>();
        Animator = GetComponent<Animator>();
        HitSplashEvent = GetComponent<HitSplashEvent>();
        ImpulseSource = GetComponent<CinemachineImpulseSource>();
        SR = RB.GetComponent<SpriteRenderer>();

        EnemyIdleBaseInstance.Initialize(gameObject, this);

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

    public void OnDead()
    {
        // Fall down when is dead
        bodyHitCollider.enabled = false;
        RB.gravityScale = 2f;
        RB.velocity = new Vector2(0, RB.velocity.y);
        deathCollider.enabled = true;
    }

    public override void RespawnSetup()
    {
        base.RespawnSetup();

        gameObject.SetActive(true);
        IsAlive = true;
        CurrentHealth = MaxHealth;
        bodyHitCollider.enabled = true;
        RB.gravityScale = 0f;
        deathCollider.enabled = false;
        Color color = SR.color;
        color.a = 1f;
        SR.color = color;
    }
}
