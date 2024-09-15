using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Prayer : ItemBase
{
    public bool isItemEquipped;
    public float manaCost;

    public override void LoadSprite()
    {
        base.LoadSprite();

        if (!string.IsNullOrEmpty(itemSpriteName))
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/prayers");
            itemSprite = System.Array.Find(sprites, sprite => sprite.name == itemSpriteName);
        }
    }
}
