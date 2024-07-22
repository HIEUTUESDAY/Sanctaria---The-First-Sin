using static QuestItemCollectable;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; set; }
    public PlayerEquipment playerEquipment;
    public GameObject[] Inventories;
    public int currentInventoryIndex = 0;

    public QuestItemSlot[] questItemSlots;
    public MeaCulpaHeartSlot[] meaCulpaHeartSlots;
    public PrayerSlot[] prayerSlots;

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
        LoadInventories();

    }

    private void Start()
    {
        UpdateActiveInventoryUI();
    }

    private void Update()
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

        SaveInventories();
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
                return;
            }
        }
    }

    public void AddMeaCulpaHeartToInventory(MeaCulpaHeart meaCulpaHeart)
    {
        for (int i = 0; i < meaCulpaHeartSlots.Length; i++)
        {
            if (!meaCulpaHeartSlots[i].hasHeart)
            {
                meaCulpaHeartSlots[i].AddMeaCulpaHeartSlot(meaCulpaHeart);
                meaCulpaHeartsInventorySO.MeaCulpaHearts.Add(meaCulpaHeart);
                return;
            }
        }
    }

    public void AddPrayerToInventory(Prayer prayer)
    {
        for (int i = 0; i < prayerSlots.Length; i++)
        {
            if (!prayerSlots[i].hasPrayer)
            {
                prayerSlots[i].AddPrayerSlot(prayer);
                prayersInventorySO.Prayers.Add(prayer);
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
            if (slot.hasHeart)
            {
                meaCulpaHeartsInventorySO.MeaCulpaHearts.Add(new MeaCulpaHeart
                {
                    itemName = slot.heartName,
                    itemDescription = slot.heartDescription,
                    itemSprite = slot.heartSprite,
                    isItemEquipped = slot.isHeartEquipped,
                    damageModifier = slot.heartDamageModifier,
                    defenseModifier = slot.heartDefenseModifier,
                    healthModifier = slot.heartHealthModifier,
                    healthRegenModifier = slot.heartHealthRegenModifier,
                    staminaModifier = slot.heartStaminaModifier,
                    staminaRegenModifier = slot.heartStaminaRegenModifier,
                    moveSpeedModifier = slot.heartMoveSpeedModifier,
                    jumpPowerModifier = slot.heartJumpPowerModifier,
                    wallJumpPowerModifier = slot.heartWallJumpPowerModifier,
                    dashPowerModifier = slot.heartDashPowerModifier,
                });
            }
        }

        prayersInventorySO.Prayers.Clear();
        foreach (var slot in prayerSlots)
        {
            if (slot.hasPrayer)
            {
                prayersInventorySO.Prayers.Add(new Prayer
                {
                    itemName = slot.prayerName,
                    itemDescription = slot.prayerDescription,
                    itemSprite = slot.prayerSprite,
                    isItemEquipped = slot.isPrayerEquipped
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

    public void EquipNewMeaCulpaHeart(MeaCulpaHeart meaCulpaHeart)
    {
        foreach (var slot in meaCulpaHeartSlots)
        {
            if (!slot.isSelected && slot.isHeartEquipped)
            {
                slot.isHeartEquipped = false;
            }
        }
        meaCulpaHeartsEquipmentSO.equippedMeaCulpaHeart = meaCulpaHeart;
        playerEquipment.UpdateEquippedMeaCulpaHeart();
    }

    public void EquipNewPrayer(Prayer prayer)
    {
        foreach (var slot in prayerSlots)
        {
            if (!slot.isSelected && slot.isPrayerEquipped)
            {
                slot.isPrayerEquipped = false;
            }
        }
        prayersEquipmentSO.equippedPrayer = prayer;
        playerEquipment.UpdateEquippedPrayer();
    }

    public void UnequipMeaCulpaHeart()
    {
        meaCulpaHeartsEquipmentSO.equippedMeaCulpaHeart = null;
        playerEquipment.UpdateEquippedMeaCulpaHeart();
    }

    public void UnequipPrayer()
    {
        prayersEquipmentSO.equippedPrayer = null;
        playerEquipment.UpdateEquippedPrayer();
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
