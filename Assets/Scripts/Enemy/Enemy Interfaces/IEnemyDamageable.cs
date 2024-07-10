using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEnemyDamageable : IDamageableBase
{
    //TODO: add any functions when needed
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
}
