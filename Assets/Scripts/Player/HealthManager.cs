using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    [SerializeField] private HealthBar healthBar;


    [Header("Health Potions")]
    [SerializeField] private int maxHealthPotions = 2;
    public int currentHealthPotions;
    public int healthRestore = 50;

    private Damageable damageable;


    private void Awake()
    {
        damageable = GetComponent<Damageable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.SetHealthBar();
        this.SetHealthPotions();
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateHealthBar();

    }

    private void SetHealthPotions()
    {
        currentHealthPotions = maxHealthPotions;
    }

    private void SetHealthBar()
    {
        healthBar.SetMaxHealth(damageable.MaxHealth);
        healthBar.SetMaxStamina(damageable.MaxStamina);
        healthBar.SetMaxHealthPotions(maxHealthPotions);
    }

    private void UpdateHealthBar()
    {
        healthBar.SetHealth(damageable.Health);
        healthBar.SetHealthPotions(currentHealthPotions);
    }

}
