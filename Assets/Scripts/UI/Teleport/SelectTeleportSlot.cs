using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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
            // Get the RectTransform of the selected button
            RectTransform buttonRectTransform = selectedButton.GetComponent<RectTransform>();

            // Calculate the target position to center the roomCanvas on the selected button
            Vector2 targetPosition = -buttonRectTransform.anchoredPosition;

            // Clamp the target position within the movement zone
            targetPosition.x = Mathf.Clamp(targetPosition.x, movementZone.rect.xMin, movementZone.rect.xMax);
            targetPosition.y = Mathf.Clamp(targetPosition.y, movementZone.rect.yMin, movementZone.rect.yMax);

            // Move the roomCanvas towards the target position smoothly
            roomCanvas.anchoredPosition = Vector2.Lerp(roomCanvas.anchoredPosition, targetPosition, moveSpeed * Time.unscaledDeltaTime);
        }
    }
}
