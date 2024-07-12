using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeaCulpaHeartsInventory", menuName = "Inventory/MeaCulpaHeartsInventory")]
public class MeaCulpaHeartsInventorySO : ScriptableObject
{
    public List<MeaCulpaHearts> items = new List<MeaCulpaHearts>();
}
