using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float health;
    public float stamina;
    public int healthPotions;

    public PlayerData(Player player)
    {
        this.health = player.CurrentHealth;
        this.stamina = player.CurrentStamina;
        this.healthPotions = player.CurrentHealthPotion;
    }
}
