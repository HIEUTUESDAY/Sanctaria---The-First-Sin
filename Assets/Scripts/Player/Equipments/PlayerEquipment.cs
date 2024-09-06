using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private Player player;
    public MeaCulpaHeart equippedMeaCulpaHeart;
    public Prayer equippedPrayer;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        SetCurrentEquipment();
    }

    private void SetCurrentEquipment()
    {
        equippedMeaCulpaHeart = InventoryManager.Instance.GetMeaCulpaHeartEquipment();
        AddMeaCulpaHeartBuffs(equippedMeaCulpaHeart);

        equippedPrayer = InventoryManager.Instance.GetPrayerEquipment();
        AddMeaCulpaHeartBuffs(equippedMeaCulpaHeart);
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
        RemovePrayer();
        var newEquippedPrayer = InventoryManager.Instance.GetPrayerEquipment();
        AddPrayer(newEquippedPrayer);
        equippedPrayer = newEquippedPrayer;
    }

    public void AddMeaCulpaHeartBuffs(MeaCulpaHeart newEquippedMeaCulpaHeart)
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

    public void AddPrayer(Prayer newPrayer)
    {
        if (newPrayer != null)
        {
            equippedPrayer.itemName = newPrayer.itemName;
        }
    }

    private void RemovePrayer()
    {
        equippedPrayer.itemName = "";
        equippedPrayer.itemSprite = null;
        equippedPrayer.itemDescription = "";
        equippedPrayer.isItemEquipped = false;
    }

    public void PerformPrayer()
    {
        if (equippedPrayer.itemName == "") return;

        switch (equippedPrayer.itemName)
        {
            case "Verdiales of The Forsaken Hamlet":
                StartCoroutine(SpawnVerdialesProjectiles());
                break;

            // Add more cases for other Prayers
            default:
                Debug.Log("No Prayer Equipped");
                break;
        }
    }

    #region Prayers functions

    #region Verdiales of The Forsaken Hamlet

    [Header("Verdiales of The Forsaken Hamlet")]
    public GameObject verdialesProjectilePrefab; // Reference to the projectile prefab
    public Transform spawnVerdialesPoint;

    private IEnumerator SpawnVerdialesProjectiles()
    {
        // Determine the facing direction
        bool isFacingRight = player.IsFacingRight; // Assuming IsFacingRight is a public property

        // Calculate the spawn rotation based on the player's facing direction
        Quaternion spawnRotation = isFacingRight ? spawnVerdialesPoint.rotation : Quaternion.Euler(spawnVerdialesPoint.rotation.eulerAngles + new Vector3(0, 180, 0));

        // Instantiate the forward-moving projectile
        GameObject forwardProjectile = Instantiate(verdialesProjectilePrefab, spawnVerdialesPoint.position, spawnRotation);
        VerdialesProjectileMovement forwardMovement = forwardProjectile.GetComponent<VerdialesProjectileMovement>();
        forwardMovement.moveForward = true;

        // Instantiate the backward-moving projectile
        GameObject backwardProjectile = Instantiate(verdialesProjectilePrefab, spawnVerdialesPoint.position, spawnRotation);
        VerdialesProjectileMovement backwardMovement = backwardProjectile.GetComponent<VerdialesProjectileMovement>();
        backwardMovement.moveForward = false;

        yield return null;
    }


    #endregion

    #endregion
}
