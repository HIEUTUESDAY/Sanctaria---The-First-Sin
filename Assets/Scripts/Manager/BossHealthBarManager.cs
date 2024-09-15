using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarManager : MonoBehaviour
{
    private Slider healthSlider;

    private void Start()
    {
        healthSlider = UIManager.Instance.tenPiedadHealthBar.GetComponentInChildren<Slider>();
    }

    public void SetMaxHealth(float health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }
    public void SetHealth(float heatlh)
    {
        healthSlider.value = heatlh;
    }
}
