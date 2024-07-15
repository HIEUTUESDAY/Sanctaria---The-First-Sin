using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Transform VFXcanvas;
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public GameObject[] playerHitSplashPrefab;
    public GameObject[] nonPlayerHitSplashPrefab;
    public GameObject inventoryMenu;
    public bool menuActivated = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        VFXcanvas = FindObjectOfType<Canvas>().transform.Find("VFX");

        CharacterEvent.characterDamaged += CharacterTookDamage;
        CharacterEvent.characterHealed += CharacterHealed;
        CharacterEvent.hitSplash += ShowHitSplash;
    }

    private void OnDisable()
    {
        CharacterEvent.characterDamaged -= CharacterTookDamage;
        CharacterEvent.characterHealed -= CharacterHealed;
        CharacterEvent.hitSplash -= ShowHitSplash;
    }

    public void CharacterTookDamage(GameObject character, float damageReceived)
    {
        // Create text at where character get hit
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, VFXcanvas).GetComponent<TMP_Text>();

        tmpText.text = ("-" + damageReceived.ToString());
    }

    public void CharacterHealed(GameObject character, float healthRestored)
    {
        // Create text at where character get heal
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, VFXcanvas).GetComponent<TMP_Text>();

        tmpText.text = ("+" + healthRestored.ToString());
    }

    public void ShowHitSplash(GameObject character, Vector2 hitDirection, int attackType)
    {
        Vector3 spawnPosition = character.transform.position;

        GameObject hitSplash;

        if (character.tag == "Player")
        {
            hitSplash = Instantiate(playerHitSplashPrefab[attackType], spawnPosition, Quaternion.identity, VFXcanvas);
        }
        else
        {
            hitSplash = Instantiate(nonPlayerHitSplashPrefab[attackType], spawnPosition, Quaternion.identity, VFXcanvas);
        }

        if (hitSplash != null)
        {
            if (hitDirection.x < 0)
            {
                hitSplash.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                hitSplash.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public void OpenInventoryMenu(InputAction.CallbackContext context)
    {
        if (context.started && !menuActivated)
        {
            Time.timeScale = 0;
            inventoryMenu.SetActive(true);
            menuActivated = true;
        }
        else if (context.started && menuActivated)
        {
            Time.timeScale = 1;
            inventoryMenu.SetActive(false);
            menuActivated = false;
        }
    }
}
