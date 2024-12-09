using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public PlayerEquipment playerEquipment;
    public GameObject[] Inventories;
    public int currentInventoryIndex = 0;

    public QuestItemSlot[] questItemSlots;
    public HeartSlot[] heartSlots;
    public PrayerSlot[] prayerSlots;

    public List<QuestItem> questItemsInventory;
    public List<Heart> heartsInventory;
    public List<Prayer> prayersInventory;

    public Heart heartsEquipment;
    public Prayer prayersEquipment;

    public float relicPoint;

    public GameObject heartUnlockHint;

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
        if (UIManager.Instance.menuActivated && UIManager.Instance.inventoryMenu.activeSelf)
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

        if (!Player.Instance.isKneelInCheckpoint)
        {
            heartUnlockHint.SetActive(true);
        }
        else
        {
            heartUnlockHint.SetActive(false);
        }
    }

    #region Change Inventories

    public void PreviousInventory()
    {
        Inventories[currentInventoryIndex].SetActive(false);
        currentInventoryIndex = (currentInventoryIndex - 1 + Inventories.Length) % Inventories.Length;
        UpdateActiveInventoryUI();
        SoundFXManager.Instance.PlayChangeTabSound();
    }

    public void NextInventory()
    {
        Inventories[currentInventoryIndex].SetActive(false);
        currentInventoryIndex = (currentInventoryIndex + 1) % Inventories.Length;
        UpdateActiveInventoryUI();
        SoundFXManager.Instance.PlayChangeTabSound();
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

    public void LoadSaveFileInventoriesData(GameData gameData)
    {
        relicPoint = gameData.playerInventoryData.tearsOfAtonement;

        questItemsInventory = gameData.playerInventoryData.questItemsInventory;
        heartsInventory = gameData.playerInventoryData.meaCulpaHeartsInventory;
        prayersInventory = gameData.playerInventoryData.prayersInventory;

        heartsEquipment = gameData.playerInventoryData.meaCulpaHeartEquipment;
        prayersEquipment = gameData.playerInventoryData.prayerEquipment;

        gameData.playerInventoryData.LoadSprites();

        LoadInventories();
    }

    private void LoadInventories()
    {
        LoadQuestItemInventory(questItemSlots, questItemsInventory);
        LoadHeartInventory(heartSlots, heartsInventory);
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

    private void LoadHeartInventory(HeartSlot[] slots, List<Heart> meaCulpaHeartsInventory)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < meaCulpaHeartsInventory.Count)
            {
                var item = meaCulpaHeartsInventory[i];
                slots[i].AddHeartSlot(item);
            }
            else
            {
                slots[i].ClearHeartSlot();
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

        heartsInventory.Clear();
        foreach (var slot in heartSlots)
        {
            if (slot.hasHeart)
            {
                heartsInventory.Add(new Heart
                {
                    itemName = slot.heartName,
                    itemDescription = slot.heartDescription,
                    itemSprite = slot.heartSprite,
                    isItemEquipped = slot.isHeartEquipped,
                    damageModifier = slot.heartDamageModifier,
                    defenseModifier = slot.heartDefenseModifier,
                    healthModifier = slot.heartHealthModifier,
                    healthRegenModifier = slot.heartHealthRegenModifier,
                    manaModifier = slot.heartStaminaModifier,
                    manaRegenModifier = slot.heartStaminaRegenModifier,
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
                    manaCost = slot.prayerManaCost,
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

    public void AddHeartToInventory(Heart heart)
    {
        if (heartsInventory.Count == 0 && !TutorialManager.Instance.heartTutor)
        {
            Time.timeScale = 0;
            UIManager.Instance.menuActivated = true;
            UIManager.Instance.heartTutorHUD.SetActive(true);
            TutorialManager.Instance.heartTutor = true;
        }

        for (int i = 0; i < heartSlots.Length; i++)
        {
            if (!heartSlots[i].hasHeart)
            {
                heartSlots[i].AddHeartSlot(heart);
                heartsInventory.Add(heart);
                return;
            }
        }
    }

    public void AddPrayerToInventory(Prayer prayer)
    {
        if (prayersInventory.Count == 0 && !TutorialManager.Instance.prayerTutor)
        {
            Time.timeScale = 0;
            UIManager.Instance.menuActivated = true;
            UIManager.Instance.prayerTutorHUD.SetActive(true);
            TutorialManager.Instance.prayerTutor = true;
        }

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

    public void EquipNewHeart(Heart heart)
    {
        foreach (var slot in heartSlots)
        {
            if (!slot.isSelected && slot.isHeartEquipped)
            {
                slot.isHeartEquipped = false;
            }
        }
        heartsEquipment = heart;
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
        SaveInventories();
    }

    public void UnequipHeart()
    {
        heartsEquipment = null;
        SaveInventories();
    }

    public void UnequipPrayer()
    {
        prayersEquipment = null;
        SaveInventories();
    }

    #endregion

    #region Get inventories

    public List<QuestItem> GetQuestItemsInventory()
    {
        return questItemsInventory;
    }

    public List<Heart> GetHeartsInventory()
    {
        return heartsInventory;
    }

    public List<Prayer> GetPrayersInventory()
    {
        return prayersInventory;
    }

    public Heart GetHeartEquipment()
    {
        return heartsEquipment;
    }

    public Prayer GetPrayerEquipment()
    {
        return prayersEquipment;
    }

    public float GetTearsAmount()
    {
        return relicPoint;
    }

    public string GetInventoryItemByName(string itemName)
    {
        var questItem = questItemsInventory.Find(item => item.itemName == itemName);
        if (questItem != null)
        {
            return questItem.itemName;
        }

        var heart = heartsInventory.Find(item => item.itemName == itemName);
        if (heart != null)
        {
            return heart.itemName;
        }

        var prayer = prayersInventory.Find(item => item.itemName == itemName);
        if (prayer != null)
        {
            return prayer.itemName;
        }

        // if not found item
        return "Item not found";
    }

    #endregion
}
