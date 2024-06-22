using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/Connection")]
public class ScriptableLevelConnection : ScriptableObject
{
    public static ScriptableLevelConnection ActiveConnection { get; set; }
}
