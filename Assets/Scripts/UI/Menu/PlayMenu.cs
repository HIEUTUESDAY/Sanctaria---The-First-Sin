using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayMenu : MonoBehaviour
{
    public List<SaveSlot> saveSlots;

    private string savePath;

    public GameObject newGameSelection;
    public GameObject loadGameSelection;
    public GameObject deleteGameSelection;

    [SerializeField] private Image fillDeleteImage;

    public UnityEvent backToMainMenu;

    private Coroutine deleteCoroutine;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/";
        RefreshSaveSlots();
    }

    private void Update()
    {
        SaveSlotSelected();
        BackToMainMenu();
    }

    public void RefreshSaveSlots()
    {
        for (int i = 0; i < saveSlots.Count; i++)
        {
            int slotIndex = i + 1;
            string filePath = savePath + "savefile" + slotIndex + ".json";

            if (File.Exists(filePath))
            {
                saveSlots[i].newGameImage.SetActive(false);
                saveSlots[i].loadGameImage.SetActive(true);
                saveSlots[i].currentAreText.text = GameManager.Instance.LoadSaveSlotData(slotIndex).playerCheckpointData.areaName;
                saveSlots[i].tearsOfAtonementText.text = ((int)GameManager.Instance.LoadSaveSlotData(slotIndex).playerInventoryData.tearsOfAtonement).ToString();
            }
            else
            {
                saveSlots[i].newGameImage.SetActive(true);
                saveSlots[i].loadGameImage.SetActive(false);
                saveSlots[i].currentAreText.text = "";
                saveSlots[i].tearsOfAtonementText.text = "";
            }

            saveSlots[i].actionButton.onClick.RemoveAllListeners();
            saveSlots[i].actionButton.onClick.AddListener(() => HandleSaveSlot(slotIndex));
        }
    }

    public void HandleSaveSlot(int slotIndex)
    {
        string filePath = savePath + "savefile" + slotIndex + ".json";

        if (File.Exists(filePath))
        {
            LoadGame(slotIndex);
            SoundFXManager.Instance.PlayEquipItemSound();
        }
        else
        {
            StartNewGame(slotIndex);
            SoundFXManager.Instance.PlayEquipItemSound();
        }
    }

    private void SaveSlotSelected()
    {
        for (int i = 0; i < saveSlots.Count; i++)
        {
            int slotIndex = i + 1;
            string filePath = savePath + "savefile" + slotIndex + ".json";

            if (EventSystem.current.currentSelectedGameObject == saveSlots[i].actionButton.gameObject)
            {
                if (File.Exists(filePath))
                {
                    loadGameSelection.SetActive(true);
                    deleteGameSelection.SetActive(true);
                    newGameSelection.SetActive(false);
                }
                else
                {
                    newGameSelection.SetActive(true);
                    loadGameSelection.SetActive(false);
                    deleteGameSelection.SetActive(false);
                }

                if (Input.GetKey(KeyCode.K))
                {
                    if (deleteCoroutine == null)
                    {
                        deleteCoroutine = StartCoroutine(HandleDeleteSave(slotIndex));
                    }
                }
                else
                {
                    if (deleteCoroutine != null)
                    {
                        StopCoroutine(deleteCoroutine);
                        deleteCoroutine = null;
                        fillDeleteImage.fillAmount = 0;
                    }
                }
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

    private void BackToMainMenu()
    {
        if (gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            backToMainMenu.Invoke();
            SoundFXManager.Instance.PlayChangeTabSound();
        }
    }

    private IEnumerator HandleDeleteSave(int slotIndex)
    {
        float holdTime = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < holdTime)
        {
            elapsedTime += Time.deltaTime;
            fillDeleteImage.fillAmount = elapsedTime / holdTime;
            yield return null;
        }

        string filePath = savePath + "savefile" + slotIndex + ".json";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            SoundFXManager.Instance.PlayEquipItemSound();
            RefreshSaveSlots();
        }

        fillDeleteImage.fillAmount = 0;
        deleteCoroutine = null;
    }
}

[System.Serializable]
public class SaveSlot
{
    public Button actionButton;
    public GameObject newGameImage;
    public GameObject loadGameImage;
    public GameObject deleteGameImage;
    public TMP_Text currentAreText;
    public TMP_Text tearsOfAtonementText;
}
