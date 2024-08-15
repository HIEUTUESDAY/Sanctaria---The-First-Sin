using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapContainerData : MonoBehaviour
{
    public SceneField RoomScene;
    public GameObject CurrentRoom;
    [field: SerializeField] public bool HasRoomRevealed { get; set; }

    private void Start()
    {
        CurrentRoom.SetActive(false);
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == RoomScene && HasRoomRevealed == true)
        {
            CurrentRoom.SetActive(true);
        }
        else
        {
            CurrentRoom.SetActive(false);
        }
    }
}
