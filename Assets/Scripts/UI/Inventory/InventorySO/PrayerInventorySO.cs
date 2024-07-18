using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrayersInventory", menuName = "Inventory/PrayersInventory")]
public class PrayerInventorySO : ScriptableObject
{
    public List<Prayer> Prayers = new List<Prayer>();
}
