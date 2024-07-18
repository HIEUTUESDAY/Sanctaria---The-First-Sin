using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

public class MeaCulpaHeartSlot : MonoBehaviour
{
    [Header("MeaCulpaHeart data")]
    public string itemName;
    public Sprite itemSprite;
    public string itemDescription;
    public bool isItemEquipped;
    public bool hasItem;
    [Space(5)]

    [Header("MeaCulpaHeart Slot")]
    [SerializeField] private Image itemSlotImage;
    [SerializeField] private Sprite emptyItemSlotBGI;
    [SerializeField] private Sprite fillItemSlotBGI;
    [SerializeField] private Sprite selectedItemSlotBGI;
    [SerializeField] private Sprite equippedItemSlotBGI;
    [SerializeField] private Image itemImage;
    [SerializeField] private Sprite emptyItemImage;
    [Space(5)]

    [Header("MeaCulpaHeart Description")]
    public Image itemDesImage;
    public TMP_Text itemDesNameText;
    public TMP_Text itemDesText;
    [Space(5)]

    public Button button;
    public bool isSelected;
    public GameObject shaderAnimation;

    public MeaCulpaHeartEquipment meaCulpaHeartEquipment;

    private void Update()
    {
        MeaCulpaHeartSlotSelect();
        UpdateMeaCulpaHeartSlot();
        EquipMeaCulpaHeartSlot();
        UnequipMeaCulpaHeartSlot();
    }

    public void AddMeaCulpaHeartSlot(MeaCulpaHeart meaCulpaHeart)
    {
        this.itemName = meaCulpaHeart.itemName;
        this.itemDescription = meaCulpaHeart.itemDescription;
        this.itemSprite = meaCulpaHeart.itemSprite;
        this.isItemEquipped = meaCulpaHeart.isEquip;
        hasItem = true;

        itemImage.sprite = itemSprite;
    }

    public void ClearMeaCulpaHeartSlot()
    {
        this.itemName = "";
        this.itemDescription = "";
        this.itemSprite = null;
        this.isItemEquipped = false;
        hasItem = false;

        itemImage.sprite = emptyItemImage;
    }

    private void UpdateMeaCulpaHeartSlot()
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

    private void MeaCulpaHeartSlotSelect()
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

    private void EquipMeaCulpaHeartSlot()
    {
        if (isSelected && Keyboard.current.kKey.wasPressedThisFrame)
        {
            if (hasItem && !isItemEquipped)
            {
                InventoryManager.Instance.UnequipMeaCulpaHearts();
                InventoryManager.Instance.EquipMeaCulpaHeart(new MeaCulpaHeart
                {
                    itemName = this.itemName,
                    itemDescription = this.itemDescription,
                    itemSprite = this.itemSprite,
                    isEquip = true
                });
                isItemEquipped = true;
            }
        }
    }

    private void UnequipMeaCulpaHeartSlot()
    {
        if (isSelected && Keyboard.current.jKey.wasPressedThisFrame)
        {
            if (hasItem && isItemEquipped)
            {
                InventoryManager.Instance.UnequipMeaCulpaHearts();
            }
        }
    }
}
