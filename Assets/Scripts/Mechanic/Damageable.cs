using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;

    Animator animator;

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
        private set
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
        private set
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
    public float invincibleTime = 0.25f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        this.OnBecameInvisible();
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !IsInvincible && OnAttack == true)
        {
            // Beable to hit
            Health -= damage;
            IsInvincible = true;

            // Deal damage but no hit animation while attacking
            damageableHit?.Invoke(damage, knockback);
            CharacterEvent.characterDamaged.Invoke(gameObject, damage);

            return true;
        }
        else if (IsAlive && !IsInvincible && OnAttack == false)
        {
            // Beable to hit
            Health -= damage;
            IsInvincible = true;

            // Notify other subcribed components that damageable was hit to handle the knockback
            animator.SetTrigger(AnimationString.hitTrigger);
            LockVelocity = true;
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
}
