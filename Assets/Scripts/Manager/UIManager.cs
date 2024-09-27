using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public GameObject[] playerHitSplashPrefab;
    public GameObject[] nonPlayerHitSplashPrefab;
    public GameObject notEnoughRelicPointsMessagePrefab;
    public GameObject collectedMessagePrefab;
    public GameObject inventoryMenu;
    public GameObject mapMenu;
    public GameObject optionsMenu;
    public GameObject checkpointMenu;
    public GameObject shopMenu;
    public bool menuActivated = false;
    [SerializeField] private Transform VFXcanvas;

    public GameObject bossHealthBar;
    public Slider bossHealthSlider;
    public Slider bossHealthDecreasedSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        CharacterEvent.characterDamaged += CharacterTookDamage;
        CharacterEvent.characterHealed += CharacterHealed;
        CharacterEvent.hitSplash += ShowHitSplash;
        CharacterEvent.collectMessage += ShowCollectMessage;
        CharacterEvent.notEnoughMessage += ShowNotEnoughMessage;
    }

    private void OnDisable()
    {
        CharacterEvent.characterDamaged -= CharacterTookDamage;
        CharacterEvent.characterHealed -= CharacterHealed;
        CharacterEvent.hitSplash -= ShowHitSplash;
        CharacterEvent.collectMessage -= ShowCollectMessage;
        CharacterEvent.notEnoughMessage -= ShowNotEnoughMessage;
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

    public void ShowCollectMessage(Sprite itemImage, string itemName)
    {
        RectTransform rectTransform = collectedMessagePrefab.GetComponent<RectTransform>();
        Vector3 spawnPosition = rectTransform.anchoredPosition;

        PopUpMessage popUpMessage = collectedMessagePrefab.GetComponent<PopUpMessage>();
        popUpMessage.image.sprite = itemImage;
        popUpMessage.message.text = itemName;

        GameObject instantiatedMessage = Instantiate(collectedMessagePrefab, VFXcanvas);
        RectTransform instantiatedRectTransform = instantiatedMessage.GetComponent<RectTransform>();
        instantiatedRectTransform.anchoredPosition = spawnPosition;
    }

    public void ShowNotEnoughMessage()
    {
        RectTransform rectTransform = notEnoughRelicPointsMessagePrefab.GetComponent<RectTransform>();
        Vector3 spawnPosition = rectTransform.anchoredPosition;

        GameObject instantiatedMessage = Instantiate(notEnoughRelicPointsMessagePrefab, VFXcanvas);
        RectTransform instantiatedRectTransform = instantiatedMessage.GetComponent<RectTransform>();
        instantiatedRectTransform.anchoredPosition = spawnPosition;
    }
}
