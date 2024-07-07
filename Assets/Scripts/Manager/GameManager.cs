using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GameData gameData;

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
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/";
        }
        else
        {
            Destroy(gameObject);
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

    public void SaveGame(Player player, Checkpoint checkpoint)
    {
        SaveSystem.SaveGame(player, checkpoint, currentSlotIndex);
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
            player.transform.position = new Vector3(data.checkpoint.position[0], data.checkpoint.position[1], data.checkpoint.position[2]);
            player.CurrentHealth = data.playerData.health;
            player.CurrentStamina = data.playerData.stamina;
            player.CurrentHealthPotion = data.playerData.healthPotions;
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
        player.transform.position = new Vector3(gameData.checkpoint.position[0], gameData.checkpoint.position[1], gameData.checkpoint.position[2]);
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
        AsyncOperation asyncLoadGame = SceneManager.LoadSceneAsync(gameData.checkpoint.sceneName);

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
        AsyncOperation asyncLoadGame = SceneManager.LoadSceneAsync(gameData.checkpoint.sceneName);

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
