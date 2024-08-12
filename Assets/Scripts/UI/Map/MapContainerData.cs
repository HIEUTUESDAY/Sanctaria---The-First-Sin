using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapContainerData : MonoBehaviour
{
    public SceneField RoomScene;
    public GameObject PlayerIcon;
    [field: SerializeField] public bool HasRoomRevealed { get; set; }

    private void Start()
    {
        PlayerIcon.SetActive(false);
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == RoomScene && HasRoomRevealed == true)
        {
            PlayerIcon.SetActive(true);
        }
        else
        {
            PlayerIcon.SetActive(false);
        }
    }
}
