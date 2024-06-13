using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private UnityEvent<int, Vector2> damageableHit;
    [SerializeField] private UnityEvent damageableDeath;
    [SerializeField] private float slowMotionDuration = 0.5f;
    [SerializeField] private float slowMotionFactor = 0.2f;
    [SerializeField] private HealthBar healthBar;

    private CinemachineImpulseSource impulseSource;
    private HitSplashManager hitSplashManager;
    private Animator animator;

    [SerializeField]
    private int _maxHealth = 100;

    public int MaxHealth
    { 
        get 
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
            
    }

    [SerializeField]
    private int _health = 100;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;

            // Character die when health drop below 0
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private int _maxStamina = 100;

    public int MaxStamina
    {
        get
        {
            return _maxStamina;
        }
        set
        {
            _maxStamina = value;
        }

    }

    [SerializeField]
    private int _stamina = 100;

    public int Stamina
    {
        get
        {
            return _stamina;
        }
        set
        {
            _stamina = value;
        }
    }

    [SerializeField]
    private bool _isAlive = true;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationString.isAlive, value);

            if(value == false)
            {
                damageableDeath.Invoke();
            }
        }
    }

    public bool OnAttack
    {
        get 
        { 
            return animator.GetBool(AnimationString.onAttack); 
        }
        set
        {
            animator.GetBool(AnimationString.onAttack);
        }
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationString.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationString.lockVelocity, value);
        }
    }

    [SerializeField]
    private bool _isInvincible = false;

    public bool IsInvincible
    {
        get
        {
            return _isInvincible;
        }
        set
        {
            _isInvincible = value;
            animator.SetBool(AnimationString.isInvincible, value);
        }

    }

    [SerializeField]
    private float timeSinceHit  = 0;
    public float invincibleTime = 2f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        hitSplashManager = GetComponent<HitSplashManager>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        this.SetHealthBar();
    }

    private void Update()
    {
        this.OnBecameInvisible();
        this.SetHealth();
    }

    public bool Hit(int damage, Vector2 knockback, Vector2 hitDirection, int attackType)
    {
        if (IsAlive && !IsInvincible)
        {
            // Beable to hit
            Health -= damage;
            IsInvincible = true;

            // Spawn Damage Particle with direction
            hitSplashManager.ShowHitSplash(transform.position, hitDirection, attackType);

            // Camera shake and slow motion when deal damage
            if (gameObject.CompareTag("Enemy"))
            {
                StartCoroutine(ApplySlowMotion());
            }
            CameraShakeManager.instance.CameraShake(impulseSource);

            // Notify other subcribed components that damageable was hit to handle the knockback
            if (!OnAttack) 
            { 
                animator.SetTrigger(AnimationString.hitTrigger);
            }

            damageableHit?.Invoke(damage, knockback);
            CharacterEvent.characterDamaged.Invoke(gameObject, damage);

            return true;
        }
        // Unable to hit
        return false;
    }


    private void OnBecameInvisible()
    {
        if (IsInvincible)
        {
            if (timeSinceHit > invincibleTime)
            {
                // Remove invincible after a short time
                IsInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }

    // Return whether the player was healed or not
    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            CharacterEvent.characterHealed(gameObject, actualHeal);
            return true;
        }
        else
            return false;
    }

    private void SetHealthBar()
    {
        if(gameObject.tag == "Player")
        {
            healthBar.SetMaxHealth(MaxHealth);
            healthBar.SetMaxStamina(MaxStamina);
        }
    }

    private void SetHealth()
    {
        if (gameObject.tag == "Player")
        {
            healthBar.SetHealth(Health);
        }
    }

    private IEnumerator ApplySlowMotion()
    {
        Time.timeScale = slowMotionFactor;
        yield return new WaitForSecondsRealtime(slowMotionDuration);
        Time.timeScale = 1f;
    }
}
