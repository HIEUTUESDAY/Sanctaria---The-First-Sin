using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCheckpointData
{
    public string areaName;
    public string sceneName;
    public float[] position;

    public PlayerCheckpointData(string areaName, string checkpointScene, Vector3 checkpointPosition)
    {
        this.areaName = areaName;
        this.sceneName = checkpointScene;
        this.position = new float[3];
        this.position[0] = checkpointPosition.x;
        this.position[1] = checkpointPosition.y;
        this.position[2] = checkpointPosition.z;
    }
}
