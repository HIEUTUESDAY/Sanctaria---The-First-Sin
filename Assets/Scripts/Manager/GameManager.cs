using System.Collections;
using UnityEditor.Build;
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
            }
        }
    }

    public void RespawnPlayer()
    {
        if (!isRespawn)
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
        player.Animator.SetTrigger(AnimationString.spawnTrigger);
        player.transform.position = new Vector3(gameData.playerCheckpointData.position[0], gameData.playerCheckpointData.position[1], gameData.playerCheckpointData.position[2]);
        player.RestoreHealthAndPotion();
    }

    #endregion

    #region GameManager coroutines

    private IEnumerator RespawnPlayerCoroutine()
    {
        isRespawn = true;
        AsyncOperation asyncLoadGame = SceneManager.LoadSceneAsync(gameData.playerCheckpointData.sceneName);

        while (!asyncLoadGame.isDone)
        {
            yield return null;
        }

        Player player = FindObjectOfType<Player>();

        if (player != null && gameData != null)
        {
            SetRespawnPlayerData(player);
            isRespawn = false;
        }
    }

    #endregion
}
