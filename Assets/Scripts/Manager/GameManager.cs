using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private string savePath;
    [SerializeField] private int currentSlotIndex;

    public SceneField newGameScene;
    public GameData gameData;
    public bool isNewGame = false;
    public bool isLoadGame = false;
    public bool isRespawn = false;

    private Coroutine autoSaveCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }

        savePath = Application.persistentDataPath + "/";
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (gameData != null)
        {
            if (gameData.playerCheckpointData != null)
            {
                if (gameData.playerCheckpointData.position != null)
                {
                    StartAutoSave();
                }
                else
                {
                    StopAutoSave();
                }
            }
            else
            {
                StopAutoSave();
            }
        }
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

    #region Exit game

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }

    #endregion

    #region Auto Save Game

    public void StartAutoSave()
    {
        if (autoSaveCoroutine == null)
        {
            autoSaveCoroutine = StartCoroutine(AutoSaveRoutine());
        }
    }

    public void StopAutoSave()
    {
        if (autoSaveCoroutine != null)
        {
            StopCoroutine(autoSaveCoroutine);
            autoSaveCoroutine = null;
        }
    }

    private void AutoSave()
    {
        Player player = Player.Instance;
        InventoryManager inventoryManager = InventoryManager.Instance;
        MapRoomManager mapRoomManager = MapRoomManager.Instance;
        SceneDataManager sceneDataManager = SceneDataManager.Instance;

        if (player != null && inventoryManager != null && mapRoomManager != null && sceneDataManager != null)
        {
            PlayerData playerData = new PlayerData(player);
            PlayerCheckpointData playerCheckpointData = gameData.playerCheckpointData;
            PlayerInventoryData playerInventoryData = new PlayerInventoryData
            (
                inventoryManager.GetTearsAmount(),
                inventoryManager.GetQuestItemsInventory(),
                inventoryManager.GetHeartsInventory(),
                inventoryManager.GetPrayersInventory(),
                inventoryManager.GetHeartEquipment(),
                inventoryManager.GetPrayerEquipment()
            );
            PlayerMapData playerMapData = new PlayerMapData(mapRoomManager.GetMaps());
            PlayerSceneData playerSceneData = new PlayerSceneData(sceneDataManager.GetSceneDataList());

            SaveGame(playerData, playerCheckpointData, playerInventoryData, playerMapData, playerSceneData);
        }
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            AutoSave();
        }
    }

    #endregion
}
