using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Item;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated = false;
    public ItemSlot[] questItemSlots;
    public ItemSlot[] meaCulpaHeartSlots;
    public ItemSlot[] prayerSlots;
    public QuestItemsInventorySO questItemsInventorySO;
    public MeaCulpaHeartsInventorySO meaCulpaHeartsInventorySO;
    public PrayersInventorySO prayersInventorySO;

    void Start()
    {
        LoadInventories();
    }

    void Update()
    {

    }

    public void OnOpenInventoryMenu(InputAction.CallbackContext context)
    {
        if (context.started && !menuActivated)
        {
            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
        else if (context.started && menuActivated)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            menuActivated = false;
        }
    }

    public void AddItem(InventoryCategory category, string itemName, string itemDescription, Sprite itemSprite)
    {
        switch (category)
        {
            case InventoryCategory.QuestItems:
                AddItemToInventory(questItemSlots, questItemsInventorySO.items, itemName, itemDescription, itemSprite);
                break;
            case InventoryCategory.MeaCulpaHearts:
                AddItemToInventory(meaCulpaHeartSlots, meaCulpaHeartsInventorySO.items, itemName, itemDescription, itemSprite);
                break;
            case InventoryCategory.Prayers:
                AddItemToInventory(prayerSlots, prayersInventorySO.items, itemName, itemDescription, itemSprite);
                break;
        }
    }

    private void AddItemToInventory<T>(ItemSlot[] slots, List<T> inventory, string itemName, string itemDescription, Sprite itemSprite) where T : InventoryItem, new()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].isFill == false)
            {
                slots[i].AddItem(itemName, itemDescription, itemSprite);
                inventory.Add(new T
                {
                    itemName = itemName,
                    itemDescription = itemDescription,
                    itemSprite = itemSprite
                });
                SaveInventories();
                return;
            }
        }
    }

    public void DeselectAllSlots(ItemSlot[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].selectedShader.SetActive(false);
            slots[i].isSelected = false;
        }
    }

    private void SaveInventories()
    {
        questItemsInventorySO.items.Clear();
        foreach (var slot in questItemSlots)
        {
            if (slot.isFill)
            {
                questItemsInventorySO.items.Add(new QuestItems
                {
                    itemName = slot.itemName,
                    itemDescription = slot.itemDescription,
                    itemSprite = slot.itemSprite
                });
            }
        }

        meaCulpaHeartsInventorySO.items.Clear();
        foreach (var slot in meaCulpaHeartSlots)
        {
            if (slot.isFill)
            {
                meaCulpaHeartsInventorySO.items.Add(new MeaCulpaHearts
                {
                    itemName = slot.itemName,
                    itemDescription = slot.itemDescription,
                    itemSprite = slot.itemSprite
                });
            }
        }

        prayersInventorySO.items.Clear();
        foreach (var slot in prayerSlots)
        {
            if (slot.isFill)
            {
                prayersInventorySO.items.Add(new Prayers
                {
                    itemName = slot.itemName,
                    itemDescription = slot.itemDescription,
                    itemSprite = slot.itemSprite
                });
            }
        }
    }

    private void LoadInventories()
    {
        LoadInventory(questItemSlots, questItemsInventorySO.items);
        LoadInventory(meaCulpaHeartSlots, meaCulpaHeartsInventorySO.items);
        LoadInventory(prayerSlots, prayersInventorySO.items);
    }

    private void LoadInventory<T>(ItemSlot[] slots, List<T> inventory) where T : InventoryItem
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.Count)
            {
                var item = inventory[i];
                slots[i].AddItem(item.itemName, item.itemDescription, item.itemSprite);
            }
            else
            {
                slots[i].ClearItem();
            }
        }
    }
}
