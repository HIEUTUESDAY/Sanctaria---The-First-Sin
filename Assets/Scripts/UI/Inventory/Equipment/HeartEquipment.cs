using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HeartEquipment : MonoBehaviour
{
    public Heart equippedHeart;

    [Header("Heart Display")]
    [SerializeField] private Image itemImage;
    [SerializeField] private Sprite emptyItemSprite;

    private void Update()
    {
        UpdateEquippedHeartDisplay();
    }

    public void UpdateEquippedHeartDisplay()
    {
        equippedHeart = InventoryManager.Instance.GetHeartEquipment();

        if (equippedHeart != null)
        {
            if (equippedHeart.itemSprite != null)
            {
                itemImage.sprite = equippedHeart.itemSprite;
            }
            else
            {
                itemImage.sprite = emptyItemSprite;
            }
        }
        else
        {
            itemImage.sprite = emptyItemSprite;
        }
    }
}
