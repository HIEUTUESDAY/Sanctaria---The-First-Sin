using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestItem : ItemBase
{
    public override void LoadSprite()
    {
        base.LoadSprite();

        if (!string.IsNullOrEmpty(itemSpriteName))
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/items");
            itemSprite = System.Array.Find(sprites, sprite => sprite.name == itemSpriteName);
        }
    }
}
