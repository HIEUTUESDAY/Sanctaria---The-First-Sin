using static Item;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance {  get; private set; }
    public GameObject[] Inventories;
    public int currentInventoryIndex = 0;
    public ItemSlot[] questItemSlots;
    public ItemSlot[] meaCulpaHeartSlots;
    public ItemSlot[] prayerSlots;
    public QuestItemsInventorySO questItemsInventorySO;
    public MeaCulpaHeartsInventorySO meaCulpaHeartsInventorySO;
    public PrayersInventorySO prayersInventorySO;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        LoadInventories();
        UpdateActiveInventoryUI();
    }

    void Update()
    {
        if (UIManager.Instance.menuActivated)
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                PreviousInventory();
            }
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                NextInventory();
            }
        }
    }

    public void PreviousInventory()
    {
        Inventories[currentInventoryIndex].SetActive(false);
        currentInventoryIndex = (currentInventoryIndex - 1 + Inventories.Length) % Inventories.Length;
        UpdateActiveInventoryUI();
    }

    public void NextInventory()
    {
        Inventories[currentInventoryIndex].SetActive(false);
        currentInventoryIndex = (currentInventoryIndex + 1) % Inventories.Length;
        UpdateActiveInventoryUI();
    }

    private void UpdateActiveInventoryUI()
    {
        for (int i = 0; i < Inventories.Length; i++)
        {
            Inventories[i].SetActive(i == currentInventoryIndex);
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

    public void DeselectSlots(ItemSlot[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].isSelected)
            {
                slots[i].isSelected = false;
            }
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
