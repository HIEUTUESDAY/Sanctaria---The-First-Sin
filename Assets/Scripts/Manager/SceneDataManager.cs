using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDataManager : MonoBehaviour
{
    public static SceneDataManager Instance;
    private EnemyManager enemyManager;
    private ItemManager itemManager;
    public List<SceneData> sceneDataList;
    private List<EnemyData> enemyDataList;
    private List<ItemData> itemDataList;

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

            enemyDataList = enemyManager.SaveEnemies();
            itemDataList = itemManager.SaveItems();
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
                    return;
                }
            }

            SceneData newSceneData = new SceneData
            (
                currentScene,
                enemyDataList,
                itemDataList
            );

            sceneDataList.Add(newSceneData);
        }
        else
        {
            SceneData firstSceneData = new SceneData
            (
                currentScene,
                enemyDataList,
                itemDataList
            );

            sceneDataList.Add(firstSceneData);
        }
    }

    public void LoadSceneData(string currentScene)
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        itemManager = FindObjectOfType<ItemManager>();

        enemyManager.LoadEnemies(currentScene);
        itemManager.LoadItems(currentScene);
    }

    public List<SceneData> GetSceneDataList()
    {
        return sceneDataList;
    }

    public void LoadSaveFileSceneData(GameData gameData)
    {
        sceneDataList = GameManager.Instance.gameData.playerSceneData.scenesDataList;
        LoadSceneData(gameData.playerCheckpointData.sceneName);
    }

    public void RespawnEnemiesInAllScenes()
    {
        // Respawn enemies in other scenes
        foreach (SceneData sceneData in sceneDataList)
        {
            foreach (EnemyData enemy in sceneData.enemies)
            {
                enemy.isAlive = true;
            }
        }
    }
}
