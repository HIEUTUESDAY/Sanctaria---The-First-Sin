using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistence : MonoBehaviour
{
    public static Persistence Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
