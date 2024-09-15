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
    public string heartName;
    public Sprite heartSprite;
    public string heartDescription;
    public float heartDamageModifier;
    public float heartDefenseModifier;
    public float heartHealthModifier;
    public float heartHealthRegenModifier;
    public float heartStaminaModifier;
    public float heartStaminaRegenModifier;
    public float heartMoveSpeedModifier;
    public float heartJumpPowerModifier;
    public float heartWallJumpPowerModifier;
    public float heartDashPowerModifier;
    [Space(5)]

    [Header("MeaCulpaHeart Slot")]
    [SerializeField] private Image heartSlotImage;
    [SerializeField] private Sprite emptyHeartSlotBGI;
    [SerializeField] private Sprite fillHeartSlotBGI;
    [SerializeField] private Sprite selectedHeartSlotBGI;
    [SerializeField] private Sprite equippedHeartSlotBGI;
    [SerializeField] private Image heartImage;
    [SerializeField] private Sprite emptyHeartImage;
    public bool hasHeart;
    public bool isHeartEquipped;
    [Space(5)]

    [Header("MeaCulpaHeart Description")]
    public Image heartDesImage;
    public TMP_Text heartDesNameText;
    public TMP_Text heartDesText;
    public GameObject equipSelection;
    public GameObject removeSelection;
    [Space(5)]

    public Button button;
    public bool isSelected;
    public GameObject shaderAnimation;

    private void Update()
    {
        MeaCulpaHeartSlotSelect();
        UpdateMeaCulpaHeartSlot();
        MeaCulpaHeartSlotAction();
    }

    public void AddMeaCulpaHeartSlot(MeaCulpaHeart meaCulpaHeart)
    {
        heartName = meaCulpaHeart.itemName;
        heartDescription = meaCulpaHeart.itemDescription;
        heartSprite = meaCulpaHeart.itemSprite;
        heartDamageModifier = meaCulpaHeart.damageModifier;
        heartDefenseModifier = meaCulpaHeart.defenseModifier;
        heartHealthModifier = meaCulpaHeart.healthModifier;
        heartHealthRegenModifier = meaCulpaHeart.healthRegenModifier;
        heartStaminaModifier = meaCulpaHeart.manaModifier;
        heartStaminaRegenModifier = meaCulpaHeart.manaRegenModifier;
        heartMoveSpeedModifier = meaCulpaHeart.moveSpeedModifier;
        heartJumpPowerModifier = meaCulpaHeart.jumpPowerModifier;
        heartWallJumpPowerModifier = meaCulpaHeart.wallJumpPowerModifier;
        heartDashPowerModifier = meaCulpaHeart.dashPowerModifier;
        hasHeart = true;
        isHeartEquipped = meaCulpaHeart.isItemEquipped;
        heartImage.sprite = heartSprite;
    }

    public void ClearMeaCulpaHeartSlot()
    {
        heartName = "";
        heartDescription = "";
        heartSprite = null;
        heartDamageModifier = 0f;
        heartDefenseModifier = 0f;
        heartHealthModifier = 0f;
        heartHealthRegenModifier = 0f;
        heartStaminaModifier = 0f;
        heartStaminaRegenModifier = 0f;
        heartMoveSpeedModifier = 0f;
        heartJumpPowerModifier = 0f;
        heartWallJumpPowerModifier = 0f;
        heartDashPowerModifier = 0f;
        hasHeart = false;
        isHeartEquipped = false;
        heartImage.sprite = emptyHeartImage;
    }

    private void UpdateMeaCulpaHeartSlot()
    {
        if (hasHeart)
        {
            if(!isSelected)
            {
                heartSlotImage.sprite = fillHeartSlotBGI;
            }
            else
            {
                heartSlotImage.sprite = selectedHeartSlotBGI;

                if (isHeartEquipped)
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

            if (isHeartEquipped)
            {
                heartSlotImage.sprite = equippedHeartSlotBGI;
            }
        }
        else if (!hasHeart && isSelected)
        {
            removeSelection.SetActive(false);
            equipSelection.SetActive(false);
        }
        else
        {
            heartSlotImage.sprite = emptyHeartSlotBGI;
        }
    }

    private void MeaCulpaHeartSlotSelect()
    {
        if (EventSystem.current.currentSelectedGameObject == button.gameObject)
        {
            isSelected = true;
            shaderAnimation.SetActive(true);

            // Fill item description 
            heartDesImage.sprite = heartSprite;
            if (heartDesImage.sprite == null)
            {
                heartDesImage.sprite = emptyHeartImage;
            }
            heartDesNameText.text = heartName;
            heartDesText.text = heartDescription;
        }
        else
        {
            isSelected = false;
            shaderAnimation.SetActive(false);
        }
    }

    private void MeaCulpaHeartSlotAction()
    {
        if (hasHeart && isSelected && Input.GetKeyDown(KeyCode.Return))
        {
            if (!isHeartEquipped)
            {
                isHeartEquipped = true;
                InventoryManager.Instance.EquipNewMeaCulpaHeart(new MeaCulpaHeart
                {
                    itemName = heartName,
                    itemDescription = heartDescription,
                    itemSprite = heartSprite,
                    isItemEquipped = isHeartEquipped,
                    damageModifier = heartDamageModifier,
                    defenseModifier = heartDefenseModifier,
                    healthModifier = heartHealthModifier,
                    healthRegenModifier = heartHealthRegenModifier,
                    manaModifier = heartStaminaModifier,
                    manaRegenModifier = heartStaminaRegenModifier,
                    moveSpeedModifier = heartMoveSpeedModifier,
                    jumpPowerModifier = heartJumpPowerModifier,
                    wallJumpPowerModifier = heartWallJumpPowerModifier,
                    dashPowerModifier = heartDashPowerModifier,
                });
            }
            else
            {
                isHeartEquipped = false;
                InventoryManager.Instance.UnequipMeaCulpaHeart();
            }
        }
    }
}
