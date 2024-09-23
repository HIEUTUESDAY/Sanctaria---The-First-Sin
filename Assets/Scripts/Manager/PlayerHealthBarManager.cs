using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarManager : MonoBehaviour
{
    public Slider healthSlider;
    public Slider healthDecreasedSlider;
    public Slider manaSlider;
    public Slider manaDecreasedSlider;
    public Slider[] healthPotionsSlider;
    public GameObject activatedPrayerBorder;
    public Image manaSliderImg;
    public Image PrayerCooldownImg;
    [SerializeField] private float decreasedSpeed = 0.02f;

    public Image manaTickPrefab;
    public Transform manaTickContainer;
    private List<Image> manaTicks = new List<Image>();
    private float lastManaCost = 0;

    private void Update()
    {
        HeathBarCheck();
        PrayerCooldownCheck();
        SetPrayerManaCost(Player.Instance.prayerManaCost);
    }

    public void SetMaxHealth(float health)
    {
        healthSlider.maxValue = health;
        healthDecreasedSlider.maxValue = health;
    }

    public void SetHealth(float heatlh)
    {
        healthSlider.value = heatlh;
    }

    public void SetMaxMana(float stamina)
    {
        manaSlider.maxValue = stamina;
        manaDecreasedSlider.maxValue = stamina;
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
            Player.Instance.prayerCooldown -= Time.deltaTime;
        }
        else if (Player.Instance.prayerCooldown <= 0)
        {
            Player.Instance.prayerCooldown = 0;
            PrayerCooldownImg.fillAmount = 0;
            activatedPrayerBorder.SetActive(false);
        }
    }

    private void HeathBarCheck()
    {
        if (Player.Instance.CurrentHealth != healthDecreasedSlider.value)
        {
            healthDecreasedSlider.value = Mathf.Lerp(healthDecreasedSlider.value, Player.Instance.CurrentHealth, decreasedSpeed);
        }

        if (Player.Instance.CurrentMana != manaDecreasedSlider.value)
        {
            manaDecreasedSlider.value = Mathf.Lerp(manaDecreasedSlider.value, Player.Instance.CurrentMana, decreasedSpeed);
        }

        if (Player.Instance.prayerManaCost > 0f)
        {
            if (Player.Instance.CurrentMana >= Player.Instance.prayerManaCost)
            {
                manaSliderImg.color = new Color32(0, 200, 255, 255);
            }
            else
            {
                manaSliderImg.color = new Color32(0, 0, 200, 255);
            }
        }
        else
        {
            manaSliderImg.color = new Color32(0, 0, 200, 255);
        }
    }

    public void SetPrayerManaCost(float manaCost)
    {
        if (manaCost != lastManaCost)
        {
            lastManaCost = manaCost;
            UpdateManaTicks();
        }
    }

    private void UpdateManaTicks()
    {
        foreach (var tick in manaTicks)
        {
            Destroy(tick.gameObject);
        }
        manaTicks.Clear();

        if (lastManaCost > 0 && manaSlider.maxValue > 0)
        {
            float maxMana = manaSlider.maxValue;
            int numberOfTicks = Mathf.FloorToInt(maxMana / lastManaCost);

            for (int i = 1; i < numberOfTicks; i++)
            {
                float tickPosition = (i * lastManaCost) / maxMana;

                Image tickMark = Instantiate(manaTickPrefab, manaTickContainer);
                tickMark.rectTransform.anchorMin = new Vector2(tickPosition, 0.5f);
                tickMark.rectTransform.anchorMax = new Vector2(tickPosition, 0.5f);
                tickMark.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                tickMark.rectTransform.anchoredPosition = Vector2.zero;

                manaTicks.Add(tickMark);
            }
        }
    }
}
