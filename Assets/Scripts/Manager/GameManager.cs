using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private string savePath;
    public int currentSlotIndex;

    private Checkpoint currentCheckpoint;
    private GameData loadedGameData;
    private bool isLoadingGame = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/";
            currentSlotIndex = 1;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame(Player player)
    {
        SaveSystem.SaveGame(player, currentCheckpoint, currentSlotIndex);
    }

    public GameData LoadGame(int slotIndex)
    {
        currentSlotIndex = slotIndex;
        GameData data = SaveSystem.LoadGame(slotIndex);
        if (data != null)
        {
            currentCheckpoint = data.currentCheckpoint;
            loadedGameData = data; // Store the loaded game data
        }
        return data;
    }

    public void StartLoadingGame()
    {
        if (loadedGameData != null)
        {
            isLoadingGame = true;
            StartCoroutine(LoadGameCoroutine(loadedGameData.currentCheckpoint.sceneName));
        }
    }

    public void ApplyGameData(Player player, GameData data)
    {
        if (player != null)
        {
            player.transform.position = new Vector3(data.currentCheckpoint.position[0], data.currentCheckpoint.position[1], data.currentCheckpoint.position[2]);
            player.CurrentHealth = data.playerData.health;
            player.CurrentStamina = data.playerData.stamina;
            player.CurrentHealthPotion = data.playerData.healthPotions;
        }
    }

    public void RespawnPlayer(Player player)
    {
        if (currentCheckpoint != null)
        {
            player.transform.position = new Vector3(currentCheckpoint.position[0], currentCheckpoint.position[1], currentCheckpoint.position[2]);
            player.IsAlive = true;
        }
    }

    public void SetCurrentCheckpoint(Checkpoint checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    private IEnumerator LoadGameCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Player player = FindObjectOfType<Player>();
        if (player != null && loadedGameData != null)
        {
            ApplyGameData(player, loadedGameData);
            isLoadingGame = false;
            Debug.Log("Loaded Game from Slot " + currentSlotIndex);
        }
    }
}
