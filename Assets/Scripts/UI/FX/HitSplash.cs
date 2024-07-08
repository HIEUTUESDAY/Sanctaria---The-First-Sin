using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitSplash : MonoBehaviour
{
    public float timeToDestroy = 1f;
    private float timeElapsed = 0f;

    private void Update()
    {
        timeElapsed += Time.deltaTime;


        if (timeElapsed >= timeToDestroy)
        {
            Destroy(gameObject);
        }
    }
}

