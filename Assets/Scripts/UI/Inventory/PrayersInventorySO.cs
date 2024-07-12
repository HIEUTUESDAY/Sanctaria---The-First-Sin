using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrayersInventory", menuName = "Inventory/PrayersInventory")]
public class PrayersInventorySO : ScriptableObject
{
    public List<Prayers> items = new List<Prayers>();
}
