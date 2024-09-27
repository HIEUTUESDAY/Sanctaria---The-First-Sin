using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamageableBase
{
    UnityEvent<float, Vector2> DamageableHit { get; set; }
    UnityEvent DamageableDead { get; set; }
    bool IsAlive { get; set; }
    void TakeDamage(float damage, Vector2 knockback, Vector2 hitDirection, int attackType);
}
