using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    public string sceneName;
    public List<EnemyData> enemies;
    public List<ItemData> items;
    public BossData boss;
    public CheckPointData checkPoint;

    public SceneData(string sceneName, List<EnemyData> enemies, List<ItemData> items, BossData boss, CheckPointData checkPoint)
    {
        this.sceneName = sceneName;
        this.enemies = enemies;
        this.items = items;
        this.boss = boss;
        this.checkPoint = checkPoint;
    }
}
