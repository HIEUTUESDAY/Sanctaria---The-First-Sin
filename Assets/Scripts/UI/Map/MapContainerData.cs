using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapContainerData : MonoBehaviour
{
    public SceneField RoomScene;
    [field: SerializeField] public bool HasRoomRevealed { get; set; }
}
