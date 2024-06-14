using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider staminaSlider;

    public Slider[] healthPotionsSlider;

    public void SetMaxHealth(int health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }
    public void SetHealth(int heatlh)
    {
        healthSlider.value = heatlh;
    }

    public void SetMaxStamina(int stamina)
    {
        staminaSlider.maxValue = stamina;
        staminaSlider.value = stamina;
    }

    public void SetStamina(int stamina)
    {
        staminaSlider.value = stamina;
    }

    public void SetMaxHealthPotions(int maxPotions)
    {
        for (int i = 0; i < healthPotionsSlider.Length; i++)
        {
            healthPotionsSlider[i].maxValue = 1;
            healthPotionsSlider[i].value = i < maxPotions ? 1 : 0;
        }
    }

    public void SetHealthPotions(int currentPotions)
    {
        for (int i = 0; i < healthPotionsSlider.Length; i++)
        {
            healthPotionsSlider[i].value = i < currentPotions ? 1 : 0;
        }
    }
}
