using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SelectAnimation : MonoBehaviour
{
    public Button button;
    public GameObject shaderObject;
    private Image shaderImage;
    public Sprite emmptySprite;
    public Sprite[] spriteArray;
    public float speed = 0.05f;
    private int indexSprite;
    private float timer;

    public TMP_Text buttonTmpText;
    public TMP_ColorGradient selectedColorGradient;
    public TMP_ColorGradient deselectedColorGradient;

    private void Start()
    {
        shaderImage = shaderObject.GetComponent<Image>();
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == button.gameObject)
        {
            UpdateHighlight();
            buttonTmpText.colorGradientPreset = selectedColorGradient;
        }
        else
        {
            shaderImage.sprite = emmptySprite;
            buttonTmpText.colorGradientPreset = deselectedColorGradient;
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
