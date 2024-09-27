using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInventoryData
{
    public float tearsOfAtonement;

    public List<QuestItem> questItemsInventory;
    public List<Heart> meaCulpaHeartsInventory;
    public List<Prayer> prayersInventory;

    public Heart meaCulpaHeartEquipment;
    public Prayer prayerEquipment;

    public PlayerInventoryData(float tearsOfAtonement, List<QuestItem> questItemsInventory, List<Heart> meaCulpaHeartsInventory, List<Prayer> prayersInventory, Heart meaCulpaHeartEquipment, Prayer prayerEquipment)
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
