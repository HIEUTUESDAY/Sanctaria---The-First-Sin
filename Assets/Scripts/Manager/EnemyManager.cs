using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("This scene DATA")]
    [SerializeField] private SceneData thisSceneData;
    [Space(5)]

    [Header("Drop Enemy in scene into this ENEMY OBJECTS LIST")]
    [SerializeField] private List<GameObject> enemyObjects;

    public List<EnemyData> SaveEnemies()
    {
        List<EnemyData> enemyDataList = new List<EnemyData>();

        foreach (GameObject enemyObject in enemyObjects)
        {
            if (enemyObject != null)
            {
                EnemyData enemyData = new EnemyData
                {
                    enemyName = enemyObject.name,
                    isAlive = enemyObject.activeSelf,
                    position = new float[] { enemyObject.transform.position.x, enemyObject.transform.position.y, enemyObject.transform.position.z }
                };

                enemyDataList.Add(enemyData);
            }
        }

        return enemyDataList;
    }

    public void LoadEnemies(string currentScene)
    {
        List<SceneData> sceneDataList = SceneDataManager.Instance.sceneDataList;

        foreach (SceneData sceneData in sceneDataList)
        {
            if (currentScene.Equals(sceneData.sceneName))
            {
                thisSceneData = sceneData;
                SetActiveEnemies();
                return;
            }
        }
    }

    private void SetActiveEnemies() 
    {
        if (thisSceneData != null)
        {
            foreach (EnemyData enemyData in thisSceneData.enemies)
            {
                GameObject enemy = GameObject.Find(enemyData.enemyName);

                if (enemy != null)
                {
                    if (enemyData.isAlive)
                    {
                        enemy.gameObject.SetActive(true);
                    }
                    else
                    {
                        enemy.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
