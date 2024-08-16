using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerManager : MonoBehaviour
{
    public static SceneChangerManager Instance;

    public bool loadToGamePlay = false;
    public bool loadFromDoor = false;
    public bool loadToMainMenu = false;

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

    public void ChangeSceneFromDoor(SceneField myScene, SceneChanger.DoorToSpawnAt doorToSpawnAt)
    {
        loadFromDoor = true;
        Instance.StartCoroutine(Instance.ChangeSceneFromDoorCoroutine(myScene, doorToSpawnAt));
    }

    public void ChangeSceneToMainMenu(SceneField mainMenuScene)
    {
        loadToMainMenu = true;
        Instance.StartCoroutine(Instance.ChangeToMainMenuCoroutine(mainMenuScene));
    }

    public void ChangeSceneFromeSaveFile(GameData gameData)
    {
        loadToGamePlay = true;
        Instance.StartCoroutine(Instance.ChangeSceneFromSaveFileCoroutine(gameData.playerCheckpointData.sceneName));
    }

    public void ChangeSceneFromNewGameFile(SceneField newGameScene)
    {
        loadToGamePlay = true;
        Instance.StartCoroutine(Instance.ChangeSceneFromNewGameFileCoroutine(newGameScene));
    }

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

    private IEnumerator ActivatePlayerControl()
    {
        Player.Instance.CanMove = false;
        Player.Instance.IsMoving = false;

        while (SceneLoadManager.Instance.IsFadingIn)
        {
            yield return null;
        }

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
            // load player position
            player.ResetPlayerAnimation();
            player.transform.position = doorSpawnPosition.position;
        }

        SceneDataManager sceneDataManager = FindObjectOfType<SceneDataManager>();

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
            player.ResetPlayerAnimation();
            player.CurrentHealth = player.MaxHealth * 0.5f;
            player.CurrentStamina = player.MaxStamina * 0.25f;
            player.CurrentHealthPotion = 1;

            Transform newGamePosition = GameObject.Find("NewGamePosition").GetComponent<Transform>();

            if (newGamePosition != null)
            {
                player.transform.position = newGamePosition.position;
            }
        }

        MapRoomManager.Instance.RevealRoom();
        GameManager.Instance.isNewGame = false;
        loadToGamePlay = false;

        while (SceneLoadManager.Instance.IsLoading)
        {
            yield return null;
        }

        SceneLoadManager.Instance.StartFadeIn();
        player.Animator.SetTrigger(AnimationString.spawnTrigger);
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
            player.ResetPlayerAnimation();
            player.transform.position = new Vector3(gameData.playerCheckpointData.position[0], gameData.playerCheckpointData.position[1], gameData.playerCheckpointData.position[2]);
            player.CurrentHealth = gameData.playerData.health;
            player.CurrentStamina = gameData.playerData.stamina;
            player.CurrentHealthPotion = gameData.playerData.healthPotions;

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
        SceneDataManager sceneDataManager = FindObjectOfType<SceneDataManager>();

        if (sceneDataManager != null)
        {
            sceneDataManager.RespawnEnemiesInAllScenes();
            sceneDataManager.LoadSaveFileSceneData(gameData);
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


    private IEnumerator LoadMainMenuCoroutine()
    {
        SceneLoadManager.Instance.StartLoading(2f);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Destroy(player);
        }

        while (SceneLoadManager.Instance.IsLoading)
        {
            yield return null;
        }

        SceneLoadManager.Instance.StartFadeIn();
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Equals("MainMenu"))
        {
            if (loadFromDoor)
            {
                StartCoroutine(LoadNewScene(scene));
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
            }

        }
    }

    private void OnSceneUnloaded(UnityEngine.SceneManagement.Scene scene)
    {
        if (!scene.name.Equals("MainMenu"))
        {
            SceneDataManager.Instance.SaveSceneData(scene.name);
        }

        if (loadToMainMenu)
        {
            StartCoroutine(LoadMainMenuCoroutine());
        }
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
}

