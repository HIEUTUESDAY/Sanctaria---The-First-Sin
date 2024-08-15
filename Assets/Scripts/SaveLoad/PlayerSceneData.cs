using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSceneData
{
    public List<SceneData> scenesDataList;

    public PlayerSceneData(List<SceneData> scenesDataList)
    {
        this.scenesDataList = scenesDataList;
    }
}

