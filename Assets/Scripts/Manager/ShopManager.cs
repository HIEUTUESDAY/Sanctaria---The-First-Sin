using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("Buyable Item data")]
    public string itemName;
    public Sprite itemSprite;
    public string itemDescription;
    public float itemPrice;
    [Space(5)]

    [Header("Buyable Item Description")]
    public Image itemDesImage;
    public TMP_Text itemDesNameText;
    public TMP_Text itemDesText;
    public TMP_Text itemPriceText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void DisplayItem()
    {
        itemDesImage.sprite = itemSprite;
        itemDesNameText.text = itemName;
        itemDesText.text = itemDescription;
        itemPriceText.text = "Do you want to buy the object for " + itemPrice + " relic points ?";
    }

    public void AddItemToInventory()
    {
        if (InventoryManager.Instance.relicPoint >= itemPrice)
        {
            InventoryManager.Instance.AddQuestItemToInventory(new QuestItem
            {
                itemName = itemName,
                itemDescription = itemDescription,
                itemSprite = itemSprite
            });

            UIManager.Instance.shopMenu.SetActive(false);
            UIManager.Instance.menuActivated = false;
            Player.Instance.PlayerInput.enabled = true;

            Player.Instance.ItemBuyable.gameObject.SetActive(false);
            InventoryManager.Instance.relicPoint -= itemPrice;

            CharacterEvent.collectMessage.Invoke(itemSprite, itemName);
        }
        else
        {
            UIManager.Instance.shopMenu.SetActive(false);
            UIManager.Instance.menuActivated = false;
            Player.Instance.PlayerInput.enabled = true;

            CharacterEvent.notEnoughMessage.Invoke();
        }
        
    }

    public void StopDisPlayItem()
    {
        itemName = null;
        itemSprite = null;
        itemDescription = null;
        itemDesImage.sprite = null;
        itemDesNameText.text = null;
        itemDesText.text = null;
        itemPriceText.text = null;

        UIManager.Instance.shopMenu.SetActive(false);
        UIManager.Instance.menuActivated = false;
        Player.Instance.PlayerInput.enabled = true;
    }
}
