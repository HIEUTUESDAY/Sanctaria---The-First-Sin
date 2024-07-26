using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class QuestItemCollectable : Collectable
{
    
    public override void CollectItem()
    {
        base.CollectItem();

        if (canBeCollect)
        {
            InventoryManager.Instance.AddQuestItemToInventory(new QuestItem
            {
                itemName = itemName,
                itemDescription = itemDescription,
                itemSprite = itemSprite
            }); 
            Destroy(gameObject);

            CharacterEvent.collectMessage.Invoke(itemSprite, itemName);
        }
    }
}
