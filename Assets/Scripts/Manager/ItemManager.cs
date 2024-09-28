using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("This scene DATA")]
    [SerializeField] private SceneData thisSceneData;
    [Space(5)]

    [Header("Drop Enemy in scene into this ITEM OBJECTS LIST")]
    [SerializeField] private List<GameObject> itemsObjects;

    // Save the current state of items in the scene
    public List<ItemData> SaveItems()
    {
        List<ItemData> itemDataList = new List<ItemData>();

        foreach (GameObject itemObject in itemsObjects)
        {
            if (itemObject != null)
            {
                bool isAcquired = false;
                ItemCollectable itemCollectable = itemObject.GetComponent<ItemCollectable>();
                ItemBuyable itemBuyable = itemObject.GetComponent<ItemBuyable>();

                if (itemCollectable != null)
                {
                    if(itemCollectable.itemName == InventoryManager.Instance.GetInventoryItemByName(itemCollectable.itemName))
                    {
                        isAcquired = true;
                    }

                    ItemData itemData = new ItemData
                    {
                        itemName = itemCollectable.itemName,
                        isAcquired = isAcquired,
                        position = new float[] { itemObject.transform.position.x, itemObject.transform.position.y, itemObject.transform.position.z }
                    };

                    itemDataList.Add(itemData);
                }
                else if (itemBuyable != null)
                {
                    if(itemBuyable.itemName == InventoryManager.Instance.GetInventoryItemByName(itemBuyable.itemName))
                    {
                        isAcquired = true;
                    }

                    ItemData itemData = new ItemData
                    {
                        itemName = itemBuyable.itemName,
                        isAcquired = isAcquired,
                        position = new float[] { itemObject.transform.position.x, itemObject.transform.position.y, itemObject.transform.position.z }
                    };

                    itemDataList.Add(itemData);
                }
            }
        }

        return itemDataList;
    }

    // Load the items in the scene based on the saved data
    public void LoadItems(string currentScene)
    {
        List<SceneData> sceneDataList = SceneDataManager.Instance.sceneDataList;

        foreach (SceneData sceneData in sceneDataList)
        {
            if (currentScene.Equals(sceneData.sceneName))
            {
                thisSceneData = sceneData;
                SetActiveItems();
                return;
            }
        }
    }

    private void SetActiveItems()
    {
        if (thisSceneData != null)
        {
            foreach (ItemData itemData in thisSceneData.items)
            {
                // Find the corresponding item prefab by name
                GameObject item = GameObject.Find(itemData.itemName);

                if (item != null)
                {
                    if (itemData.isAcquired)
                    {
                        item.SetActive(false);
                    }
                    else
                    {
                        item.SetActive(true);
                    }

                }
            }
        }
    }
}
