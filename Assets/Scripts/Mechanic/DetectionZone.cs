using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedCols = new List<Collider2D>();

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
