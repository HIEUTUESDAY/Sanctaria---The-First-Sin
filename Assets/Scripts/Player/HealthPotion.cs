using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    private Damageable damageable;

    [SerializeField] private HealthBar healthBar;

    [Header("Health Potions")]
    public int maxHealthPotions = 2;
    public int healthRestore = 50;

    public int _currentHealthPotions;

    public int CurrentHealthPotions
    {
        get
        {
            return _currentHealthPotions;
        }
        set
        {
            _currentHealthPotions = value;
        }
    }


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
        _currentHealthPotions = maxHealthPotions;
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
        healthBar.SetHealthPotions(_currentHealthPotions);
    }

}