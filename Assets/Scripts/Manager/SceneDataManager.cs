using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDataManager : MonoBehaviour
{
    public static SceneDataManager Instance { get; private set; }
    private EnemyManager enemyManager;
    private ItemManager itemManager;
    private BossManager bossManager;
    private CheckpointManager checkPointManager;
    private NPCManager npcManager;
    public List<SceneData> sceneDataList;
    private List<EnemyData> enemyDataList;
    private List<ItemData> itemDataList;
    private BossData bossData;
    private NPCData npcData;
    private CheckPointData checkPointData;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (!SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            enemyManager = FindObjectOfType<EnemyManager>();
            itemManager = FindObjectOfType<ItemManager>();
            bossManager = FindObjectOfType<BossManager>();
            checkPointManager = FindObjectOfType<CheckpointManager>();
            npcManager = FindObjectOfType<NPCManager>();

            if (enemyManager != null)
            {
                enemyDataList = enemyManager.SaveEnemies();
            }
            else
            {
                enemyDataList = null;
            }

            if (itemManager != null)
            {
                itemDataList = itemManager.SaveItems();
            }
            else
            {
                itemDataList = null;
            }

            if (bossManager != null)
            {
                bossData = bossManager.SaveBoss();
            }
            else
            {
                bossData = null;
            }

            if (checkPointManager != null)
            {
                checkPointData = checkPointManager.SaveCheckpoint();
            }
            else
            {
                checkPointData = null;
            }

            if (npcManager != null)
            {
                npcData = npcManager.SaveNPC();
            }
            else
            {
                npcData = null;
            }

            SaveSceneData(SceneManager.GetActiveScene().name);
        }
    }

    // Method to save the scene data
    public void SaveSceneData(string currentScene)
    {
        if (sceneDataList.Count > 0)
        {
            foreach (SceneData sceneData in sceneDataList)
            {
                if(sceneData.sceneName.Equals(currentScene))
                {
                    sceneData.sceneName = currentScene;
                    sceneData.enemies = enemyDataList;
                    sceneData.items = itemDataList;
                    sceneData.boss = bossData;
                    sceneData.checkPoint = checkPointData;
                    sceneData.npc = npcData;
                    return;
                }
            }

            SceneData newSceneData = new SceneData
            (
                currentScene,
                enemyDataList,
                itemDataList,
                bossData,
                checkPointData,
                npcData
            );

            sceneDataList.Add(newSceneData);
        }
        else
        {
            SceneData firstSceneData = new SceneData
            (
                currentScene,
                enemyDataList,
                itemDataList,
                bossData,
                checkPointData,
                npcData
            );

            sceneDataList.Add(firstSceneData);
        }

        LoadSceneData(currentScene);
    }

    public void LoadSceneData(string currentScene)
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        itemManager = FindObjectOfType<ItemManager>();
        bossManager = FindObjectOfType<BossManager>();
        checkPointManager = FindObjectOfType<CheckpointManager>();
        npcManager = FindObjectOfType<NPCManager>();

        if (enemyManager != null)
        {
            enemyManager.LoadEnemies(currentScene);
        }

        if (itemManager != null)
        {
            itemManager.LoadItems(currentScene);
        }

        if (bossManager != null)
        {
            bossManager.LoadBoss(currentScene);
        }

        if (checkPointManager != null)
        {
            checkPointManager.LoadCheckpoint(currentScene);
        }

        if (npcManager != null)
        {
            npcManager.LoadNPC(currentScene);
        }
    }

    public List<SceneData> GetSceneDataList()
    {
        return sceneDataList;
    }

    public void RespawnEnemiesInAllScenes()
    {
        // Respawn enemies in other scenes
        foreach (SceneData sceneData in sceneDataList)
        {
            if (sceneData.enemies != null)
            {
                foreach (EnemyData enemy in sceneData.enemies)
                {
                    enemy.isAlive = true;
                }
            }
        }
    }
}
