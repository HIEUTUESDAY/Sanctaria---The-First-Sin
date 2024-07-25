using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public PlayerData playerData;
    public PlayerCheckpointData playerCheckpointData;
    public PlayerInventoryData playerInventoryData;
}

[System.Serializable]
public class PlayerData
{
    public float health;
    public float stamina;
    public int healthPotions;

    public PlayerData(Player player)
    {
        this.health = player.CurrentHealth;
        this.stamina = player.CurrentStamina;
        this.healthPotions = player.CurrentHealthPotion;
    }
}

[System.Serializable]
public class PlayerCheckpointData
{
    public string sceneName;
    public float[] position;

    public PlayerCheckpointData(string sceneName, Vector3 position)
    {
        this.sceneName = sceneName;
        this.position = new float[3];
        this.position[0] = position.x;
        this.position[1] = position.y;
        this.position[2] = position.z;
    }
}

[System.Serializable]
public class PlayerInventoryData
{
    public int tearsOfAtonement;

    public List<QuestItem> questItemsInventory;
    public List<MeaCulpaHeart> meaCulpaHeartsInventory;
    public List<Prayer> prayersInventory;

    public MeaCulpaHeart meaCulpaHeartEquipment;
    public Prayer prayerEquipment;

    public PlayerInventoryData(int tearsOfAtonement, List<QuestItem> questItemsInventory, List<MeaCulpaHeart> meaCulpaHeartsInventory, List<Prayer> prayersInventory, MeaCulpaHeart meaCulpaHeartEquipment, Prayer prayerEquipment)
    {
        this.tearsOfAtonement = tearsOfAtonement;

        this.questItemsInventory = questItemsInventory;
        this.meaCulpaHeartsInventory = meaCulpaHeartsInventory;
        this.prayersInventory = prayersInventory;

        this.meaCulpaHeartEquipment = meaCulpaHeartEquipment;
        this.prayerEquipment = prayerEquipment;
    }
}

