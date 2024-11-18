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
    public string prayerName;
    public Sprite prayerSprite;
    public string prayerDescription;
    public float prayerManaCost;
    [Space(5)]

    [Header("Prayer Slot")]
    [SerializeField] private Image prayerSlotImage;
    [SerializeField] private Sprite emptyPrayerSlotBGI;
    [SerializeField] private Sprite fillPrayerSlotBGI;
    [SerializeField] private Sprite selectedPrayerSlotBGI;
    [SerializeField] private Sprite equippedPrayerSlotBGI;
    [SerializeField] private Image prayerImage;
    [SerializeField] private Sprite emptyPrayerImage;
    public bool hasPrayer;
    public bool isPrayerEquipped;
    [Space(5)]

    [Header("Prayer Description")]
    public Image prayerDesImage;
    public TMP_Text prayerDesNameText;
    public TMP_Text prayerDesText;
    public GameObject equipSelection;
    public GameObject removeSelection;
    [Space(5)]

    public Button button;
    public bool isSelected;
    public GameObject shaderAnimation;
    private GameObject lastSelectedItem;

    private void Update()
    {
        PrayerSlotSelect();
        UpdatePrayerSlot();
        PrayerSlotAction();
    }

    public void AddPrayerSlot(Prayer prayer)
    {
        prayerName = prayer.itemName;
        prayerDescription = prayer.itemDescription;
        prayerManaCost = prayer.manaCost;
        prayerSprite = prayer.itemSprite;
        hasPrayer = true;
        isPrayerEquipped = prayer.isItemEquipped;
        prayerImage.sprite = prayerSprite;
    }

    public void ClearPrayerSlot()
    {
        prayerName = "";
        prayerDescription = "";
        prayerManaCost = 0f;
        prayerSprite = null;
        hasPrayer = false;
        isPrayerEquipped = false;

        prayerImage.sprite = emptyPrayerImage;
    }

    private void UpdatePrayerSlot()
    {
        if (hasPrayer)
        {
            if (!isSelected)
            {
                prayerSlotImage.sprite = fillPrayerSlotBGI;
            }
            else
            {
                prayerSlotImage.sprite = selectedPrayerSlotBGI;

                if (isPrayerEquipped)
                {
                    equipSelection.SetActive(false);
                    removeSelection.SetActive(true);
                }
                else
                {
                    removeSelection.SetActive(false);
                    equipSelection.SetActive(true);
                }
            }

            if (isPrayerEquipped)
            {
                prayerSlotImage.sprite = equippedPrayerSlotBGI;
            }
        }
        else if (!hasPrayer && isSelected)
        {
            removeSelection.SetActive(false);
            equipSelection.SetActive(false);
        }
        else
        {
            prayerSlotImage.sprite = emptyPrayerSlotBGI;
        }
    }

    private void PrayerSlotSelect()
    {
        if (EventSystem.current.currentSelectedGameObject == button.gameObject)
        {
            if (!isSelected || EventSystem.current.currentSelectedGameObject != lastSelectedItem)
            {
                SoundFXManager.Instance.PlayChangeSelectionSound();
                lastSelectedItem = EventSystem.current.currentSelectedGameObject;
            }

            isSelected = true;
            shaderAnimation.SetActive(true);

            prayerDesImage.sprite = prayerSprite;
            if (prayerDesImage.sprite == null)
            {
                prayerDesImage.sprite = emptyPrayerImage;
            }
            prayerDesNameText.text = prayerName;
            prayerDesText.text = prayerDescription;
        }
        else
        {
            isSelected = false;
            shaderAnimation.SetActive(false);
        }
    }

    private void PrayerSlotAction()
    {
        if (hasPrayer && isSelected && Input.GetKeyDown(KeyCode.Return))
        {
            if (!isPrayerEquipped)
            {
                isPrayerEquipped = true;
                InventoryManager.Instance.EquipNewPrayer(new Prayer
                {
                    itemName = prayerName,
                    itemDescription = prayerDescription,
                    manaCost = prayerManaCost,
                    itemSprite = prayerSprite,
                    isItemEquipped = isPrayerEquipped
                });

                SoundFXManager.Instance.PlayEquipItemSound();

            }
            else
            {
                isPrayerEquipped = false;
                InventoryManager.Instance.UnequipPrayer();
                SoundFXManager.Instance.PlayUnequipItemSound();
            }
        }
    }

}
