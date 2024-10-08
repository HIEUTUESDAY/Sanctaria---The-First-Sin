﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;


public class CharacterEvent
{
    // Character damaged and damage value
    public static UnityAction<GameObject, float> characterDamaged;

    // Character healed and healed value
    public static UnityAction<GameObject, float> characterHealed;

    // Character hit splash when got hit
    public static UnityAction<GameObject, Vector2, int> hitSplash;

    // Show message
    public static UnityAction<Sprite, string> collectMessage;
    public static UnityAction notEnoughMessage;
}
