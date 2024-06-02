using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    private static CoroutineManager _instance = null;

    public static CoroutineManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("CoroutineManager");
                DontDestroyOnLoad(obj);
                _instance = obj.AddComponent<CoroutineManager>();
            }
            return _instance;
        }
    }
}

