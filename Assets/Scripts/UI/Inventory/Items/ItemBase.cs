using UnityEngine;

[System.Serializable]
public class ItemBase
{
    public string itemName;
    public string itemSpriteName;
    public string itemDescription;

    [System.NonSerialized]
    public Sprite itemSprite;

    public void SaveSpriteName()
    {
        if (itemSprite != null)
        {
            itemSpriteName = itemSprite.name;
        }
    }

    public virtual void LoadSprite()
    {
        
    }
}
