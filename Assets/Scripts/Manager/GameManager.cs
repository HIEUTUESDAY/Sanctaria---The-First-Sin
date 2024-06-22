using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private string savePath;
    public int currentSlotIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/";
            currentSlotIndex = 1; // Set the default slot to 1
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame(PlayerController player)
    {
        SaveSystem.SaveGame(player, currentSlotIndex); // Save using the current slot index
    }

    public GameData LoadGame(int slotIndex)
    {
        currentSlotIndex = slotIndex; // Set the current slot index when loading
        return SaveSystem.LoadGame(slotIndex);
    }

    public void ApplyGameData(PlayerController player, GameData data)
    {
        if (player == null)
        {
            Debug.LogError("Player is null");
            return;
        }

        player.transform.position = new Vector3(data.playerData.position[0], data.playerData.position[1], data.playerData.position[2]);

        Damageable damageable = player.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.CurrentHealth = data.playerData.health;
            damageable.CurrentStamina = data.playerData.stamina;
            damageable.CurrentHealthPotion = data.playerData.healthPostions;
        }
        else
        {
            Debug.LogError("Damageable component not found on PlayerController");
        }

        player.currentArea = data.playerData.currentArea;
    }
}
