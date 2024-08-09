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

    public PlayerCheckpointData(string checkpointScene, Vector3 checkpointPosition)
    {
        this.sceneName = checkpointScene;
        this.position = new float[3];
        this.position[0] = checkpointPosition.x;
        this.position[1] = checkpointPosition.y;
        this.position[2] = checkpointPosition.z;
    }
}

[System.Serializable]
public class PlayerInventoryData
{
    public float tearsOfAtonement;

    public List<QuestItem> questItemsInventory;
    public List<MeaCulpaHeart> meaCulpaHeartsInventory;
    public List<Prayer> prayersInventory;

    public MeaCulpaHeart meaCulpaHeartEquipment;
    public Prayer prayerEquipment;

    public PlayerInventoryData(float tearsOfAtonement, List<QuestItem> questItemsInventory, List<MeaCulpaHeart> meaCulpaHeartsInventory, List<Prayer> prayersInventory, MeaCulpaHeart meaCulpaHeartEquipment, Prayer prayerEquipment)
    {
        this.tearsOfAtonement = tearsOfAtonement;
        this.questItemsInventory = questItemsInventory;
        this.meaCulpaHeartsInventory = meaCulpaHeartsInventory;
        this.prayersInventory = prayersInventory;
        this.meaCulpaHeartEquipment = meaCulpaHeartEquipment;
        this.prayerEquipment = prayerEquipment;

        SaveSpriteNames();
    }

    public void SaveSpriteNames()
    {
        foreach (var item in questItemsInventory)
        {
            item.SaveSpriteName();
        }
        foreach (var item in meaCulpaHeartsInventory)
        {
            item.SaveSpriteName();
        }
        foreach (var item in prayersInventory)
        {
            item.SaveSpriteName();
        }
        if (meaCulpaHeartEquipment != null)
        {
            meaCulpaHeartEquipment.SaveSpriteName();
        }
        if (prayerEquipment != null)
        {
            prayerEquipment.SaveSpriteName();
        }
    }

    public void LoadSprites()
    {
        foreach (var item in questItemsInventory)
        {
            item.LoadSprite();
        }
        foreach (var item in meaCulpaHeartsInventory)
        {
            item.LoadSprite();
        }
        foreach (var item in prayersInventory)
        {
            item.LoadSprite();
        }
        if (meaCulpaHeartEquipment != null)
        {
            meaCulpaHeartEquipment.LoadSprite();
        }
        if (prayerEquipment != null)
        {
            prayerEquipment.LoadSprite();
        }
    }
}

