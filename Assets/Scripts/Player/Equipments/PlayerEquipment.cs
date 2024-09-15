using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private Player player;
    public MeaCulpaHeart equippedMeaCulpaHeart;
    public Prayer equippedPrayer;

    [SerializeField] private float prayerCooldownTime = 5f;
    public float prayerCooldown = 0f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        SetCurrentEquipment();
        UpdatePrayerCooldown();
    }

    private void SetCurrentEquipment()
    {
        equippedMeaCulpaHeart = InventoryManager.Instance.GetMeaCulpaHeartEquipment();
        AddMeaCulpaHeartBuffs(equippedMeaCulpaHeart);

        equippedPrayer = InventoryManager.Instance.GetPrayerEquipment();
        AddPrayer(equippedPrayer);
    }

    private void UpdatePrayerCooldown()
    {
        if (prayerCooldown > 0)
        {
            prayerCooldown -= Time.deltaTime;
        }
    }

    public void PerformPrayer()
    {
        if (prayerCooldown > 0 || equippedPrayer.itemName.Equals(null))
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

        prayerCooldown = prayerCooldownTime;
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
            player.manaBuff = newEquippedMeaCulpaHeart.manaModifier;
            player.manaRegenBuff = newEquippedMeaCulpaHeart.manaRegenModifier;
            player.moveSpeedBuff = newEquippedMeaCulpaHeart.moveSpeedModifier;
            player.jumpPowerBuff = newEquippedMeaCulpaHeart.jumpPowerModifier;
            player.wallJumpPowerBuff = newEquippedMeaCulpaHeart.wallJumpPowerModifier;
            player.dashPowerBuff = newEquippedMeaCulpaHeart.dashPowerModifier;
        }
    }

    private void RemoveMeaCulpaHeartBuffs()
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
