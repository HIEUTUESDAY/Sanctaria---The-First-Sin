using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StableAudioListener : MonoBehaviour
{
    private Quaternion originalRotation;

    private void Start()
    {
        originalRotation = transform.rotation;
    }

   
    private void LateUpdate()
    {
        transform.rotation = originalRotation;
    }
}
