using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameData gameData;

    private string savePath;
    private int currentSlotIndex;

    public bool isRespawnPlayer = false;
    public bool isNewGame = false;
    public bool isLoadGame = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            savePath = Application.persistentDataPath + "/";
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

    public void SaveGame(PlayerData playerData, PlayerCheckpointData playerCheckpointData, PlayerInventoryData playerInventoryData)
    {
        SaveSystem.SaveGame(playerData, playerCheckpointData, playerInventoryData, currentSlotIndex);
    }

    public void NewGame(int slotIndex)
    {
        if (!isNewGame)
        {
            currentSlotIndex = slotIndex;
            StartCoroutine(NewGameCoroutine());
        }
    }

    public void LoadGame(int slotIndex)
    {
        if (!isLoadGame)
        {
            currentSlotIndex = slotIndex;
            gameData = SaveSystem.LoadGame(currentSlotIndex);

            if (gameData != null)
            {
                StartCoroutine(LoadGameCoroutine());
            }
        }
    }

    public void LoadGameData(Player player, GameData data)
    {
        if (player != null)
        {
            // Load player data
            player.transform.position = new Vector3(data.playerCheckpointData.position[0], data.playerCheckpointData.position[1], data.playerCheckpointData.position[2]);
            player.CurrentHealth = data.playerData.health;
            player.CurrentStamina = data.playerData.stamina;
            player.CurrentHealthPotion = data.playerData.healthPotions;

            // Load inventory data
            InventoryManager playerInventory = player.GetComponentInChildren<InventoryManager>();
            if (playerInventory != null)
            {
                playerInventory.LoadInventoriesData(data);
            }
        }
    }

    public void RespawnPlayer()
    {
        if (!isRespawnPlayer)
        {
            gameData = SaveSystem.LoadGame(currentSlotIndex);

            if (gameData != null)
            {
                StartCoroutine(RespawnPlayerCoroutine());
            }
        }
    }

    public void SetRespawnPlayerData(Player player)
    {
        player.IsAlive = true;
        player.transform.position = new Vector3(gameData.playerCheckpointData.position[0], gameData.playerCheckpointData.position[1], gameData.playerCheckpointData.position[2]);
        player.RestoreFullStats();
    }

    #endregion

    #region GameManager coroutines

    private IEnumerator NewGameCoroutine()
    {
        isNewGame = true;
        AsyncOperation asyncNewGame = SceneManager.LoadSceneAsync("Level1.1");

        while (!asyncNewGame.isDone)
        {
            yield return null;
        }

        Player player = FindObjectOfType<Player>();

        Transform newGamePosition = GameObject.Find("NewGamePosition").GetComponent<Transform>();

        if (player != null && newGamePosition != null)
        {
            player.transform.position = newGamePosition.position;
            isLoadGame = false;
        }
    }

    private IEnumerator LoadGameCoroutine()
    {
        isLoadGame = true;
        AsyncOperation asyncLoadGame = SceneManager.LoadSceneAsync(gameData.playerCheckpointData.sceneName);

        while (!asyncLoadGame.isDone)
        {
            yield return null;
        }

        Player player = FindObjectOfType<Player>();

        if (player != null && gameData != null)
        {
            LoadGameData(player, gameData);
            isLoadGame = false;
        }
    }

    private IEnumerator RespawnPlayerCoroutine()
    {
        isRespawnPlayer = true;
        AsyncOperation asyncLoadGame = SceneManager.LoadSceneAsync(gameData.playerCheckpointData.sceneName);

        while (!asyncLoadGame.isDone)
        {
            yield return null;
        }

        Player player = FindObjectOfType<Player>();

        if (player != null && gameData != null)
        {
            SetRespawnPlayerData(player);
            isRespawnPlayer = false;
        }
    }

    #endregion
}
