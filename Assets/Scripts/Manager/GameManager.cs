using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameData gameData;

    public SceneField newGameScene;

    private string savePath;
    private int currentSlotIndex;

    public bool isNewGame = false;
    public bool isLoadGame = false;
    public bool isRespawn = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }

        savePath = Application.persistentDataPath + "/";
    }

    #region Load menus data

    public GameData LoadSaveSlotData(int slotIndex)
    {
        GameData data = SaveSystem.LoadGame(slotIndex);
        return data;
    }

    #endregion

    #region New, Load and Save game functions

    public void SaveGame(PlayerData playerData, PlayerCheckpointData playerCheckpointData, PlayerInventoryData playerInventoryData, PlayerMapData playerMapData, PlayerSceneData playerSceneData)
    {
        SaveSystem.SaveGame(playerData, playerCheckpointData, playerInventoryData, playerMapData, playerSceneData, currentSlotIndex);
    }

    public void NewGame(int slotIndex)
    {
        if(!isNewGame)
        {
            isNewGame = true;
            currentSlotIndex = slotIndex;
            gameData = new GameData();
            SceneChangerManager.Instance.ChangeSceneFromNewGameFile(newGameScene);
        }
    }

    public void LoadGame(int slotIndex)
    {
        if (!isLoadGame)
        {
            isLoadGame = true;
            currentSlotIndex = slotIndex;
            gameData = SaveSystem.LoadGame(currentSlotIndex);

            if (gameData != null)
            {
                SceneChangerManager.Instance.ChangeSceneFromeSaveFile(gameData);
                SceneDataManager.Instance.sceneDataList = gameData.playerSceneData.scenesDataList;
            }
        }
    }

    public void RespawnPlayer()
    {
        if (!isRespawn)
        {
            isRespawn = true;
            gameData = SaveSystem.LoadGame(currentSlotIndex);

            if (gameData != null)
            {
                SceneChangerManager.Instance.ChangeSceneFromeSaveFile(gameData);
            }
        }
    }

    #endregion
}
