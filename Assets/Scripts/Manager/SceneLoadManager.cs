using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance;

    [SerializeField] private Image fadeOutImage;
    [Range(0.1f, 10f), SerializeField] private float fadeOutSpeed = 2f;
    [Range(0.1f, 10f), SerializeField] private float fadeInSpeed = 1f;
    [SerializeField] private Color fadeOutStartColor;

    public bool IsFadingOut { get; private set; }
    public bool IsLoading { get; private set; }
    public bool IsFadingIn { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        fadeOutStartColor.a = 0f;
    }

    private void Update()
    {
        if (IsFadingOut)
        {
            if (fadeOutImage.color.a < 1f)
            {
                fadeOutStartColor.a += Time.deltaTime * fadeOutSpeed;
                fadeOutImage.color = fadeOutStartColor;
            }
            else
            {
                IsFadingOut = false;
            }
        }

        if (IsFadingIn)
        {
            if (fadeOutImage.color.a > 0f)
            {
                fadeOutStartColor.a -= Time.deltaTime * fadeInSpeed;
                fadeOutImage.color = fadeOutStartColor;
            }
            else
            {
                IsFadingIn = false;
            }
        }
    }

    private IEnumerator WaitBeforeFadeIn(float loadTime)
    {
        IsLoading = true;
        yield return new WaitForSeconds(loadTime);
        IsLoading = false;
    }

    public void StartFadeOut()
    {
        fadeOutImage.color = fadeOutStartColor;
        IsFadingOut = true;
    }

    public void StartLoading(float loadTime)
    {
        StartCoroutine(WaitBeforeFadeIn(loadTime));
    }

    public void StartFadeIn()
    {
        if (fadeOutImage.color.a >= 1f)
        {
            fadeOutImage.color = fadeOutStartColor;
            IsFadingIn = true;
        }
    }
}
