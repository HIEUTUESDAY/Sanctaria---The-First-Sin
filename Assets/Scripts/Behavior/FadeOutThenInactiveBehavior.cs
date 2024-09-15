using UnityEngine;

public class FadeOutThenInactiveBehavior : StateMachineBehaviour
{
    [SerializeField] private float fadeOutDuration = 3f; 
    private float elapsedTime = 0f; 
    private bool isFading = false; 
    private SpriteRenderer spriteRenderer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        elapsedTime = 0f;
        isFading = false;

        spriteRenderer = animator.gameObject.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 1.0f && !isFading)
        {
            isFading = true;
            Player.Instance.isDefeatedBoss = true;
        }

        if (isFading && spriteRenderer != null)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

            if (elapsedTime >= fadeOutDuration)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);

                animator.gameObject.SetActive(false);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
