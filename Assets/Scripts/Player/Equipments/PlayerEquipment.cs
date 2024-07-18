using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private Player player;
    [SerializeField] private MeaCulpaHeart equippedMeaCulpaHeart;
    [SerializeField] private Prayer equippedPrayer;
    private Dictionary<string, Action> meaCulpaHeartBonuses;

    private void Start()
    {
        player = GetComponent<Player>();
        InitializeMeaCulpaHeartBonuses();
        UpdateEquippedItems();
    }

    private void InitializeMeaCulpaHeartBonuses()
    {
        meaCulpaHeartBonuses = new Dictionary<string, Action>
        {
            { "Heart of Smoking Incense", ApplyHeartOfSmokingIncenseBonus },
            { "Heart of The Single Tone", ApplyHeartOfTheSingleToneBonus }
            // Add more items and their bonus methods here
        };
    }

    public void UpdateEquippedItems()
    {
        var newEquippedMeaCulpaHeart = InventoryManager.Instance.GetEquippedMeaCulpaHeart();

        if (newEquippedMeaCulpaHeart != equippedMeaCulpaHeart)
        {
            RemoveMeaCulpaHeartBonus();
            equippedMeaCulpaHeart = newEquippedMeaCulpaHeart;
            PerformMeaCulpaHeart();
        }
        equippedPrayer = InventoryManager.Instance.GetEquippedPrayer();
    }

    private void RemoveMeaCulpaHeartBonus()
    {
        if (equippedMeaCulpaHeart != null && meaCulpaHeartBonuses.ContainsKey(equippedMeaCulpaHeart.itemName))
        {
            switch (equippedMeaCulpaHeart.itemName)
            {
                case "Heart of Smoking Incense":
                    player.moveSpeed -= 5f;
                    break;
                case "Heart of The Single Tone":
                    player.MaxHealth -= 50f;
                    break;
                // Add more cases for other MeaCulpaHearts if needed
                default:
                    break;
            }
        }
    }

    public void PerformMeaCulpaHeart()
    {
        if (equippedMeaCulpaHeart == null) return;

        if (meaCulpaHeartBonuses.ContainsKey(equippedMeaCulpaHeart.itemName))
        {
            meaCulpaHeartBonuses[equippedMeaCulpaHeart.itemName].Invoke();
        }
    }

    public void PerformPrayer()
    {
        if (equippedPrayer == null) return;

        switch (equippedPrayer.itemName)
        {
            case "Tanranto of My Sister":
                Debug.Log("Using Tanranto of My Sister");
                break;

            // Add more cases for other Prayers
            default:
                break;
        }
    }

    #region MeaCulpaHearts function

    private void ApplyHeartOfSmokingIncenseBonus()
    {
        player.moveSpeed += 5f;
    }

    private void ApplyHeartOfTheSingleToneBonus()
    {
        player.MaxHealth += 50f;
    }

    #endregion

    #region Prayers function

    // Add functions for Prayers here

    #endregion
}
