using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrayerEquipment : MonoBehaviour
{
    [Header("Prayer data")]
    public string prayerName;
    public Sprite prayerSprite;
    public string prayerDescription;
    [Space(5)]

    [Header("Prayer Eqiipment")]
    [SerializeField] private Image itemImage;
    [SerializeField] private Sprite emptyItemImage;

    private void Update()
    {
        if (prayerSprite != null)
        {
            itemImage.sprite = prayerSprite;
        }
        else
        {
            itemImage.sprite = emptyItemImage;
        }
    }

    public void EquipPrayer(Prayer prayer)
    {
        this.prayerName = prayer.itemName;
        this.prayerDescription = prayer.itemDescription;
        this.prayerSprite = prayer.itemSprite;

        itemImage.sprite = prayer.itemSprite;
    }

    public void UnequipPrayer()
    {
        this.prayerName = "";
        this.prayerDescription = "";
        this.prayerSprite = null;

        itemImage.sprite = emptyItemImage;
    }
}
