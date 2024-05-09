using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DetectionZone : MonoBehaviour
{
    public UnityEvent noGroundRemain;
    public List<Collider2D> detectedCols = new List<Collider2D>();
    Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collusion)
    {
        detectedCols.Add(collusion);    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedCols.Remove(collision);
        if(detectedCols.Count <= 0) 
        {
            noGroundRemain.Invoke();
        }
    }
}
