using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SettingMenu : MonoBehaviour
{
    public UnityEvent backToMainMenu;


    private void Update()
    {
        BackToMainMenu();
    }

    private void BackToMainMenu()
    {
        if (gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            backToMainMenu.Invoke();
        }
    }
}
