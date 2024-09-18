using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarManager : MonoBehaviour
{
    public Slider healthSlider;
    public Slider manaSlider;
    public Slider[] healthPotionsSlider;
    public GameObject activatedPrayerBorder;
    public Image PrayerCooldownImg;

    private void Update()
    {
        PrayerCooldownCheck();
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

    public void SetMaxStamina(float stamina)
    {
        manaSlider.maxValue = stamina;
        manaSlider.value = stamina;
    }

    public void SetMana(float stamina)
    {
        manaSlider.value = stamina;
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

    private void PrayerCooldownCheck()
    {
        if (Player.Instance.prayerCooldown > 0)
        {
            activatedPrayerBorder.SetActive(true);

            float fillAmount = Player.Instance.prayerCooldown / Player.Instance.prayerCooldownTime;
            PrayerCooldownImg.fillAmount = fillAmount;

            // Decrease the cooldown over time
            Player.Instance.prayerCooldown -= Time.deltaTime;
        }
        else if (Player.Instance.prayerCooldown <= 0)
        {
            activatedPrayerBorder.SetActive(false);
            Player.Instance.prayerCooldown = 0;
            PrayerCooldownImg.fillAmount = 0;
        }
    }
}
