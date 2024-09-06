using System.Collections;
using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private float ghostSpawnInterval = 0.1f;
    [SerializeField] private float ghostLifeTime = 0.5f;
    [SerializeField] private Color ghostColor = new Color(1f, 1f, 1f, 0.5f);

    private Player player;
    private Coroutine ghostCoroutine;
    [SerializeField] private Transform VFXcanvas;


    private void Start()
    {
        player = GetComponent<Player>();
    }

    public void StartGhostTrail()
    {
        if (ghostCoroutine != null)
        {
            StopCoroutine(ghostCoroutine);
        }
        ghostCoroutine = StartCoroutine(GenerateGhostTrail());
    }

    public void StopGhostTrail()
    {
        if (ghostCoroutine != null)
        {
            StopCoroutine(ghostCoroutine);
            ghostCoroutine = null;
        }
    }

    private IEnumerator GenerateGhostTrail()
    {
        while (player.IsDashing)
        {
            GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation, VFXcanvas);
            SpriteRenderer ghostSpriteRenderer = ghost.GetComponent<SpriteRenderer>();
            ghostSpriteRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
            ghostSpriteRenderer.color = ghostColor;

            StartCoroutine(FadeAndDestroy(ghost, ghostLifeTime));
            yield return new WaitForSeconds(ghostSpawnInterval);
        }
    }

    private IEnumerator FadeAndDestroy(GameObject ghost, float lifetime)
    {
        SpriteRenderer spriteRenderer = ghost.GetComponent<SpriteRenderer>();
        Color initialColor = spriteRenderer.color;
        float elapsedTime = 0f;

        while (elapsedTime < lifetime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(initialColor.a, 0, elapsedTime / lifetime);
            spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        Destroy(ghost);
    }
}
