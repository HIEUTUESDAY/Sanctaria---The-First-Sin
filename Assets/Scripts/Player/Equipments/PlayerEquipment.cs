using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private Player player;
    public Heart equippedHeart;
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
        equippedHeart = InventoryManager.Instance.GetHeartEquipment();
        if(equippedHeart != null)
        {
            AddHeartBuffs(equippedHeart);
        }
        else
        {
            RemoveHeartBuffs();
        }

        equippedPrayer = InventoryManager.Instance.GetPrayerEquipment();
        if (equippedPrayer != null)
        {
            AddPrayer(equippedPrayer);
        }
        else
        {
            RemovePrayer();
        }
    }

    public void PerformPrayer()
    {
        if (player.prayerCooldown > 0 || equippedPrayer.itemName.Equals(null))
        {
            return;
        }

        switch (equippedPrayer.itemName)
        {
            case "Verdiales of The Forsaken Hamlet":
                SpawnVerdialesProjectiles();
                break;

            default:
                Debug.Log("No Prayer Equipped");
                break;
        }

        player.prayerCooldown = player.prayerCooldownTime;
    }

    public void AddHeartBuffs(Heart newHeart)
    {
        if (newHeart != null)
        {
            player.damageBuff = newHeart.damageModifier;
            player.defenseBuff = newHeart.defenseModifier;
            player.healthBuff = newHeart.healthModifier;
            player.healthRegenBuff = newHeart.healthRegenModifier;
            player.manaBuff = newHeart.manaModifier;
            player.manaRegenBuff = newHeart.manaRegenModifier;
            player.moveSpeedBuff = newHeart.moveSpeedModifier;
            player.jumpPowerBuff = newHeart.jumpPowerModifier;
            player.wallJumpPowerBuff = newHeart.wallJumpPowerModifier;
            player.dashPowerBuff = newHeart.dashPowerModifier;
        }
    }

    private void RemoveHeartBuffs()
    {
        if (player != null)
        {
            player.damageBuff = 0f;
            player.defenseBuff = 0f;
            player.healthBuff = 0f;
            player.healthRegenBuff = 0f;
            player.manaBuff = 0f;
            player.manaRegenBuff = 0f;
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
            player.prayerManaCost = newPrayer.manaCost;
        }
    }

    private void RemovePrayer()
    {
        if (player != null)
        {
            player.prayerManaCost = 0f;
        }
    }

    #region Prayers functions

    #region Verdiales of The Forsaken Hamlet

    [Header("Verdiales of The Forsaken Hamlet")]
    public GameObject verdialesProjectilePrefab;
    public Transform spawnVerdialesPoint;

    private void SpawnVerdialesProjectiles()
    {
        bool isFacingRight = player.IsFacingRight;

        Quaternion spawnRotation = isFacingRight ? spawnVerdialesPoint.rotation : Quaternion.Euler(spawnVerdialesPoint.rotation.eulerAngles + new Vector3(0, 180, 0));

        GameObject forwardProjectile = Instantiate(verdialesProjectilePrefab, spawnVerdialesPoint.position, spawnRotation);
        VerdialesProjectileMovement forwardMovement = forwardProjectile.GetComponent<VerdialesProjectileMovement>();
        forwardMovement.moveForward = true;

        GameObject backwardProjectile = Instantiate(verdialesProjectilePrefab, spawnVerdialesPoint.position, spawnRotation);
        VerdialesProjectileMovement backwardMovement = backwardProjectile.GetComponent<VerdialesProjectileMovement>();
        backwardMovement.moveForward = false;
    }

    #endregion

    #endregion
}
