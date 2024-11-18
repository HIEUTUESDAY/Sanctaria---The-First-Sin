using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderSelectAnimation : MonoBehaviour
{
    public Slider slider;
    public GameObject shaderObject;
    private Image shaderImage;
    public Sprite emmptySprite;
    public Sprite[] spriteArray;
    public float speed = 0.05f;
    private int indexSprite;
    private float timer;

    public TMP_Text textTMP;
    public TMP_ColorGradient selectedColorGradient;
    public TMP_ColorGradient deselectedColorGradient;
    private GameObject lastSelectedItem;

    private void Start()
    {
        shaderImage = shaderObject.GetComponent<Image>();
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == slider.gameObject)
        {
            if (lastSelectedItem != EventSystem.current.currentSelectedGameObject)
            {
                SoundFXManager.Instance.PlayChangeSelectionSound();
                lastSelectedItem = EventSystem.current.currentSelectedGameObject;
            }

            UpdateHighlight();
            textTMP.colorGradientPreset = selectedColorGradient;
        }
        else
        {
            if (lastSelectedItem == slider.gameObject)
            {
                lastSelectedItem = null;
            }
            shaderImage.sprite = emmptySprite;
            textTMP.colorGradientPreset = deselectedColorGradient;
        }
    }

    private void UpdateHighlight()
    {
        timer += Time.unscaledDeltaTime;

        if (timer >= speed)
        {
            timer = 0f;

            if (indexSprite >= spriteArray.Length)
            {
                indexSprite = 0;
            }

            shaderImage.sprite = spriteArray[indexSprite];
            indexSprite += 1;
        }
    }
}
