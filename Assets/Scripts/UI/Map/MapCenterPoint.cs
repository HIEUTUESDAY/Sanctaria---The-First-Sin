using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCenterPoint : MonoBehaviour
{
    public static MapCenterPoint Instance;

    public RectTransform roomCanvas;
    public float moveSpeed = 500f;

    private MapContainerData currentRoom;

    public RectTransform movementZone;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SetCenterPoint();
    }

    private void Update()
    {
        MoveCenterPoint();
    }

    private void MoveCenterPoint()
    {
        Vector2 moveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection.y -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection.x += 1;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SetCenterPoint();
        }

        Vector2 newPosition = roomCanvas.anchoredPosition - moveDirection * moveSpeed * Time.unscaledDeltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, movementZone.rect.xMin, movementZone.rect.xMax);
        newPosition.y = Mathf.Clamp(newPosition.y, movementZone.rect.yMin, movementZone.rect.yMax);
        roomCanvas.anchoredPosition = newPosition;
    }

    public void SetCenterPoint()
    {
        foreach (var room in MapRoomManager.Instance.rooms)
        {
            if (room.CurrentRoom.activeSelf)
            {
                currentRoom = room;
                break;
            }
        }

        if (currentRoom != null)
        {
            Vector2 targetPosition = -currentRoom.GetComponent<RectTransform>().anchoredPosition;
            targetPosition.x = Mathf.Clamp(targetPosition.x, movementZone.rect.xMin, movementZone.rect.xMax);
            targetPosition.y = Mathf.Clamp(targetPosition.y, movementZone.rect.yMin, movementZone.rect.yMax);
            roomCanvas.anchoredPosition = targetPosition;
        }
    }

}
