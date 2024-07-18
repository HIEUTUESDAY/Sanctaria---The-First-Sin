using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestItemsInventory", menuName = "Inventory/QuestItemsInventory")]
public class QuestItemInventorySO : ScriptableObject
{
    public List<QuestItem> QuestItems = new List<QuestItem>();
}
