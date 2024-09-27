using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

public class HeartSlot : MonoBehaviour
{
    [Header("Heart data")]
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

    [Header("Heart Slot")]
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

    [Header("Heart Description")]
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
        HeartSlotSelect();
        UpdateHeartSlot();
        HeartSlotAction();
    }

    public void AddHeartSlot(Heart heart)
    {
        heartName = heart.itemName;
        heartDescription = heart.itemDescription;
        heartSprite = heart.itemSprite;
        heartDamageModifier = heart.damageModifier;
        heartDefenseModifier = heart.defenseModifier;
        heartHealthModifier = heart.healthModifier;
        heartHealthRegenModifier = heart.healthRegenModifier;
        heartStaminaModifier = heart.manaModifier;
        heartStaminaRegenModifier = heart.manaRegenModifier;
        heartMoveSpeedModifier = heart.moveSpeedModifier;
        heartJumpPowerModifier = heart.jumpPowerModifier;
        heartWallJumpPowerModifier = heart.wallJumpPowerModifier;
        heartDashPowerModifier = heart.dashPowerModifier;
        hasHeart = true;
        isHeartEquipped = heart.isItemEquipped;
        heartImage.sprite = heartSprite;
    }

    public void ClearHeartSlot()
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

    private void UpdateHeartSlot()
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

                if (Player.Instance.isKneelInCheckpoint)
                {
                    if (isHeartEquipped)
                    {
                        removeSelection.SetActive(true);
                        equipSelection.SetActive(false);
                    }
                    else
                    {
                        removeSelection.SetActive(false);
                        equipSelection.SetActive(true);
                    }
                }
                else
                {
                    removeSelection.SetActive(false);
                    equipSelection.SetActive(false);
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

    private void HeartSlotSelect()
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

    private void HeartSlotAction()
    {
        if (hasHeart && isSelected && Input.GetKeyDown(KeyCode.Return) && Player.Instance.isKneelInCheckpoint)
        {
            if (!isHeartEquipped)
            {
                isHeartEquipped = true;
                InventoryManager.Instance.EquipNewHeart(new Heart
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
                InventoryManager.Instance.UnequipHeart();
            }
        }
    }
}
