using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public List<Transform> pointList = new List<Transform>();
    public Transform nextPoint;
    public int pointNumber = 0;

    private void OnDrawGizmos()
    {
        if (pointList.Count > 0)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < pointList.Count; i++)
            {
                Gizmos.DrawSphere(pointList[i].position, 0.1f);

                if (i < pointList.Count - 1)
                {
                    Gizmos.DrawLine(pointList[i].position, pointList[i + 1].position);
                }
            }
        }
    }

}
