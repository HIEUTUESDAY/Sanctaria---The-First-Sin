using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TeleportMapManager : MonoBehaviour
{
    public static TeleportMapManager Instance;

    public List<MapContainerData> rooms;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void LockAllRoom()
    {
        foreach (MapContainerData room in rooms)
        {
            room.HasRoomRevealed = false;
            room.gameObject.SetActive(false);
        }
    }

    public void RevealRoom()
    {
        string newLoadedScene = SceneManager.GetActiveScene().name;

        foreach (MapContainerData room in rooms)
        {
            if (room.RoomScene.SceneName == newLoadedScene && !room.HasRoomRevealed)
            {
                room.HasRoomRevealed = true;
                room.gameObject.SetActive(true);
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

    public void SetCurrentRoomSelected()
    {
        foreach (var room in rooms)
        {
            if (room.RoomScene == SceneManager.GetActiveScene().name)
            {
                EventSystem.current.SetSelectedGameObject(room.gameObject);
                return;
            }
        }
    }
}
