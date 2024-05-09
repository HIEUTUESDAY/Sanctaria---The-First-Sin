using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;


public class CharacterEvent
{
    // Character damaged and damage value
    public static UnityAction<GameObject, int> characterDamaged;

    // Character healed and healed value
    public static UnityAction<GameObject, int> characterHealed;
}
