using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrayerCollectable : Collectable
{
    private bool isItemEquipped = false;

    public override void CollectItem()
    {
        base.CollectItem();

        if (canBeCollect)
        {
            InventoryManager.Instance.AddPrayerToInventory(new Prayer
            {
                itemName = itemName,
                itemDescription = itemDescription,
                itemSprite = itemSprite,
                isItemEquipped = isItemEquipped
            });
            Destroy(gameObject);

            CharacterEvent.collectMessage.Invoke(itemSprite, itemName);
        }
    }
}
