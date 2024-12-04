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

        MusicManager.Instance.StopMusic();
        MusicManager.Instance.musicAudioSource.clip = MusicManager.Instance.tenPiedadBrethingClip;
        MusicManager.Instance.PlayMusic();
    }

    private void SetBossMaxHealthBar()
    {
        BossHealthBarManager.SetMaxHealth(MaxHealth);
    }

    private void SetBossCurrentHealthBar()
    {
        BossHealthBarManager.SetHealth(CurrentHealth);
    }

    private void ShakeCamera()
    {
        CameraShakeManager.Instance.CameraShake(ImpulseSource);
    }

    public void PlayWalkSound()
    {
        if (Player.Instance.IsAlive)
        {
            SoundFXManager.Instance.Play2DRandomSoundFXClip(SoundFXManager.Instance.TPWalkSound, transform, 5f);
            SoundFXManager.Instance.Play3DRandomSoundFXClip(SoundFXManager.Instance.TPWalkVoiceSound, transform, 0.2f);
        }
    }

    public void PlayStormSound()
    {
        if (Player.Instance.IsAlive)
        {
            SoundFXManager.Instance.Play2DSoundFXClip(SoundFXManager.Instance.TPStormSound, transform, 1f);
        }
    }

    public void PlayMovingSound()
    {
        if (Player.Instance.IsAlive)
        {
            SoundFXManager.Instance.Play3DSoundFXClip(SoundFXManager.Instance.TPMovingSound, transform, 0.2f);
        }
    }

    public void PlaySceamSound()
    {
        if (Player.Instance.IsAlive)
        {
            SoundFXManager.Instance.Play3DSoundFXClip(SoundFXManager.Instance.TPAttackVoiceSound, transform, 0.1f);
        }
    }

    public void PlayHandAttackSound()
    {
        if (Player.Instance.IsAlive)
        {
            SoundFXManager.Instance.Play2DSoundFXClip(SoundFXManager.Instance.TPStormVoiceSound, transform, 1f);
        }
    }

    public void PlayDeathSound()
    {
        if (Player.Instance.IsAlive)
        {
            SoundFXManager.Instance.Play2DSoundFXClip(SoundFXManager.Instance.TPDeathSound, transform, 1f);
        }
    }
}
