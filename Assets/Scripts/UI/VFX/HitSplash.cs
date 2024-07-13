using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitSplash : MonoBehaviour
{
    public Animator animator;
    public string animationName;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f)
        {
            Destroy(gameObject);
        }
    }
}

