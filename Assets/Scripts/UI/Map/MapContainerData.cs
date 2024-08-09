using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapContainerData : MonoBehaviour
{
    public SceneField RoomScene;
    public GameObject CenterPoint;
    [field: SerializeField] public bool HasRoomRevealed { get; set; }

    private void Start()
    {
        CenterPoint.SetActive(false);
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == RoomScene && HasRoomRevealed == true)
        {
            CenterPoint.SetActive(true);
        }
        else
        {
            CenterPoint.SetActive(false);
        }
    }
}
