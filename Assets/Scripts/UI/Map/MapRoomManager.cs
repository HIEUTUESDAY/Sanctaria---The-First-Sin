using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapRoomManager : MonoBehaviour
{
    public static MapRoomManager Instance;

    [SerializeField] private MapContainerData[] rooms;

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

        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].RoomScene.SceneName == newLoadedScene && !rooms[i].HasRoomRevealed)
            {
                rooms[i].gameObject.SetActive(true);
                rooms[i].HasRoomRevealed = true;

                return;
            }
        }
    }
}
