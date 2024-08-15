using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointManager : MonoBehaviour
{
    private Animator animator;
    private EnemyManager enemyManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    private bool isActivate = false;

    public bool IsActivate
    {
        get { return isActivate; }
        set
        {
            isActivate = value;
            if (isActivate)
            {
                animator.SetBool(AnimationString.isActivated, true);
            }
        }
    }

    public void SaveCheckPoint()
    {
        GameManager gameManager = GameManager.Instance;
        Player player = Player.Instance;
        InventoryManager inventoryManager = InventoryManager.Instance;
        MapRoomManager mapRoomManager = MapRoomManager.Instance;
        SceneDataManager sceneDataManager = SceneDataManager.Instance;

        if (gameManager != null && player != null && inventoryManager != null && mapRoomManager != null && sceneDataManager != null)
        {
            // Gather all player data
            PlayerData playerData = new PlayerData(player);
            PlayerCheckpointData playerCheckpointData = new PlayerCheckpointData(SceneManager.GetActiveScene().name, transform.position);
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

            // Save the game including scene data
            gameManager.SaveGame(playerData, playerCheckpointData, playerInventoryData, playerMapData, playerSceneData);
        }
    }

    public void RespawnEnemiesAfterSpawn()
    {
        
    }

    public void ActivateCheckPoint()
    {
        if (!IsActivate)
        {
            IsActivate = true;
        }
    }
}
