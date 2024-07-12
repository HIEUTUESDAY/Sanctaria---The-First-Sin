using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestItemsInventory", menuName = "Inventory/QuestItemsInventory")]
public class QuestItemsInventorySO : ScriptableObject
{
    public List<QuestItems> items = new List<QuestItems>();
}
