using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeaCulpaHeartCollectable : Collectable
{

    public override void CollectItem()
    {
        base.CollectItem();

        if (canBeCollect)
        {
            InventoryManager.Instance.AddMeaCulpaHeartToInventory(new MeaCulpaHeart
            {
                itemName = itemName,
                itemDescription = itemDescription,
                itemSprite = itemSprite
            });
            Destroy(gameObject);
        }
    }
}
