using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectable : MonoBehaviour
{
    public string itemName;
    public Sprite itemSprite;
    [TextArea] public string itemDescription;
    public bool isOnFloor;

    public bool IsOnFloor
    {
        get { return isOnFloor; }
        private set { isOnFloor = value; }
    }
    public bool canBeCollect = false;


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

    public virtual void CollectItem()
    {

    }
}
