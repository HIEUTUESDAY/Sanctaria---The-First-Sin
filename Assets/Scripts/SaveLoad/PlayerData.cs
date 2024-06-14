using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] position;
    public int health;
    public int stamina;
    public int healthPostions;

    public PlayerData(PlayerController player)
    {
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        health = player.GetComponent<Damageable>().Health;
        stamina = player.GetComponent<Damageable>().Stamina;
        healthPostions = player.GetComponent<HealthPotion>().CurrentHealthPotions;
    }
}
