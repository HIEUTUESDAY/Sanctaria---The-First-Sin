using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; set; }
    public PlayerEquipment playerEquipment;
    public GameObject[] Inventories;
    public int currentInventoryIndex = 0;

    public QuestItemSlot[] questItemSlots;
    public MeaCulpaHeartSlot[] meaCulpaHeartSlots;
    public PrayerSlot[] prayerSlots;

    public List<QuestItem> questItemsInventory;
    public List<MeaCulpaHeart> meaCulpaHeartsInventory;
    public List<Prayer> prayersInventory;

    public MeaCulpaHeart meaCulpaHeartsEquipment;
    public Prayer prayersEquipment;

    public int tearsOfAtonement;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateActiveInventoryUI();
    }

    private void Update()
    {
        if (UIManager.Instance.menuActivated)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PreviousInventory();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                NextInventory();
            }
        }
    }

    #region Change Inventories

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

    #endregion

    #region Load inventories data 

    public void LoadInventoriesData(GameData gameData)
    {
        tearsOfAtonement = gameData.playerInventoryData.tearsOfAtonement;

        questItemsInventory = gameData.playerInventoryData.questItemsInventory;
        meaCulpaHeartsInventory = gameData.playerInventoryData.meaCulpaHeartsInventory;
        prayersInventory = gameData.playerInventoryData.prayersInventory;

        meaCulpaHeartsEquipment = gameData.playerInventoryData.meaCulpaHeartEquipment;
        prayersEquipment = gameData.playerInventoryData.prayerEquipment;

        gameData.playerInventoryData.LoadSprites();

        LoadInventories();
    }

    private void LoadInventories()
    {
        LoadQuestItemInventory(questItemSlots, questItemsInventory);
        LoadMeaCulpaHeartInventory(meaCulpaHeartSlots, meaCulpaHeartsInventory);
        LoadPrayerInventory(prayerSlots, prayersInventory);
    }

    private void LoadQuestItemInventory(QuestItemSlot[] slots, List<QuestItem> questItemsInventory)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < questItemsInventory.Count)
            {
                var item = questItemsInventory[i];
                slots[i].AddQuestItemSlot(item);
            }
            else
            {
                slots[i].ClearQuestItemSlot();
            }
        }
    }

    private void LoadMeaCulpaHeartInventory(MeaCulpaHeartSlot[] slots, List<MeaCulpaHeart> meaCulpaHeartsInventory)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < meaCulpaHeartsInventory.Count)
            {
                var item = meaCulpaHeartsInventory[i];
                slots[i].AddMeaCulpaHeartSlot(item);
            }
            else
            {
                slots[i].ClearMeaCulpaHeartSlot();
            }
        }
    }

    private void LoadPrayerInventory(PrayerSlot[] slots, List<Prayer> prayersInventory)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < prayersInventory.Count)
            {
                var item = prayersInventory[i];
                slots[i].AddPrayerSlot(item);
            }
            else
            {
                slots[i].ClearPrayerSlot();
            }
        }
    }

    #endregion

    #region Save inventories data

    private void SaveInventories()
    {
        questItemsInventory.Clear();
        foreach (var slot in questItemSlots)
        {
            if (slot.hasItem)
            {
                questItemsInventory.Add(new QuestItem
                {
                    itemName = slot.itemName,
                    itemDescription = slot.itemDescription,
                    itemSprite = slot.itemSprite
                });
            }
        }

        meaCulpaHeartsInventory.Clear();
        foreach (var slot in meaCulpaHeartSlots)
        {
            if (slot.hasHeart)
            {
                meaCulpaHeartsInventory.Add(new MeaCulpaHeart
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

        prayersInventory.Clear();
        foreach (var slot in prayerSlots)
        {
            if (slot.hasPrayer)
            {
                prayersInventory.Add(new Prayer
                {
                    itemName = slot.prayerName,
                    itemDescription = slot.prayerDescription,
                    itemSprite = slot.prayerSprite,
                    isItemEquipped = slot.isPrayerEquipped
                });
            }
        }
    }

    #endregion

    #region Add items to inventories

    public void AddQuestItemToInventory(QuestItem questItem)
    {
        for (int i = 0; i < questItemSlots.Length; i++)
        {
            if (!questItemSlots[i].hasItem)
            {
                questItemSlots[i].AddQuestItemSlot(questItem);
                questItemsInventory.Add(questItem);
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
                meaCulpaHeartsInventory.Add(meaCulpaHeart);
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
                prayersInventory.Add(prayer);
                return;
            }
        }
    }

    #endregion

    #region Equip items

    public void EquipNewMeaCulpaHeart(MeaCulpaHeart meaCulpaHeart)
    {
        foreach (var slot in meaCulpaHeartSlots)
        {
            if (!slot.isSelected && slot.isHeartEquipped)
            {
                slot.isHeartEquipped = false;
            }
        }
        meaCulpaHeartsEquipment = meaCulpaHeart;
        playerEquipment.UpdateEquippedMeaCulpaHeart();
        SaveInventories();
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
        prayersEquipment = prayer;
        playerEquipment.UpdateEquippedPrayer();
        SaveInventories();

    }

    public void UnequipMeaCulpaHeart()
    {
        meaCulpaHeartsEquipment = null;
        playerEquipment.UpdateEquippedMeaCulpaHeart();
        SaveInventories();

    }

    public void UnequipPrayer()
    {
        prayersEquipment = null;
        playerEquipment.UpdateEquippedPrayer();
        SaveInventories();

    }

    #endregion

    #region Get inventories

    public List<QuestItem> GetQuestItemsInventory()
    {
        return questItemsInventory;
    }

    public List<MeaCulpaHeart> GetMeaCulpaHeartsInventory()
    {
        return meaCulpaHeartsInventory;
    }
    public List<Prayer> GetPrayersInventory()
    {
        return prayersInventory;
    }

    public MeaCulpaHeart GetMeaCulpaHeartEquipment()
    {
        return meaCulpaHeartsEquipment;
    }

    public Prayer GetPrayerEquipment()
    {
        return prayersEquipment;
    }

    public int GetTearsAmount()
    {
        return tearsOfAtonement;
    }

    #endregion
}
