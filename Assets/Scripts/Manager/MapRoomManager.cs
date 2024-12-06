using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapRoomManager : MonoBehaviour
{
    public static MapRoomManager Instance { get; private set; }

    [Header("Normal Rooms")]
    public List<MapContainerData> rooms;

    [Header("Map Menu HUD Update")]
    public GameObject mapCenterPoint;
    public GameObject selectTeleportSlot;
    public GameObject mapHUD;
    public GameObject teleportHUD;

    [Header("Hidden Rooms")]
    public List<SceneField> hiddenRoom;
    public bool isInHiddenRoom;
    [SerializeField] private GameObject roomHUD;
    [SerializeField] private GameObject hiddenRoomHUD;

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

        foreach (SceneField room  in hiddenRoom)
        {
            if (room.SceneName == newLoadedScene)
            {
                isInHiddenRoom = true;
                roomHUD.SetActive(false);
                hiddenRoomHUD.SetActive(true);
                selectTeleportSlot.SetActive(false);
                mapCenterPoint.SetActive(false);
            }
            else
            {
                isInHiddenRoom = false;
                hiddenRoomHUD.SetActive(false);
                roomHUD.SetActive(true);
            }
        }

        if (!isInHiddenRoom)
        {
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
