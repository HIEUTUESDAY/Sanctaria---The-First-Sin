using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Item data")]
    public string itemName;
    public Sprite itemSprite;
    public bool isFill;
    public string itemDescription;
    public Sprite emptySprite;
    [Space(5)]

    [Header("Item Slot")]
    [SerializeField] private Sprite emptyItemSlotBGI;
    [SerializeField] private Sprite fillItemSlotBGI;
    [SerializeField] private Sprite selectedItemSlotBGI;
    private Image itemSlotImage;
    [SerializeField] private Image itemImage;
    [Space(5)]

    [Header("Item Description")]
    public Image itemDesImage;
    public TMP_Text itemDesNameText;
    public TMP_Text itemDesText;
    [Space(5)]

    public bool isSelected;

    private void Start()
    {
        itemSlotImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (!isFill)
        {
            itemSlotImage.sprite = emptyItemSlotBGI;
        }

        if (isFill && !isSelected)
        {
            itemSlotImage.sprite = fillItemSlotBGI;
        }
        else if (isFill && isSelected)
        {
            itemSlotImage.sprite = selectedItemSlotBGI;
        }
    }

    public void AddItem(string itemName, string itemDescription, Sprite itemSprite)
    {
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.itemSprite = itemSprite;
        isFill = true;

        itemImage.sprite = itemSprite;
    }

    public void ClearItem()
    {
        this.itemName = "";
        this.itemDescription = "";
        this.itemSprite = null;
        isFill = false;

        itemImage.sprite = emptySprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(UIManager.Instance.menuActivated)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnLeftClick();
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightClick();
            }
        }
    }

    private void OnLeftClick()
    {
        // Ensure to deselect all slots of the current active inventory
        switch (InventoryManager.Instance.currentInventoryIndex)
        {
            case 0:
                InventoryManager.Instance.DeselectSlots(InventoryManager.Instance.questItemSlots);
                break;
            case 1:
                InventoryManager.Instance.DeselectSlots(InventoryManager.Instance.meaCulpaHeartSlots);
                break;
            case 2:
                InventoryManager.Instance.DeselectSlots(InventoryManager.Instance.prayerSlots);
                break;
        }

        isSelected = true;

        // Fill item description 
        itemDesImage.sprite = itemSprite;
        if (itemDesImage.sprite == null)
        {
            itemDesImage.sprite = emptySprite;
        }
        itemDesNameText.text = itemName;
        itemDesText.text = itemDescription;
    }

    private void OnRightClick()
    {
        // Handle right-click if needed
    }

}
