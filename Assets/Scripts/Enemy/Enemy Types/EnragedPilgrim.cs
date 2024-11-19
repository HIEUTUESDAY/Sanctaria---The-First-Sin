using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;

public class EnragedPilgrim : Enemy
{
    [SerializeField] private Collider2D attackCollider;
    [SerializeField] private Collider2D bodyHitCollider;

    protected override void AwakeSetup()
    {
        base.AwakeSetup();

        EnemyIdleBaseInstance = Instantiate(EnemyIdleBase);
        EnemyChaseBaseInstance = Instantiate(EnemyChaseBase);

        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
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

        EnemyIdleBaseInstance.Initialize(gameObject, this);
        EnemyChaseBaseInstance.Initialize(gameObject, this);

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

        attackCollider.enabled = false;
        bodyHitCollider.enabled = false;
    }

    public override void RespawnSetup()
    {
        base.RespawnSetup();

        gameObject.SetActive(true);
        IsAlive = true;
        CurrentHealth = MaxHealth;
        attackCollider.enabled = true;
        bodyHitCollider.enabled = true;
        Color color = SR.color;
        color.a = 1f;
        SR.color = color;
    }

    public void PlayWalkSound()
    {
        SoundFXManager.Instance.Play3DRandomSoundFXClip(SoundFXManager.Instance.EPWalkSound, transform, 1f);
    }

    public void PlayDeathSound()
    {
        SoundFXManager.Instance.Play3DSoundFXClip(SoundFXManager.Instance.EPDeathSound, transform, 1f);
    }
}
