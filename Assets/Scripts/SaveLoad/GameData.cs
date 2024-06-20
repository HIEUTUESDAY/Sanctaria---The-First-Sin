using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public PlayerData playerData;
    public string currentScene; // Add this field
}

[System.Serializable]
public class PlayerData
{
    public float[] position;
    public float health;
    public float stamina;
    public int healthPostions;
    public string currentArea;

    public PlayerData(PlayerController player)
    {
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        Damageable damageable = player.GetComponent<Damageable>();
        if (damageable != null)
        {
            health = damageable.CurrentHealth;
            stamina = damageable.CurrentStamina;
            healthPostions = damageable.CurrentHealthPotion;
        }

        currentArea = player.currentArea;
    }
}
