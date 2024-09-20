using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TeleportSlot : MonoBehaviour
{
    public SceneField SceneField;
    public Button button;
    public bool currentRoom;
    public bool isSelected;
    public GameObject shaderAnimation;
    public GameObject selection;

    private void Update()
    {
        if (Player.Instance.isKneelInCheckpoint)
        {
            TeleportSlotSelect();
            PerformTeleport();
        }
        else
        {
            shaderAnimation.SetActive(false);
        }
    }

    private void TeleportSlotSelect()
    {
        if (EventSystem.current.currentSelectedGameObject == button.gameObject)
        {
            SelectTeleportSlot.Instance.SelectButton(button);
            isSelected = true;
            shaderAnimation.SetActive(true);
            currentRoom = button.gameObject.GetComponent<MapContainerData>().CurrentRoom.activeSelf;

            if(currentRoom)
            {
                selection.SetActive(false);
            }
            else
            {
                selection.SetActive(true);
            }
        }
        else
        {
            isSelected = false;
            shaderAnimation.SetActive(false);
            currentRoom = false;
        }
    }

    private void PerformTeleport()
    {
        if(isSelected && !currentRoom && Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1;
            SceneChangerManager.Instance.ChangeSceneFromCheckpoint(SceneField);
        }
    }
}
