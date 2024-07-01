using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerDamageable : IDamageableBase
{
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
    float MaxStamina { get; set; }
    float CurrentStamina { get; set; }
    int MaxHealthPotion { get; set; }
    int CurrentHealthPotion { get; set; }
    float HealthRestore {  get; set; }
    bool IsAlive { get; set; }
    bool IsInvincible { get; set; }
    bool WasHit { get; set; }
    bool LockVelocity { get; set; }
    void BeInvisible();
}
