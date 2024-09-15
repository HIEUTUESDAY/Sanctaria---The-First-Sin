using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DefeatBossFadeInTitle : MonoBehaviour
{
    public Image backGroundImage;
    public Image titleImage;
    [Range(0.1f, 10f), SerializeField] private float imageFadeInSpeed = 1f;
    [Range(0.1f, 10f), SerializeField] private float imageFadeOutSpeed = 1f;
    [SerializeField] private float waitTimeBeforeFadeOut = 3f;

    private void Start()
    {
        SetInitialAlphaValues();
    }

    private void Update()
    {
        if (Player.Instance.isDefeatedBoss)
        {
            StartCoroutine(FadeInAndOutSequence());
            Player.Instance.isDefeatedBoss = false;
        }
    }

    public void SetInitialAlphaValues()
    {
        backGroundImage.color = SetAlpha(backGroundImage.color, 0f);
        titleImage.color = SetAlpha(titleImage.color, 0f);
    }

    private Color SetAlpha(Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }

    private IEnumerator FadeInAndOutSequence()
    {
        UIManager.Instance.tenPiedadHealthBar.SetActive(false);

        GameObject bossGround = GameObject.FindGameObjectWithTag("BossGround");

        if (bossGround != null)
        {
            bossGround.SetActive(false);
        }

        CinemachineVirtualCamera activeCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (activeCamera != null)
        {
            CinemachineConfiner2D confiner = activeCamera.GetComponent<CinemachineConfiner2D>();

            if (confiner != null)
            {
                confiner.m_BoundingShape2D = GameObject.FindGameObjectWithTag("CameraBoundary").GetComponent<Collider2D>();
            }
        }

        yield return StartCoroutine(FadeInImages());

        yield return new WaitForSeconds(waitTimeBeforeFadeOut);

        yield return StartCoroutine(FadeOutImages());
    }

    private IEnumerator FadeInImages()
    {
        while (backGroundImage.color.a < 1f || titleImage.color.a < 1f)
        {
            backGroundImage.color = FadeToAlpha(backGroundImage.color, imageFadeInSpeed);
            titleImage.color = FadeToAlpha(titleImage.color, imageFadeInSpeed);
            yield return null;
        }
    }

    private IEnumerator FadeOutImages()
    {
        while (backGroundImage.color.a > 0f || titleImage.color.a > 0f)
        {
            Player.Instance.PlayerInput.enabled = true;
            backGroundImage.color = FadeToAlpha(backGroundImage.color, -imageFadeOutSpeed);
            titleImage.color = FadeToAlpha(titleImage.color, -imageFadeOutSpeed);
            yield return null;
        }
    }

    private Color FadeToAlpha(Color color, float fadeSpeed)
    {
        color.a += Time.deltaTime * fadeSpeed;
        color.a = Mathf.Clamp01(color.a);
        return color;
    }
}
