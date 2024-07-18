using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayMenu : MonoBehaviour
{
    [System.Serializable]
    public class SaveSlot
    {
        public Button actionButton;
        public GameObject newGameImage;
        public GameObject loadGameImage;
        public TextMeshProUGUI infoText;
    }

    public List<SaveSlot> saveSlots;

    private string savePath;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/";
        RefreshSaveSlots();
    }

    public void RefreshSaveSlots()
    {
        for (int i = 0; i < saveSlots.Count; i++)
        {
            string filePath = savePath + "savefile" + (i + 1) + ".json";
            if (File.Exists(filePath))
            {
                saveSlots[i].newGameImage.SetActive(false);
                saveSlots[i].loadGameImage.SetActive(true);
                saveSlots[i].infoText.text = "Area: " + GameManager.Instance.LoadSaveSlotData(i + 1).checkpoint.sceneName;
            }
            else
            {
                saveSlots[i].newGameImage.SetActive(true);
                saveSlots[i].loadGameImage.SetActive(false);
                saveSlots[i].infoText.text = "New Game";
            }
        }
    }

    public void HandleSaveSlot(int slotIndex)
    {
        string filePath = savePath + "savefile" + slotIndex + ".json";
        if (File.Exists(filePath))
        {
            LoadGame(slotIndex);
        }
        else
        {
            StartNewGame(slotIndex);
        }
    }

    public void LoadGame(int slotIndex)
    {
        GameManager.Instance.LoadGame(slotIndex);
    }

    public void StartNewGame(int slotIndex)
    {
        GameManager.Instance.NewGame(slotIndex);
    }
}
