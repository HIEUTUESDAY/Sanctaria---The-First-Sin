using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }
}
