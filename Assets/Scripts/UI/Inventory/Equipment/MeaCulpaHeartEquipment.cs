using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MeaCulpaHeartEquipment : MonoBehaviour
{
    public MeaCulpaHeart equippedMeaCulpaHeart;

    [Header("MeaCulpaHeart Display")]
    [SerializeField] private Image itemImage;
    [SerializeField] private Sprite emptyItemSprite;

    private void Update()
    {
        UpdateEquippedMeaCulpaHeartDisplay();
    }

    public void UpdateEquippedMeaCulpaHeartDisplay()
    {
        equippedMeaCulpaHeart = InventoryManager.Instance.GetMeaCulpaHeartEquipment();

        if (equippedMeaCulpaHeart != null)
        {
            if (equippedMeaCulpaHeart.itemSprite != null)
            {
                itemImage.sprite = equippedMeaCulpaHeart.itemSprite;
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
