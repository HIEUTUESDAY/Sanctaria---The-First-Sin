using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

public class PrayerSlot : MonoBehaviour
{
    [Header("Prayer data")]
    public string itemName;
    public Sprite itemSprite;
    public string itemDescription;
    public bool isItemEquipped;
    public bool hasItem;
    [Space(5)]

    [Header("Prayer Slot")]
    [SerializeField] private Image itemSlotImage;
    [SerializeField] private Sprite emptyItemSlotBGI;
    [SerializeField] private Sprite fillItemSlotBGI;
    [SerializeField] private Sprite selectedItemSlotBGI;
    [SerializeField] private Sprite equippedItemSlotBGI;
    [SerializeField] private Image itemImage;
    [SerializeField] private Sprite emptyItemImage;
    [Space(5)]

    [Header("Prayer Description")]
    public Image itemDesImage;
    public TMP_Text itemDesNameText;
    public TMP_Text itemDesText;
    [Space(5)]

    public Button button;
    public bool isSelected;
    public GameObject shaderAnimation;

    public PrayerEquipment prayerEquipment;

    private void Update()
    {
        PrayerSlotSelect();
        UpdatePrayerSlot();
        EquipPrayerSlot();
        UnequipPrayerSlot();
    }

    public void AddPrayerSlot(Prayer prayer)
    {
        this.itemName = prayer.itemName;
        this.itemDescription = prayer.itemDescription;
        this.itemSprite = prayer.itemSprite;
        this.isItemEquipped = prayer.isEquip;
        hasItem = true;

        itemImage.sprite = itemSprite;
    }

    public void ClearPrayerSlot()
    {
        this.itemName = "";
        this.itemDescription = "";
        this.itemSprite = null;
        this.isItemEquipped = false;
        hasItem = false;

        itemImage.sprite = emptyItemImage;
    }

    private void UpdatePrayerSlot()
    {
        if (!hasItem)
        {
            itemSlotImage.sprite = emptyItemSlotBGI;
        }

        if (hasItem && !isSelected)
        {
            itemSlotImage.sprite = fillItemSlotBGI;
        }
        
        if (hasItem && isSelected)
        {
            itemSlotImage.sprite = selectedItemSlotBGI;
        }

        if (hasItem && isItemEquipped)
        {
            itemSlotImage.sprite = equippedItemSlotBGI;
        }
    }

    private void PrayerSlotSelect()
    {
        if (EventSystem.current.currentSelectedGameObject == button.gameObject)
        {
            isSelected = true;
            shaderAnimation.SetActive(true);

            // Fill item description 
            itemDesImage.sprite = itemSprite;
            if (itemDesImage.sprite == null)
            {
                itemDesImage.sprite = emptyItemImage;
            }
            itemDesNameText.text = itemName;
            itemDesText.text = itemDescription;
        }
        else
        {
            isSelected = false;
            shaderAnimation.SetActive(false);
        }
    }

    private void EquipPrayerSlot()
    {
        if (isSelected && Keyboard.current.kKey.wasPressedThisFrame)
        {
            if (hasItem && !isItemEquipped)
            {
                InventoryManager.Instance.UnequipPrayers();
                InventoryManager.Instance.EquipPrayer(new Prayer
                {
                    itemName = itemName,
                    itemDescription = itemDescription,
                    itemSprite = itemSprite,
                    isEquip = true
                });
                isItemEquipped = true;
            }
        }
    }

    private void UnequipPrayerSlot()
    {
        if (isSelected && Keyboard.current.jKey.wasPressedThisFrame)
        {
            if (hasItem && isItemEquipped)
            {
                InventoryManager.Instance.UnequipPrayers();
            }
        }
    }
}
