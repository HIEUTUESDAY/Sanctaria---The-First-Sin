using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TearsBar : MonoBehaviour
{
    [Header("Tears of Atonement data")]
    public float currentTearsOfAtonement;

    [Header("Tears of Atonement display")]
    public TMP_Text tearsText;

    private float targetTearsOfAtonement;
    private float countDuration = 0.2f;
    private Coroutine _countTo;

    private void Start()
    {
        currentTearsOfAtonement = InventoryManager.Instance.tearsOfAtonement;
    }

    private void Update()
    {
        targetTearsOfAtonement = InventoryManager.Instance.tearsOfAtonement;
        if (targetTearsOfAtonement != currentTearsOfAtonement)
        {
            if (_countTo != null)
            {
                StopCoroutine(_countTo);
            }
            _countTo = StartCoroutine(CountTo(targetTearsOfAtonement));
        }
    }

    public IEnumerator CountTo(float targetValue)
    {
        float rate = Mathf.Abs(targetValue - currentTearsOfAtonement) / countDuration;
        while (currentTearsOfAtonement != targetValue)
        {
            currentTearsOfAtonement = Mathf.MoveTowards(currentTearsOfAtonement, targetValue, rate * Time.deltaTime);
            tearsText.text = ((int)currentTearsOfAtonement).ToString();
            yield return null;
        }
    }

}