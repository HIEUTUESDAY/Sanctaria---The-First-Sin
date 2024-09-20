using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneChangerManager : MonoBehaviour
{
    public static SceneChangerManager Instance;

    public bool loadToGamePlay = false;
    public bool loadFromDoor = false;
    public bool loadToMainMenu = false;
    public bool loadFromCheckpoint = false;

    private SceneChanger.DoorToSpawnAt doorToSpawnTo;

    private Transform doorSpawnPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    #region Change scenes functions

    public void ChangeSceneFromDoor(SceneField myScene, SceneChanger.DoorToSpawnAt doorToSpawnAt)
    {
        loadFromDoor = true;
        StartCoroutine(ChangeSceneFromDoorCoroutine(myScene, doorToSpawnAt));
    }

    public void ChangeSceneToMainMenu(SceneField mainMenuScene)
    {
        loadToMainMenu = true;
        StartCoroutine(ChangeToMainMenuCoroutine(mainMenuScene));
    }

    public void ChangeSceneFromeSaveFile(GameData gameData)
    {
        loadToGamePlay = true;
        StartCoroutine(ChangeSceneFromSaveFileCoroutine(gameData.playerCheckpointData.sceneName));
    }

    public void ChangeSceneFromNewGameFile(SceneField newGameScene)
    {
        loadToGamePlay = true;
        StartCoroutine(ChangeSceneFromNewGameFileCoroutine(newGameScene));
    }

    public void ChangeSceneFromCheckpoint(SceneField checkpointScene)
    {
        loadFromCheckpoint = true;
        StartCoroutine(ChangeSceneFromCheckpointCoroutine(checkpointScene));
    }

    private void FindDoor(SceneChanger.DoorToSpawnAt doorSpawnNumber)
    {
        SceneChanger[] doors = FindObjectsOfType<SceneChanger>();

        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i].CurrentDoor == doorSpawnNumber)
            {
                doorSpawnPosition = doors[i].spawnPosition.transform;
                return;
            }
        }
    }

    #endregion

    #region Coroutine to change scenes

    private IEnumerator ChangeSceneFromDoorCoroutine(SceneField myScene, SceneChanger.DoorToSpawnAt doorToSpawnAt = SceneChanger.DoorToSpawnAt.None)
    {
        SceneLoadManager.Instance.StartFadeOut();

        // keep loading
        while (SceneLoadManager.Instance.IsFadingOut)
        {
            yield return null;
        }

        doorToSpawnTo = doorToSpawnAt;
        SceneManager.LoadScene(myScene);
    }

    private IEnumerator ChangeToMainMenuCoroutine(SceneField mainMenuScene)
    {
        // run load scene
        SceneLoadManager.Instance.StartFadeOut();

        // keep loading
        while (SceneLoadManager.Instance.IsFadingOut)
        {
            yield return null;
        }

        SceneManager.LoadScene(mainMenuScene);
    }

    private IEnumerator ChangeSceneFromSaveFileCoroutine(string saveFileScene)
    {
        // run load scene
        SceneLoadManager.Instance.StartFadeOut();

        // keep loading
        while (SceneLoadManager.Instance.IsFadingOut)
        {
            yield return null;
        }

        SceneManager.LoadScene(saveFileScene);
    }

    private IEnumerator ChangeSceneFromNewGameFileCoroutine(SceneField newGameScene)
    {
        // run load scene
        SceneLoadManager.Instance.StartFadeOut();

        // keep loading
        while (SceneLoadManager.Instance.IsFadingOut)
        {
            yield return null;
        }

        SceneManager.LoadScene(newGameScene);
    }

    private IEnumerator ChangeSceneFromCheckpointCoroutine(SceneField checkpointScene)
    {
        // run load scene
        SceneLoadManager.Instance.StartFadeOut();

        // keep loading
        while (SceneLoadManager.Instance.IsFadingOut)
        {
            yield return null;
        }

        Player.Instance.isKneelInCheckpoint = false;
        UIManager.Instance.mapMenu.SetActive(false);
        UIManager.Instance.menuActivated = false;
        SceneManager.LoadScene(checkpointScene);
    }

    #endregion

    #region Coroutines after loaded scenes

    private IEnumerator ActivatePlayerControl()
    {
        Player.Instance.CanMove = false;
        Player.Instance.IsMoving = false;

        while (SceneLoadManager.Instance.IsFadingIn)
        {
            yield return null;
        }

        // enable player input
        PlayerInput playerInput = Player.Instance.GetComponent<PlayerInput>();
        playerInput.enabled = true;
        Player.Instance.CanMove = true;
        Player.Instance.IsMoving = Player.Instance.HorizontalInput != Vector2.zero;
    }

    private IEnumerator LoadNewScene(UnityEngine.SceneManagement.Scene scene)
    {
        SceneLoadManager.Instance.StartLoading(2f);

        FindDoor(doorToSpawnTo);

        Player player = Player.Instance;

        if (player != null)
        {
            // load player data
            player.transform.position = doorSpawnPosition.position;

            player.ResetPlayerAnimation();
        }

        SceneDataManager sceneDataManager = SceneDataManager.Instance;

        if (sceneDataManager != null)
        {
            sceneDataManager.LoadSceneData(scene.name);
        }

        MapRoomManager.Instance.RevealRoom();
        loadFromDoor = false;

        while (SceneLoadManager.Instance.IsLoading)
        {
            yield return null;
        }

        SceneLoadManager.Instance.StartFadeIn();
        StartCoroutine(ActivatePlayerControl());
    }
    
    private IEnumerator LoadNewGameCoroutine()
    {
        SceneLoadManager.Instance.StartLoading(2f);

        Player player = FindObjectOfType<Player>();

        if (player != null)
        {
            Transform newGamePosition = GameObject.Find("NewGamePosition").GetComponent<Transform>();

            if (newGamePosition != null)
            {
                player.transform.position = newGamePosition.position;
            }

            player.ResetPlayerAnimation();
        }

        SceneDataManager sceneDataManager = SceneDataManager.Instance;

        if (sceneDataManager != null)
        {
            sceneDataManager.LoadSceneData(SceneManager.GetActiveScene().name);
        }

        MapRoomManager.Instance.LockAllRoom();
        MapRoomManager.Instance.RevealRoom();
        GameManager.Instance.isNewGame = false;
        loadToGamePlay = false;

        while (SceneLoadManager.Instance.IsLoading)
        {
            yield return null;
        }

        SceneLoadManager.Instance.StartFadeIn();
        StartCoroutine(WaitForEnterCoroutine(player));
    }

    private IEnumerator WaitForEnterCoroutine(Player player)
    {
        player.Animator.SetBool(AnimationString.isWaitForEnter, true);

        while (player.IsWaitForEnter)
        {
            yield return null;
        }

        // Load player data
        player.CurrentHealth = player.MaxHealth * 0.25f;
        player.CurrentMana = player.MaxMana * 0.25f;
        player.CurrentHealthPotion = 2;
    }

    private IEnumerator LoadSaveGameCoroutine()
    {
        SceneLoadManager.Instance.StartLoading(2f);

        // Load game data
        GameData gameData = GameManager.Instance.gameData;

        // Load player data
        Player player = FindObjectOfType<Player>();

        if (player != null)
        {
            // Load player data
            player.transform.position = new Vector3(gameData.playerCheckpointData.position[0], gameData.playerCheckpointData.position[1], gameData.playerCheckpointData.position[2]);
            player.CurrentHealth = gameData.playerData.health;
            player.CurrentMana = gameData.playerData.stamina;
            player.CurrentHealthPotion = gameData.playerData.healthPotions;

            player.ResetPlayerAnimation();

            // Load inventory data
            InventoryManager inventoryManager = player.GetComponentInChildren<InventoryManager>();
            if (inventoryManager != null)
            {
                inventoryManager.LoadSaveFileInventoriesData(gameData);
            }

            // Load map data
            MapRoomManager mapRoomManager = player.GetComponentInChildren<MapRoomManager>();
            if (mapRoomManager != null)
            {
                mapRoomManager.LoadSaveFileMapRoomsData(gameData);
            }
        }

        // Load save file scene data
        SceneDataManager sceneDataManager = SceneDataManager.Instance;

        if (sceneDataManager != null)
        {
            sceneDataManager.LoadSceneData(SceneManager.GetActiveScene().name);
            sceneDataManager.RespawnEnemiesInAllScenes();
        }

        MapRoomManager.Instance.RevealRoom();
        GameManager.Instance.isLoadGame = false;
        loadToGamePlay = false;

        while (SceneLoadManager.Instance.IsLoading)
        {
            yield return null;
        }

        SceneLoadManager.Instance.StartFadeIn();
        player.Animator.SetTrigger(AnimationString.spawnTrigger);
    }

    private IEnumerator RespawnPlayerCoroutine()
    {
        SceneLoadManager.Instance.StartLoading(2f);

        // Load game data
        GameData gameData = GameManager.Instance.gameData;

        // Load player data
        Player player = Player.Instance;

        if (player != null)
        {
            // Reset death title
            DeathFadeInTitle deathFadeIn = player.playerDeathTitle.GetComponent<DeathFadeInTitle>();

            if (deathFadeIn != null)
            {
                deathFadeIn.SetInitialAlphaValues();
            }

            // Load player data
            player.IsAlive = true;
            player.transform.position = new Vector3(gameData.playerCheckpointData.position[0], gameData.playerCheckpointData.position[1], gameData.playerCheckpointData.position[2]);
            player.CurrentHealth = player.MaxHealth;
            player.CurrentMana = player.CurrentMana;
            player.CurrentHealthPotion = player.MaxHealthPotion;

            player.ResetPlayerAnimation();
        }

        // Load save file scene data
        SceneDataManager sceneDataManager = SceneDataManager.Instance;

        if (sceneDataManager != null)
        {
            sceneDataManager.LoadSceneData(SceneManager.GetActiveScene().name);
            sceneDataManager.RespawnEnemiesInAllScenes();
        }

        MapRoomManager.Instance.RevealRoom();
        GameManager.Instance.isRespawn = false;
        loadToGamePlay = false;

        while (SceneLoadManager.Instance.IsLoading)
        {
            yield return null;
        }

        SceneLoadManager.Instance.StartFadeIn();
        player.Animator.SetTrigger(AnimationString.spawnTrigger);
    }

    private IEnumerator LoadMainMenuCoroutine()
    {
        SceneLoadManager.Instance.StartLoading(2f);

        SceneDataManager.Instance.sceneDataList.Clear();

        GameObject player = Player.Instance.gameObject;

        if (player != null)
        {
            Destroy(player);
        }

        loadToMainMenu = false;

        while (SceneLoadManager.Instance.IsLoading)
        {
            yield return null;
        }

        SceneLoadManager.Instance.StartFadeIn();
    }

    private IEnumerator LoadCheckpointScene(UnityEngine.SceneManagement.Scene scene)
    {
        SceneLoadManager.Instance.StartLoading(2f);

        CheckpointManager checkPoint = FindObjectOfType<CheckpointManager>();

        Player player = Player.Instance;

        if (player != null)
        {
            // load player data
            player.transform.position = checkPoint.transform.position;

            player.ResetPlayerAnimation();
        }

        SceneDataManager sceneDataManager = SceneDataManager.Instance;

        if (sceneDataManager != null)
        {
            sceneDataManager.LoadSceneData(scene.name);
        }

        MapRoomManager.Instance.RevealRoom();
        loadFromCheckpoint = false;

        while (SceneLoadManager.Instance.IsLoading)
        {
            yield return null;
        }

        SceneLoadManager.Instance.StartFadeIn();
        player.Animator.SetTrigger(AnimationString.spawnTrigger);
    }

    #endregion

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Equals("MainMenu"))
        {
            if (loadFromDoor)
            {
                StartCoroutine(LoadNewScene(scene));
            }
            else if (loadFromCheckpoint)
            {
                StartCoroutine(LoadCheckpointScene(scene));
            }

            if (loadToGamePlay)
            {
                if (GameManager.Instance.isNewGame)
                {
                    StartCoroutine(LoadNewGameCoroutine());
                }
                else if (GameManager.Instance.isLoadGame)
                {
                    StartCoroutine(LoadSaveGameCoroutine());
                }
                else if (GameManager.Instance.isRespawn)
                {
                    StartCoroutine(RespawnPlayerCoroutine());
                }
            }
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (loadToMainMenu)
        {
            StartCoroutine(LoadMainMenuCoroutine());
        }
    }

}
