using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMapData
{
    public List<MapData> mapData;

    public PlayerMapData(List<MapContainerData> mapContainerData)
    {
        mapData = new List<MapData>();

        foreach (var map in mapContainerData)
        {
            mapData.Add(new MapData(map.RoomScene.SceneName, map.HasRoomRevealed));
        }
    }
}
