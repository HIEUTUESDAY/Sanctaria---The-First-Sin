using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartInventoryHint : MonoBehaviour
{
    [SerializeField] private GameObject heartInventory;
    [SerializeField] private GameObject heartHint;

    private void Update()
    {
        if(heartInventory.activeSelf) 
        {
            if (Player.Instance.isKneelInCheckpoint)
            {
                heartHint.SetActive(false);
            }
            else
            {
                heartHint.SetActive(true);
            }
        }
    }
}
