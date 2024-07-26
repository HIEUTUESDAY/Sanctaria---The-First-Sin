using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopUpMessage : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float displayDuration = 3f;

    private CanvasGroup canvasGroup;
    public Image image;
    public TMP_Text message;

    private void Start()
    {
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        image = GetComponentInChildren<Image>();
        message = GetComponentInChildren<TMP_Text>();

        canvasGroup.alpha = 0;
        StartCoroutine(ShowMessage());
    }

    private IEnumerator ShowMessage()
    {
        yield return FadeIn();

        yield return new WaitForSeconds(displayDuration);

        yield return FadeOut();

        Destroy(gameObject);
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }
}
