using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RelicPointBar : MonoBehaviour
{
    [Header("Relic Point data")]
    public float currentRelicPoint;

    [Header("Relic Point display")]
    public TMP_Text relicPointText;

    private float targetRelicPoint;
    private float countDuration = 0.2f;
    private Coroutine _countTo;

    private void Start()
    {
        currentRelicPoint = InventoryManager.Instance.relicPoint;
        relicPointText.text = ((int)currentRelicPoint).ToString();
    }

    private void Update()
    {
        RelicPointChangeCheck();
    }

    private void RelicPointChangeCheck()
    {
        targetRelicPoint = InventoryManager.Instance.relicPoint;
        if (targetRelicPoint != currentRelicPoint)
        {
            if (!UIManager.Instance.menuActivated)
            {
                if (_countTo != null)
                {
                    StopCoroutine(_countTo);
                }
                _countTo = StartCoroutine(CountTo(targetRelicPoint));
            }
            else
            {
                if (_countTo != null)
                {
                    StopCoroutine(_countTo);
                }
                currentRelicPoint = targetRelicPoint;
                relicPointText.text = ((int)currentRelicPoint).ToString();
            }
        }
    }

    public IEnumerator CountTo(float targetValue)
    {
        float rate = Mathf.Abs(targetValue - currentRelicPoint) / countDuration;
        while (currentRelicPoint != targetValue)
        {
            currentRelicPoint = Mathf.MoveTowards(currentRelicPoint, targetValue, rate * Time.deltaTime);
            relicPointText.text = ((int)currentRelicPoint).ToString();
            yield return null;
        }
    }

}