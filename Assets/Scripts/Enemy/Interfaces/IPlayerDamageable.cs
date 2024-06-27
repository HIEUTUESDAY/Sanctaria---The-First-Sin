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
    bool IsAlive { get; set; }
}
