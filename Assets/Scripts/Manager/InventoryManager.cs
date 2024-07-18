using static QuestItemCollectable;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public PlayerEquipment playerEquipment;
    public GameObject[] Inventories;
    public int currentInventoryIndex = 0;

    public QuestItemSlot[] questItemSlots;
    public MeaCulpaHeartSlot[] meaCulpaHeartSlots;
    public PrayerSlot[] prayerSlots;

    public MeaCulpaHeartEquipment meaCulpaHeartsEquipment;
    public PrayerEquipment prayersEquipment;

    public QuestItemInventorySO questItemsInventorySO;
    public MeaCulpaHeartInventorySO meaCulpaHeartsInventorySO;
    public PrayerInventorySO prayersInventorySO;

    public MeaCulpaHeartEquipmentSO meaCulpaHeartsEquipmentSO;
    public PrayerEquipmentSO prayersEquipmentSO;

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
        LoadEquipment();
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

    public void AddQuestItemToInventory(QuestItem questItem)
    {
        for (int i = 0; i < questItemSlots.Length; i++)
        {
            if (!questItemSlots[i].hasItem)
            {
                questItemSlots[i].AddQuestItemSlot(questItem);
                questItemsInventorySO.QuestItems.Add(questItem);
                SaveInventories();
                return;
            }
        }
    }

    public void AddMeaCulpaHeartToInventory(MeaCulpaHeart meaCulpaHeart)
    {
        for (int i = 0; i < meaCulpaHeartSlots.Length; i++)
        {
            if (!meaCulpaHeartSlots[i].hasItem)
            {
                meaCulpaHeartSlots[i].AddMeaCulpaHeartSlot(meaCulpaHeart);
                meaCulpaHeartsInventorySO.MeaCulpaHearts.Add(meaCulpaHeart);
                SaveInventories();
                return;
            }
        }
    }

    public void AddPrayerToInventory(Prayer prayer)
    {
        for (int i = 0; i < prayerSlots.Length; i++)
        {
            if (!prayerSlots[i].hasItem)
            {
                prayerSlots[i].AddPrayerSlot(prayer);
                prayersInventorySO.Prayers.Add(prayer);
                SaveInventories();
                return;
            }
        }
    }

    private void SaveInventories()
    {
        questItemsInventorySO.QuestItems.Clear();
        foreach (var slot in questItemSlots)
        {
            if (slot.hasItem)
            {
                questItemsInventorySO.QuestItems.Add(new QuestItem
                {
                    itemName = slot.itemName,
                    itemDescription = slot.itemDescription,
                    itemSprite = slot.itemSprite
                });
            }
        }

        meaCulpaHeartsInventorySO.MeaCulpaHearts.Clear();
        foreach (var slot in meaCulpaHeartSlots)
        {
            if (slot.hasItem)
            {
                meaCulpaHeartsInventorySO.MeaCulpaHearts.Add(new MeaCulpaHeart
                {
                    itemName = slot.itemName,
                    itemDescription = slot.itemDescription,
                    itemSprite = slot.itemSprite,
                    isEquip = slot.isItemEquipped
                });
            }
        }

        prayersInventorySO.Prayers.Clear();
        foreach (var slot in prayerSlots)
        {
            if (slot.hasItem)
            {
                prayersInventorySO.Prayers.Add(new Prayer
                {
                    itemName = slot.itemName,
                    itemDescription = slot.itemDescription,
                    itemSprite = slot.itemSprite,
                    isEquip = slot.isItemEquipped
                });
            }
        }
    }

    private void LoadInventories()
    {
        LoadQuestItemInventory(questItemSlots, questItemsInventorySO.QuestItems);
        LoadMeaCulpaHeartInventory(meaCulpaHeartSlots, meaCulpaHeartsInventorySO.MeaCulpaHearts);
        LoadPrayerInventory(prayerSlots, prayersInventorySO.Prayers);
    }

    private void LoadQuestItemInventory(QuestItemSlot[] slots, List<QuestItem> inventory)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.Count)
            {
                var item = inventory[i];
                slots[i].AddQuestItemSlot(item);
            }
            else
            {
                slots[i].ClearQuestItemSlot();
            }
        }
    }

    private void LoadMeaCulpaHeartInventory(MeaCulpaHeartSlot[] slots, List<MeaCulpaHeart> inventory)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.Count)
            {
                var item = inventory[i];
                slots[i].AddMeaCulpaHeartSlot(item);
            }
            else
            {
                slots[i].ClearMeaCulpaHeartSlot();
            }
        }
    }

    private void LoadPrayerInventory(PrayerSlot[] slots, List<Prayer> inventory)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.Count)
            {
                var item = inventory[i];
                slots[i].AddPrayerSlot(item);
            }
            else
            {
                slots[i].ClearPrayerSlot();
            }
        }
    }

    public void EquipMeaCulpaHeart(MeaCulpaHeart meaCulpaHeart)
    {
        meaCulpaHeartsEquipment.EquipMeaCulpaHeart(meaCulpaHeart);
        meaCulpaHeartsEquipmentSO.equippedMeaCulpaHeart = meaCulpaHeart;
        playerEquipment.UpdateEquippedItems();
    }

    public void EquipPrayer(Prayer prayer)
    {
        prayersEquipment.EquipPrayer(prayer);
        prayersEquipmentSO.equippedPrayer = prayer;
    }

    public void UnequipMeaCulpaHearts()
    {
        foreach (var slot in meaCulpaHeartSlots)
        {
            if (slot.isItemEquipped)
            {
                slot.isItemEquipped = false;
            }
        }
        meaCulpaHeartsEquipment.UnequipMeaCulpaHeart();
        meaCulpaHeartsEquipmentSO.equippedMeaCulpaHeart = null;
        playerEquipment.UpdateEquippedItems();
    }

    public void UnequipPrayers()
    {
        foreach (var slot in prayerSlots)
        {
            if (slot.isItemEquipped)
            {
                slot.isItemEquipped = false;
            }
        }
        prayersEquipment.UnequipPrayer();
        prayersEquipmentSO.equippedPrayer = null;
    }

    private void LoadEquipment()
    {
        if (meaCulpaHeartsEquipmentSO.equippedMeaCulpaHeart != null)
        {
            meaCulpaHeartsEquipment.EquipMeaCulpaHeart(
                meaCulpaHeartsEquipmentSO.equippedMeaCulpaHeart
            );
        }

        if (prayersEquipmentSO.equippedPrayer != null)
        {
            prayersEquipment.EquipPrayer(
                prayersEquipmentSO.equippedPrayer
            );
        }
    }

    public MeaCulpaHeart GetEquippedMeaCulpaHeart()
    {
        return meaCulpaHeartsEquipmentSO.equippedMeaCulpaHeart;
    }

    public Prayer GetEquippedPrayer()
    {
        return prayersEquipmentSO.equippedPrayer;
    }
}
