using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

public class QuestItemSlot : MonoBehaviour
{
    [Header("QuestItem data")]
    public string itemName;
    public Sprite itemSprite;
    public string itemDescription;
    public bool hasItem;
    [Space(5)]

    [Header("QuestItem Slot")]
    [SerializeField] private Image itemSlotImage;
    [SerializeField] private Sprite emptyItemSlotBGI;
    [SerializeField] private Sprite fillItemSlotBGI;
    [SerializeField] private Sprite selectedItemSlotBGI;
    [SerializeField] private Image itemImage;
    [SerializeField] private Sprite emptyItemImage;
    [Space(5)]

    [Header("QuestItem Description")]
    public Image itemDesImage;
    public TMP_Text itemDesNameText;
    public TMP_Text itemDesText;
    [Space(5)]

    public Button button;
    public bool isSelected;
    public GameObject shaderAnimation;

    private void Update()
    {
        QuestItemSlotSelect();
        UpdateQuestItemSlot();
    }

    public void AddQuestItemSlot(QuestItem questItem)
    {
        this.itemName = questItem.itemName;
        this.itemDescription = questItem.itemDescription;
        this.itemSprite = questItem.itemSprite;
        hasItem = true;
        itemImage.sprite = itemSprite;
    }

    public void ClearQuestItemSlot()
    {
        this.itemName = "";
        this.itemDescription = "";
        this.itemSprite = null;
        hasItem = false;
        itemImage.sprite = emptyItemImage;
    }

    private void UpdateQuestItemSlot()
    {
        if (!hasItem)
        {
            itemSlotImage.sprite = emptyItemSlotBGI;
        }

        if (hasItem && !isSelected)
        {
            itemSlotImage.sprite = fillItemSlotBGI;
        }
        else if (hasItem && isSelected)
        {
            itemSlotImage.sprite = selectedItemSlotBGI;
        }
    }

    private void QuestItemSlotSelect()
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

}
