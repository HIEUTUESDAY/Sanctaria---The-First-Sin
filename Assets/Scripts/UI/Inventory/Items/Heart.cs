using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Heart : ItemBase
{
    public bool isItemEquipped;
    public float damageModifier;
    public float defenseModifier;
    public float healthModifier;
    public float healthRegenModifier;
    public float manaModifier;
    public float manaRegenModifier;
    public float moveSpeedModifier;
    public float jumpPowerModifier;
    public float wallJumpPowerModifier;
    public float dashPowerModifier;

    public override void LoadSprite()
    {
        base.LoadSprite();

        if (!string.IsNullOrEmpty(itemSpriteName))
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/hearts");
            itemSprite = System.Array.Find(sprites, sprite => sprite.name == itemSpriteName);
        }
    }
}
