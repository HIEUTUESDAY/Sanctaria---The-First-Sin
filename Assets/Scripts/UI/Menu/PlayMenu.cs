using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
    [System.Serializable]
    public class SaveSlot
    {
        public Button newGameButton;
        public Button loadButton;
        public TextMeshProUGUI infoText;
        public int slotIndex;
    }

    public List<SaveSlot> saveSlots;

    private string savePath;

    void Start()
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
                saveSlots[i].loadButton.gameObject.SetActive(true);
                saveSlots[i].newGameButton.gameObject.SetActive(false);
                GameData saveData = GameManager.Instance.LoadSaveSlotData(i + 1);
                saveSlots[i].infoText.text = "Area: " + saveData.checkpoint.sceneName;
                int index = i + 1; // Capture the index for the button click event
                saveSlots[i].loadButton.onClick.RemoveAllListeners();
                saveSlots[i].loadButton.onClick.AddListener(() => LoadGame(index));
            }
            else
            {
                saveSlots[i].loadButton.gameObject.SetActive(false);
                saveSlots[i].newGameButton.gameObject.SetActive(true);
                saveSlots[i].infoText.text = "New Game";
                int index = i + 1; // Capture the index for the button click event
                saveSlots[i].newGameButton.onClick.RemoveAllListeners();
                saveSlots[i].newGameButton.onClick.AddListener(() => StartNewGame(index));
            }
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
