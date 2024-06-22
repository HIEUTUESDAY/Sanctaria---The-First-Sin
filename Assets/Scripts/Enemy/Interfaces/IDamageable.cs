using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamageable
{
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
    bool IsAlive { get; set; }
    bool IsInvincible { get; set; }

    UnityEvent<float, Vector2> DamageableHit { get; set; }

    UnityEvent DamageableDeath { get; set; }

    bool Hit(float damage, Vector2 knockback, Vector2 hitDirection, int attackType);

    void Die();

}
