using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite sprite;
    [SerializeField] private bool isOnFloor;
    [TextArea][SerializeField] private string itemDescription;
    [SerializeField] private InventoryCategory category;

    public bool IsOnFloor
    {
        get { return isOnFloor; }
        private set { isOnFloor = value; }
    }
    private bool canBeCollect = false;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canBeCollect = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canBeCollect = false;
        }
    }

    public void CollectItem()
    {
        if (canBeCollect)
        {
            inventoryManager.AddItem(category, itemName, itemDescription, sprite);
            Destroy(gameObject);
        }
    }

    public enum InventoryCategory
    {
        QuestItems,
        MeaCulpaHearts,
        Prayers
    }
}
