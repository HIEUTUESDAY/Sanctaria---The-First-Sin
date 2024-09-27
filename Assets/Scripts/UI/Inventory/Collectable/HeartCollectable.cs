using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCollectable : ItemCollectable
{
    private bool isItemEquipped = false;
    public float damageModifier;
    public float defenseModifier;
    public float healthModifier;
    public float healthRegenModifier;
    public float staminaModifier;
    public float staminaRegenModifier;
    public float moveSpeedModifier;
    public float jumpPowerModifier;
    public float wallJumpPowerModifier;
    public float dashPowerModifier;

    public override void CollectItem()
    {
        if (canBeCollect)
        {
            InventoryManager.Instance.AddHeartToInventory(new Heart
            {
                itemName = itemName,
                itemDescription = itemDescription,
                itemSprite = itemSprite,
                isItemEquipped = isItemEquipped,
                damageModifier = damageModifier,
                defenseModifier = defenseModifier,
                healthModifier = healthModifier,
                healthRegenModifier = healthRegenModifier,
                manaModifier = staminaModifier,
                manaRegenModifier = staminaRegenModifier,
                moveSpeedModifier = moveSpeedModifier,
                jumpPowerModifier = jumpPowerModifier,
                wallJumpPowerModifier = wallJumpPowerModifier,
                dashPowerModifier = dashPowerModifier
            });
            gameObject.SetActive(false);

            CharacterEvent.collectMessage.Invoke(itemSprite, itemName);
        }
    }
}
