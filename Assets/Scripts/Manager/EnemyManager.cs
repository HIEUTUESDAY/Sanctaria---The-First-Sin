using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();

    void Awake()
    {
        InitializeEnemies();
    }

    // Initialize all enemies
    private void InitializeEnemies()
    {
        // Find all enemies in the game and add them to the list
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in allEnemies)
        {
            enemies.Add(enemy);
        }
    }

    // Respawn all enemies
    public void RespawnAllEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            // Reset the enemy's health and other attributes if needed
            Enemy dieEnemy = enemy.GetComponent<Enemy>();
            if(dieEnemy != null && !dieEnemy.IsAlive)
            {
                dieEnemy.RespawnSetup();
            }
        }
    }
}
