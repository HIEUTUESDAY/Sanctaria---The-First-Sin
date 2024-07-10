using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedCols = new List<Collider2D>();
    public List<Collider2D> attackableCols = new List<Collider2D>();

    private void Update()
    {
        RemoveFromAttackCols();
    }

    private void RemoveFromAttackCols()
    {
        if (attackableCols != null && attackableCols.Count > 0)
        {
            List<Collider2D> colsToRemove = new List<Collider2D>();

            foreach (Collider2D col in attackableCols)
            {
                if (col != null)
                {
                    IDamageableBase damageable = col.GetComponent<IDamageableBase>();

                    if (damageable != null && !damageable.IsAlive)
                    {
                        colsToRemove.Add(col);
                    }
                }
            }

            foreach (Collider2D col in colsToRemove)
            {
                attackableCols.Remove(col);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!detectedCols.Contains(collision))
        {
            detectedCols.Add(collision);
        }

        if (!attackableCols.Contains(collision))
        {
            if(collision != null)
            {
                IDamageableBase damageable = collision.GetComponent<IDamageableBase>();

                if (damageable != null)
                {
                    if (damageable.IsAlive)
                    {
                        attackableCols.Add(collision);
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (detectedCols.Contains(collision))
        {
            detectedCols.Remove(collision);
           
        }

        if (attackableCols.Contains(collision))
        {
            attackableCols.Remove(collision);
        }
    }
}
