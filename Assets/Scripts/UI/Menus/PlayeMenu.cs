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
                GameData saveData = GameManager.Instance.LoadGame(i + 1);
                saveSlots[i].infoText.text = "Area: " + saveData.playerData.currentArea;
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
        GameManager.Instance.currentSlotIndex = slotIndex; // Set the current slot index in the GameManager
        StartCoroutine(LoadGameCoroutine(slotIndex));
    }

    private IEnumerator LoadGameCoroutine(int slotIndex)
    {
        GameData saveData = GameManager.Instance.LoadGame(slotIndex);
        if (saveData != null)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(saveData.currentScene);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            PlayerController player = FindObjectOfType<PlayerController>();
            GameManager.Instance.ApplyGameData(player, saveData);
            Debug.Log("Loaded Game from Slot " + slotIndex);
        }
        else
        {
            Debug.LogError("No save data found in slot " + slotIndex);
        }
    }

    public void StartNewGame(int slotIndex)
    {
        GameManager.Instance.currentSlotIndex = slotIndex; // Set the current slot index when starting a new game
        SceneManager.LoadScene("Level1.1");
    }
}
