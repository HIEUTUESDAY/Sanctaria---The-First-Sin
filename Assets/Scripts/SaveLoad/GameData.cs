using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public PlayerData playerData;
    public Checkpoint checkpoint;
}

[System.Serializable]
public class PlayerData
{
    public float health;
    public float stamina;
    public int healthPotions;

    public PlayerData(Player player, Checkpoint checkpoint)
    {
        health = player.CurrentHealth;
        stamina = player.CurrentStamina;
        healthPotions = player.CurrentHealthPotion;
    }
}

[System.Serializable]
public class Checkpoint
{
    public string sceneName;
    public float[] position;

    public Checkpoint(string sceneName, Vector3 position)
    {
        this.sceneName = sceneName;
        this.position = new float[3];
        this.position[0] = position.x;
        this.position[1] = position.y;
        this.position[2] = position.z;
    }
}
