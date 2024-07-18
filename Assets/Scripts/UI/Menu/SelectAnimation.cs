using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectAnimation : MonoBehaviour
{
    public Button button;
    public GameObject shaderObject;
    private QuestItemSlot itemSlot;
    private Image shaderImage;
    public Sprite[] spriteArray;
    public float speed = 0.05f;
    private int indexSprite;
    private float timer;

    private void Start()
    {
        itemSlot = GetComponent<QuestItemSlot>();
        shaderImage = shaderObject.GetComponent<Image>();
        shaderObject.SetActive(false); // Ensure the shaderObject is initially inactive
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == button.gameObject)
        {
            shaderObject.SetActive(true);
            UpdateHighlight();
        }
        else
        {
            shaderObject.SetActive(false);
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
