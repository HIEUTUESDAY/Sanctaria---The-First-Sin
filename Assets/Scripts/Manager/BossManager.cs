using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("This scene DATA")]
    [SerializeField] private SceneData thisSceneData;
    [Space(5)]

    [Header("Drop Boss in scene into this BOSS OBJECT")]
    [SerializeField] private GameObject bossObject;

    public BossData SaveBoss()
    {
        BossData bossData = new BossData();

        if (bossObject != null)
        {
            Enemy boss = bossObject.GetComponent<Enemy>();

            if (boss != null)
            {
                bossData.bossName = bossObject.name;
                bossData.isAlive = bossObject.activeSelf;
                bossData.position = new float[] { bossObject.transform.position.x, bossObject.transform.position.y, bossObject.transform.position.z };
            }
        }

        return bossData;
    }

    public void LoadBoss(string currentScene)
    {
        List<SceneData> sceneDataList = SceneDataManager.Instance.sceneDataList;

        foreach (SceneData sceneData in sceneDataList)
        {
            if (currentScene.Equals(sceneData.sceneName))
            {
                thisSceneData = sceneData;
                SetActiveBoss();
                return;
            }
        }
    }

    private void SetActiveBoss()
    {
        if (thisSceneData != null)
        {
            GameObject boss = GameObject.Find(thisSceneData.boss.bossName);

            if (boss != null)
            {
                if (thisSceneData.boss.isAlive)
                {
                    boss.gameObject.SetActive(true);
                }
                else
                {
                    boss.gameObject.SetActive(false);
                }
            }
        }
    }
}
