using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeInAnimation : MonoBehaviour
{
    public Image backGroundImage;
    public Image titleImage;
    public TMP_Text continueText;
    [Range(0.1f, 10f), SerializeField] private float imageFadeInSpeed = 1f;
    [Range(0.1f, 10f), SerializeField] private float textFadeInSpeed = 0.5f;

    private bool isFadingInImages = false;
    private bool isFadingInText = false;
    private bool waitingForInput = false;

    private Player player;

    private void Start()
    {
        // Set initial alpha values to 0
        SetInitialAlphaValues();

        // Find the player object in the scene
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (isFadingInImages)
        {
            UpdateFadeInImages();
        }

        if (isFadingInText)
        {
            UpdateFadeInText();
        }

        if (waitingForInput && Input.anyKeyDown)
        {
            GameManager.Instance.RespawnPlayer();
            waitingForInput = false;
        }
    }

    public void SetInitialAlphaValues()
    {
        // Set initial alpha values of images and text to 0
        backGroundImage.color = SetAlpha(backGroundImage.color, 0f);
        titleImage.color = SetAlpha(titleImage.color, 0f);
        continueText.color = SetAlpha(continueText.color, 0f);
    }

    private Color SetAlpha(Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }

    public void StartFadeIn()
    {
        isFadingInImages = true;
    }

    private void UpdateFadeInImages()
    {
        // Update alpha of both images simultaneously
        backGroundImage.color = FadeToAlpha(backGroundImage.color, imageFadeInSpeed);
        titleImage.color = FadeToAlpha(titleImage.color, imageFadeInSpeed);

        // Check if fade-in of both images is complete
        if (backGroundImage.color.a >= 1f && titleImage.color.a >= 1f)
        {
            isFadingInImages = false;
            StartFadeInText();
        }
    }

    public void StartFadeInText()
    {
        isFadingInText = true;
    }

    private void UpdateFadeInText()
    {
        // Update alpha of the text
        continueText.color = FadeToAlpha(continueText.color, textFadeInSpeed);

        // Check if fade-in of text is complete
        if (continueText.color.a >= 1f)
        {
            isFadingInText = false;
            waitingForInput = true; // Begin waiting for player input
        }
    }

    private Color FadeToAlpha(Color color, float fadeInSpeed)
    {
        color.a +=  Time.deltaTime * fadeInSpeed;
        return color;
    }
}
