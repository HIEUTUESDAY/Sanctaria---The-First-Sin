using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private Player player;
    [SerializeField] private MeaCulpaHeart equippedMeaCulpaHeart;
    [SerializeField] private Prayer equippedPrayer;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        EquipmentInitialize();
    }

    private void EquipmentInitialize()
    {
        equippedMeaCulpaHeart = InventoryManager.Instance.GetMeaCulpaHeartEquipment();
        AddMeaCulpaHeartBuffs(equippedMeaCulpaHeart);

        equippedPrayer = InventoryManager.Instance.GetPrayerEquipment();

    }

    public void UpdateEquippedMeaCulpaHeart()
    {
        RemoveMeaCulpaHeartBuffs();
        var newEquippedMeaCulpaHeart = InventoryManager.Instance.GetMeaCulpaHeartEquipment();
        AddMeaCulpaHeartBuffs(newEquippedMeaCulpaHeart);
        equippedMeaCulpaHeart = newEquippedMeaCulpaHeart;
    }

    public void UpdateEquippedPrayer()
    {
        var newEquippedPrayer = InventoryManager.Instance.GetPrayerEquipment();
    }

    public void AddMeaCulpaHeartBuffs( MeaCulpaHeart newEquippedMeaCulpaHeart)
    {
        if (newEquippedMeaCulpaHeart != null)
        {
            player.damageBuff = newEquippedMeaCulpaHeart.damageModifier;
            player.defenseBuff = newEquippedMeaCulpaHeart.defenseModifier;
            player.healthBuff = newEquippedMeaCulpaHeart.healthModifier;
            player.healthRegenBuff = newEquippedMeaCulpaHeart.healthRegenModifier;
            player.staminaBuff = newEquippedMeaCulpaHeart.staminaModifier;
            player.staminaRegenBuff = newEquippedMeaCulpaHeart.staminaRegenModifier;
            player.moveSpeedBuff = newEquippedMeaCulpaHeart.moveSpeedModifier;
            player.jumpPowerBuff = newEquippedMeaCulpaHeart.jumpPowerModifier;
            player.wallJumpPowerBuff = newEquippedMeaCulpaHeart.wallJumpPowerModifier;
            player.dashPowerBuff = newEquippedMeaCulpaHeart.dashPowerModifier;
        }
    }

    private void RemoveMeaCulpaHeartBuffs()
    {
        if(player != null)
        {
            player.damageBuff = 0f;
            player.defenseBuff = 0f;
            player.healthBuff = 0f;
            player.healthRegenBuff = 0f;
            player.staminaBuff = 0f;
            player.staminaRegenBuff = 0f;
            player.moveSpeedBuff = 0f;
            player.jumpPowerBuff = 0f;
            player.wallJumpPowerBuff = 0f;
            player.dashPowerBuff = 0f;
        }
    }

    public void PerformPrayer()
    {
        if (equippedPrayer == null) return;

        switch (equippedPrayer.itemName)
        {
            case "Tanranto of My Sister":
                Debug.Log("Using Tanranto of My Sister");
                break;

            // Add more cases for other Prayers
            default:
                break;
        }
    }

    #region Prayers function

    // Add functions for Prayers here

    #endregion
}
