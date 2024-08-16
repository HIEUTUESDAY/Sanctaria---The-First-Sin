using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightAnimation : MonoBehaviour
{
    public GameObject shaderObject;
    private Image shaderImage;
    public Sprite[] spriteArray;
    public float speed = 0.05f;
    private int indexSprite;
    private float timer;

    private void Start()
    {
        shaderImage = shaderObject.GetComponent<Image>();
    }

    private void Update()
    {
        if (shaderObject.activeSelf)
        {
            UpdateHighlight();
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
