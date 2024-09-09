using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointManager : MonoBehaviour
{
    [Header("This scene DATA")]
    [SerializeField] private SceneData thisSceneData;
    [Space(5)]

    [Header("This checkpoint Area")]
    [SerializeField] private CheckPointArea checkPointArea;

    public enum CheckPointArea
    {
        Forest,
        Castle,
        Dungeon,
        Village,
        Mountain
    }

    private Animator animator;
    [SerializeField] private bool isActivated;

    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(AnimationString.isActivated, isActivated);
    }

    public void ActiveCheckPointThenSaveGame()
    {
        isActivated = true;
        StartCoroutine(SaveGameInCheckPoint());
    }

    public CheckPointData SaveCheckPoint()
    {
        CheckPointData checkPointData = new CheckPointData();

        checkPointData.isActived = isActivated;
        checkPointData.position = new float[] { gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z };

        return checkPointData;
    }

    public void LoadCheckpoint(string currentScene)
    {
        List<SceneData> sceneDataList = SceneDataManager.Instance.sceneDataList;

        foreach (SceneData sceneData in sceneDataList)
        {
            if (currentScene.Equals(sceneData.sceneName))
            {
                thisSceneData = sceneData;
                SetActiveCheckPoint();
                return;
            }
        }
    }

    private void SetActiveCheckPoint()
    {
        if (thisSceneData != null)
        {
            isActivated = thisSceneData.checkPoint.isActived;
        }
        else
        {
            isActivated = false;
        }
    }

    private IEnumerator SaveGameInCheckPoint()
    {
        while (!thisSceneData.checkPoint.isActived)
        {
            yield return null;
        }

        GameManager gameManager = GameManager.Instance;
        Player player = Player.Instance;
        InventoryManager inventoryManager = InventoryManager.Instance;
        MapRoomManager mapRoomManager = MapRoomManager.Instance;
        SceneDataManager sceneDataManager = SceneDataManager.Instance;
        sceneDataManager.SaveSceneData(SceneManager.GetActiveScene().name);

        if (gameManager != null && player != null && inventoryManager != null && mapRoomManager != null && sceneDataManager != null)
        {
            // Gather all player data
            PlayerData playerData = new PlayerData(player);
            PlayerCheckpointData playerCheckpointData = new PlayerCheckpointData( checkPointArea.ToString(), SceneManager.GetActiveScene().name, transform.position);
            PlayerInventoryData playerInventoryData = new PlayerInventoryData
            (
                inventoryManager.GetTearsAmount(),
                inventoryManager.GetQuestItemsInventory(),
                inventoryManager.GetMeaCulpaHeartsInventory(),
                inventoryManager.GetPrayersInventory(),
                inventoryManager.GetMeaCulpaHeartEquipment(),
                inventoryManager.GetPrayerEquipment()
            );
            PlayerMapData playerMapData = new PlayerMapData(mapRoomManager.GetMaps());

            // Gather scene data
            PlayerSceneData playerSceneData = new PlayerSceneData(sceneDataManager.GetSceneDataList());

            // Respawn enemies in all scenes
            sceneDataManager.RespawnEnemiesInAllScenes();

            // Save the game including scene data
            gameManager.SaveGame(playerData, playerCheckpointData, playerInventoryData, playerMapData, playerSceneData);
        }
    }
}

