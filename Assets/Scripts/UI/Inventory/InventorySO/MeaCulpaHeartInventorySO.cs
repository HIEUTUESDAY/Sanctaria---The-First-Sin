using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeaCulpaHeartsInventory", menuName = "Inventory/MeaCulpaHeartsInventory")]
public class MeaCulpaHeartInventorySO : ScriptableObject
{
    public List<MeaCulpaHeart> MeaCulpaHearts = new List<MeaCulpaHeart>();
}
