using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrayerEquipment : MonoBehaviour
{
    public Prayer equippedPrayer;

    [Header("Prayer Display")]
    [SerializeField] private Image itemImage;
    [SerializeField] private Sprite emptyItemSprite;

    private void Update()
    {
        UpdateEquippedPrayerDisplay();
    }

    public void UpdateEquippedPrayerDisplay()
    {
        equippedPrayer = InventoryManager.Instance.GetEquippedPrayer();

        if (equippedPrayer != null)
        {
            if (equippedPrayer.itemSprite != null)
            {
                itemImage.sprite = equippedPrayer.itemSprite;
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
