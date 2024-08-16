using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapRoomManager : MonoBehaviour
{
    public static MapRoomManager Instance;

    public List<MapContainerData> rooms;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void RevealRoom()
    {
        string newLoadedScene = SceneManager.GetActiveScene().name;

        foreach (MapContainerData room in rooms)
        {
            if (room.RoomScene.SceneName == newLoadedScene && !room.HasRoomRevealed)
            {
                room.gameObject.SetActive(true);
                room.HasRoomRevealed = true;

                return;
            }
        }
    }

    public void LoadSaveFileMapRoomsData(GameData gameData)
    {
        foreach (var mapData in gameData.playerMapData.mapData)
        {
            foreach (var room in rooms)
            {
                if (room.RoomScene.SceneName == mapData.mapName)
                {
                    room.HasRoomRevealed = mapData.isRevealed;
                    room.gameObject.SetActive(mapData.isRevealed);
                    break;
                }
            }
        }
    }

    public List<MapContainerData> GetMaps()
    {
        return rooms;
    }
}