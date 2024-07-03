using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WingedFace : Enemy
{ 
    [SerializeField] public Collider2D deathCollider;
    private SpriteRenderer spriteRenderer;

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
        spriteRenderer = RB.GetComponent<SpriteRenderer>();

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

    public void OnDeath()
    {
        // Fall down when is dead
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
        RB.gravityScale = 0f;
        deathCollider.enabled = false;
        Color color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;
    }
}
