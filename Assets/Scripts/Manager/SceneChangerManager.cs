using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerManager : MonoBehaviour
{
    public static SceneChangerManager Instance;

    public bool loadFromMainMenu = false;
    public bool loadFromDoor = false;
    public bool loadFromGameOptions = false;

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
        loadFromGameOptions = true;
        Instance.StartCoroutine(Instance.ChangeToMainMenuCoroutine(mainMenuScene));
    }

    public void ChangeSceneFromeSaveFile(GameData gameData)
    {
        loadFromMainMenu = true;
        Instance.StartCoroutine(Instance.ChangeSceneFromSaveFileCoroutine(gameData.playerCheckpointData.sceneName));
    }

    public void ChangeSceneFromNewGameFile(SceneField newGameScene)
    {
        loadFromMainMenu = true;
        Instance.StartCoroutine(Instance.ChangeSceneFromNewGameFileCoroutine(newGameScene));
    }

    private IEnumerator ChangeSceneFromDoorCoroutine(SceneField myScene, SceneChanger.DoorToSpawnAt doorToSpawnAt = SceneChanger.DoorToSpawnAt.None)
    {
        // run load scene
        Player.Instance.ResetPlayerAnimation();
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
        GameManager.Instance.isLoadGame = true;

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
        while (SceneLoadManager.Instance.IsFadingIn)
        {
            Player.Instance.CanMove = false;
            Player.Instance.IsMoving = false;
            yield return null;
        }

        Player.Instance.CanMove = true;
        Player.Instance.IsMoving = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneLoadManager.Instance.StartFadeIn();

        if (loadFromGameOptions)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Destroy(player);
            }
            loadFromGameOptions = false;
        }

        if (!scene.name.Equals("MainMenu"))
        {
            SceneDataManager.Instance.LoadSceneData(scene.name);

            if (loadFromDoor)
            {
                StartCoroutine(ActivatePlayerControl());
                FindDoor(doorToSpawnTo);
                Player.Instance.transform.position = doorSpawnPosition.position;
                loadFromDoor = false;
            }

            if (loadFromMainMenu)
            {
                if (GameManager.Instance.isNewGame)
                {
                    Player player = FindObjectOfType<Player>();

                    Transform newGamePosition = GameObject.Find("NewGamePosition").GetComponent<Transform>();

                    if (player != null && newGamePosition != null)
                    {
                        player.transform.position = newGamePosition.position;
                        player.CurrentHealth = player.MaxHealth * 0.5f;
                        player.CurrentStamina = player.MaxStamina * 0.25f;
                        player.CurrentHealthPotion = 1;
                        player.Animator.SetTrigger(AnimationString.spawnTrigger);
                    }

                    GameManager.Instance.isNewGame = false;
                    loadFromMainMenu = false;
                }
                else if (GameManager.Instance.isLoadGame)
                {
                    // Load game data
                    GameData gameData = GameManager.Instance.gameData;

                    // Load player data
                    Player player = FindObjectOfType<Player>();

                    if (player != null)
                    {
                        player.Animator.SetTrigger(AnimationString.spawnTrigger);

                        // Load player data
                        player.transform.position = new Vector3(gameData.playerCheckpointData.position[0], gameData.playerCheckpointData.position[1], gameData.playerCheckpointData.position[2]);
                        player.CurrentHealth = gameData.playerData.health;
                        player.CurrentStamina = gameData.playerData.stamina;
                        player.CurrentHealthPotion = gameData.playerData.healthPotions;

                        // Load inventory data
                        InventoryManager playerInventory = player.GetComponentInChildren<InventoryManager>();
                        if (playerInventory != null)
                        {
                            playerInventory.LoadSaveFileInventoriesData(gameData);
                        }

                        // Load map data
                        MapRoomManager playerMapData = player.GetComponentInChildren<MapRoomManager>();
                        if (playerMapData != null)
                        {
                            playerMapData.LoadSaveFileMapRoomsData(gameData);
                        }

                        GameManager.Instance.isLoadGame = false;
                        loadFromMainMenu = false;
                    }

                    // Load save file scene data
                    SceneDataManager playerSceneData = FindObjectOfType<SceneDataManager>();

                    if (playerSceneData != null)
                    {
                        playerSceneData.LoadSaveFileSceneData(gameData);
                    }
                }
                
            }

            MapRoomManager.Instance.RevealRoom();
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        SceneDataManager.Instance.SaveSceneData(scene.name);
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

