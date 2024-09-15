using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrayerCollectable : ItemCollectable
{
    private bool isItemEquipped = false;
    public float manaCost;

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
                isItemEquipped = isItemEquipped,
                manaCost = manaCost
            });
            gameObject.SetActive(false);

            CharacterEvent.collectMessage.Invoke(itemSprite, itemName);
        }
    }
}
