using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private UnityEvent<float, Vector2> damageableHit;
    [SerializeField] private UnityEvent damageableDeath;
    [SerializeField] private float slowMotionDuration = 0.5f;
    [SerializeField] private float slowMotionFactor = 0.2f;

    private CinemachineImpulseSource impulseSource;
    private HitSplashManager hitSplashManager;
    private Animator animator;

    [SerializeField]
    private float _maxHealth = 100;

    public float MaxHealth
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
    private float _currentHealth = 100;

    public float CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;

            // Character die when health drop below 0
            if (_currentHealth <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private float _maxStamina = 100;

    public float MaxStamina
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
    private float _currentStamina = 100;

    public float CurrentStamina
    {
        get
        {
            return _currentStamina;
        }
        set
        {
            _currentStamina = value;
        }
    }

    [SerializeField]
    private int _maxHealthPotion = 2;
    public float healthRestore = 50f;

    public int MaxHealthPotion
    {
        get
        {
            return _maxHealthPotion;
        }
        set
        {
            _maxHealthPotion = value;
        }
    }

    [SerializeField] 
    private int _currentHealthPotion;

    public int CurrentHealthPotion
    {
        get
        {
            return _currentHealthPotion;
        }
        set
        {
            _currentHealthPotion = value;
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
    private bool _wasHit = false;

    public bool WasHit
    {
        get
        {
            return _wasHit;
        }
        private set
        {
            _wasHit = value;
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
    }

    private void Update()
    {
        this.OnBecameInvisible();
    }

    public bool Hit(float damage, Vector2 knockback, Vector2 hitDirection, int attackType)
    {
        if (IsAlive && !IsInvincible)
        {
            // Beable to hit
            CurrentHealth -= damage;
            WasHit = true;

            // Spawn Damage Particle with direction
            hitSplashManager.ShowHitSplash(transform.position, hitDirection, attackType);

            StartCoroutine(ApplySlowMotion());

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

    // Return whether the player was healed or not
    public bool UseHealthPotion()
    {
        if (IsAlive && !LockVelocity)
        {
            float maxHeal = Mathf.Max(MaxHealth - CurrentHealth, 0);
            float actualHeal = Mathf.Min(maxHeal, healthRestore);
            CurrentHealth += actualHeal;
            CurrentStamina = MaxStamina;
            CurrentHealthPotion -= 1;
            CharacterEvent.characterHealed(gameObject, actualHeal);
            return true;
        }
        else
            return false;
    }

    private IEnumerator ApplySlowMotion()
    {
        Time.timeScale = slowMotionFactor;
        yield return new WaitForSecondsRealtime(slowMotionDuration);
        Time.timeScale = 1f;
    }
}
