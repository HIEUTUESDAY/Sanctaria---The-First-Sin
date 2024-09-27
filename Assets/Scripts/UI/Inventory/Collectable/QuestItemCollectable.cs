using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class QuestItemCollectable : ItemCollectable
{
    
    public override void CollectItem()
    {
        if (canBeCollect)
        {
            InventoryManager.Instance.AddQuestItemToInventory(new QuestItem
            {
                itemName = itemName,
                itemDescription = itemDescription,
                itemSprite = itemSprite
            });
            gameObject.SetActive(false);

            CharacterEvent.collectMessage.Invoke(itemSprite, itemName);
        }
    }
}
