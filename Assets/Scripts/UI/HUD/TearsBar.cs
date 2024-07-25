using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TearsBar : MonoBehaviour
{
    [Header("Tears of Atonement data")]
    public int tearsOfAtonement;

    [Header("Tears of Atonement display")]
    public TMP_Text tearsText;

    private void Update()
    {
        tearsOfAtonement = InventoryManager.Instance.tearsOfAtonement;
        tearsText.text = tearsOfAtonement.ToString();
    }
}