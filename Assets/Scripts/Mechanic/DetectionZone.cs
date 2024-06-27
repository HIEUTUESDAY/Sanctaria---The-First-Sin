using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedCols = new List<Collider2D>();
    Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!detectedCols.Contains(collision))
        {
            detectedCols.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (detectedCols.Contains(collision))
        {
            detectedCols.Remove(collision);
        }
    }
}
