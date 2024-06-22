using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public PlayerData playerData;
    public string currentScene;
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

    /*    public Dictionary<string, SceneData> scenesData; // Add this field

        public GameData()
        {
            scenesData = new Dictionary<string, SceneData>();
        }*/

/*[System.Serializable]
public class SceneData
{
    public List<EnemyData> enemies;
    public List<ObjectData> objects; // Add any other data you want to save about the scene

    public SceneData()
    {
        enemies = new List<EnemyData>();
        objects = new List<ObjectData>();
    }
}

[System.Serializable]
public class EnemyData
{
    public string enemyId; // Unique ID for the enemy
    public float[] position;
    public float health;
    public bool isAlive;

    public EnemyData(Enemy enemy)
    {
        enemyId = enemy.id; // Assuming each enemy has a unique ID
        position = new float[3];
        position[0] = enemy.transform.position.x;
        position[1] = enemy.transform.position.y;
        position[2] = enemy.transform.position.z;
        health = enemy.CurrentHealth;
        isAlive = enemy.isAlive;
    }
}

[System.Serializable]
public class ObjectData
{
    public string objectId; // Unique ID for the object
    public float[] position;
    public bool isActive;

    public ObjectData(GameObject obj)
    {
        objectId = obj.name; // Assuming each object has a unique name
        position = new float[3];
        position[0] = obj.transform.position.x;
        position[1] = obj.transform.position.y;
        position[2] = obj.transform.position.z;
        isActive = obj.activeSelf;
    }
}*/