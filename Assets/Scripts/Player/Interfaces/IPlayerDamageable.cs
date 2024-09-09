using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerDamageable : IDamageableBase
{
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
    float MaxMana { get; set; }
    float CurrentMana { get; set; }
    int MaxHealthPotion { get; set; }
    int CurrentHealthPotion { get; set; }
    float HealthRestore {  get; set; }
    bool IsInvincible { get; set; }
    bool WasHit { get; set; }
    bool LockVelocity { get; set; }
    void BeInvisible();
}
