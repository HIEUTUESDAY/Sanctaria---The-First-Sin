using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMenuControlHUD : MonoBehaviour
{
    public static MapMenuControlHUD Instance;
    public GameObject mapCenterPoint;
    public GameObject selectTeleportSlot;
    public GameObject mapHUD;
    public GameObject teleportHUD;

    private void Awake()
    {
        Instance = this;
    }
}
