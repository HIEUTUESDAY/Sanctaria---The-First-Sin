using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCenterPoint : MonoBehaviour
{
    public static MapCenterPoint Instance;

    public RectTransform roomCanvas; // The canvas or parent containing the rooms
    public float moveSpeed = 500f;

    // Define the boundaries for the movement zone
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
        SetCenterToPlayerPoint();
    }

    private void Update()
    {
        MoveCenterPoint();

        if (Input.GetKeyDown(KeyCode.C))
        {
            SetCenterToPlayerPoint();
        }
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

        // Move the roomCanvas in the opposite direction to keep the centerPoint stationary
        Vector2 newPosition = roomCanvas.anchoredPosition - moveDirection * moveSpeed * Time.unscaledDeltaTime;

        // Clamp the position within the movement zone
        newPosition.x = Mathf.Clamp(newPosition.x, movementZone.rect.xMin, movementZone.rect.xMax);
        newPosition.y = Mathf.Clamp(newPosition.y, movementZone.rect.yMin, movementZone.rect.yMax);

        // Apply the clamped position
        roomCanvas.anchoredPosition = newPosition;

        // Check if the C key is pressed to center on the PlayerPoint
    }

    public void SetCenterToPlayerPoint()
    {
        // Find the MapRoomData with the active PlayerIcon
        MapContainerData targetRoom = null;

        foreach (var room in MapRoomManager.Instance.rooms)
        {
            if (room.PlayerIcon != null && room.PlayerIcon.activeSelf)
            {
                targetRoom = room;
                break;
            }
        }

        if (targetRoom != null)
        {
            // Calculate the target position to center the roomCanvas on the targetRoom
            Vector2 targetPosition = -targetRoom.GetComponent<RectTransform>().anchoredPosition;

            // Clamp the target position within the movement zone
            targetPosition.x = Mathf.Clamp(targetPosition.x, movementZone.rect.xMin, movementZone.rect.xMax);
            targetPosition.y = Mathf.Clamp(targetPosition.y, movementZone.rect.yMin, movementZone.rect.yMax);

            // Move the roomCanvas to center on the targetRoom
            roomCanvas.anchoredPosition = targetPosition;
        }
    }

}
