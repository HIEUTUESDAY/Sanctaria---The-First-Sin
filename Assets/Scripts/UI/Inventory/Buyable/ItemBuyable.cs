using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemBuyable : MonoBehaviour
{
    public string itemName;
    public Sprite itemSprite;
    [TextArea] public string itemDescription;
    public float itemPrice;

    [SerializeField] private GameObject interactIcon;

    public void InspectItem()
    {
        ShopManager.Instance.itemName = itemName;
        ShopManager.Instance.itemSprite = itemSprite;
        ShopManager.Instance.itemDescription = itemDescription;
        ShopManager.Instance.itemPrice = itemPrice;
        ShopManager.Instance.DisplayItem();

        Player.Instance.PlayerInput.enabled = false;
        UIManager.Instance.menuActivated = true;
        UIManager.Instance.shopMenu.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactIcon.SetActive(false);
        }
    }

}
