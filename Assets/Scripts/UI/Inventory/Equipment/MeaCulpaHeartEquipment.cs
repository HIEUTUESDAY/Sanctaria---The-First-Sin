using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MeaCulpaHeartEquipment : MonoBehaviour
{
    [Header("MeaCulpaHeart data")]
    public string meaCulpaHeartName;
    public Sprite meaCulpaHeartSprite;
    public string meaCulpaHeartDescription;
    public bool hasMeaCulpaHeartEquipped;
    [Space(5)]

    [Header("MeaCulpaHeart Eqiipment")]
    [SerializeField] private Image itemImage;
    [SerializeField] private Sprite emptyItemImage;

    private void Update()
    {
        if (meaCulpaHeartSprite != null)
        {
            itemImage.sprite = meaCulpaHeartSprite;
        }
        else
        {
            itemImage.sprite = emptyItemImage;
        }
    }

    public void EquipMeaCulpaHeart(MeaCulpaHeart meaCulpaHeart)
    {
        this.meaCulpaHeartName = meaCulpaHeart.itemName;
        this.meaCulpaHeartDescription = meaCulpaHeart.itemDescription;
        this.meaCulpaHeartSprite = meaCulpaHeart.itemSprite;

        itemImage.sprite = meaCulpaHeart.itemSprite;
    }

    public void UnequipMeaCulpaHeart()
    {
        this.meaCulpaHeartName = "";
        this.meaCulpaHeartDescription = "";
        this.meaCulpaHeartSprite = null;

        itemImage.sprite = emptyItemImage;
    }
}
