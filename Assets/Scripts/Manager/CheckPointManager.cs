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
        GameManager gameManager = FindObjectOfType<GameManager>();
        Player player = FindObjectOfType<Player>();
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (gameManager != null && player != null && inventoryManager != null)
        {
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
            gameManager.SaveGame(playerData, playerCheckpointData, playerInventoryData);
        }
    }

    public void RespawnEnemiesAfterSpawn()
    {
        enemyManager.RespawnAllEnemies();
    }

    public void ActivateCheckPoint()
    {
        if (IsActivate == false)
        {
            IsActivate = true;
        }
    }
}
