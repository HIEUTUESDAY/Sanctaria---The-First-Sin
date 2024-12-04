using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectTeleportSlot : MonoBehaviour
{
    public static SelectTeleportSlot Instance;

    public RectTransform roomCanvas; // The canvas or parent containing the rooms
    public float moveSpeed = 500f;

    // Define the boundaries for the movement zone
    public RectTransform movementZone;

    private Button selectedButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SetCurrentRoomSelected();
    }

    private void Update()
    {
        if (selectedButton != null)
        {
            CenterOnSelectedButton();
        }
    }

    public void SelectButton(Button button)
    {
        selectedButton = button;
    }

    private void CenterOnSelectedButton()
    {
        if (selectedButton != null)
        {
            RectTransform buttonRectTransform = selectedButton.GetComponent<RectTransform>();
            Vector2 targetPosition = -buttonRectTransform.anchoredPosition;
            targetPosition.x = Mathf.Clamp(targetPosition.x, movementZone.rect.xMin, movementZone.rect.xMax);
            targetPosition.y = Mathf.Clamp(targetPosition.y, movementZone.rect.yMin, movementZone.rect.yMax);
            roomCanvas.anchoredPosition = Vector2.Lerp(roomCanvas.anchoredPosition, targetPosition, moveSpeed * Time.unscaledDeltaTime);
        }
    }

    public void SetCurrentRoomSelected()
    {
        foreach (var room in MapRoomManager.Instance.rooms)
        {
            if (room.RoomScene == SceneManager.GetActiveScene().name)
            {
                EventSystem.current.SetSelectedGameObject(room.gameObject);
                return;
            }
        }
    }
}
