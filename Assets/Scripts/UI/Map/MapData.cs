using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    public string mapName;
    public bool isRevealed;

    public MapData(string mapName, bool isRevealed)
    {
        this.mapName = mapName;
        this.isRevealed = isRevealed;
    }
}
