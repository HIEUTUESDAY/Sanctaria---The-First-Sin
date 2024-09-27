using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarManager : MonoBehaviour
{
    private Slider healthSlider;
    private Slider healthDecreasedSlider;
    [SerializeField] private float decreasedSpeed = 0.05f;

    private void Start()
    {
        healthSlider = UIManager.Instance.bossHealthSlider;
        healthDecreasedSlider = UIManager.Instance.bossHealthDecreasedSlider;
    }

    private void Update()
    {
        HealthBarDecreasesCheck();
    }

    public void SetMaxHealth(float health)
    {
        healthSlider.maxValue = health;
        healthDecreasedSlider.maxValue = health;
        healthSlider.value = health;
        healthDecreasedSlider.value = health;
    }

    public void SetHealth(float heatlh)
    {
        healthSlider.value = heatlh;
    }

    private void HealthBarDecreasesCheck()
    {
        if (healthSlider.value != healthDecreasedSlider.value)
        {
            healthDecreasedSlider.value = Mathf.Lerp(healthDecreasedSlider.value, healthSlider.value, decreasedSpeed);
        }
    }
}
