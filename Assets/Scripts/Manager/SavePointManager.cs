using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SavePointManager : MonoBehaviour
{
    private Animator animator;
    private EnemyManager enemyManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyManager = GetComponent<EnemyManager>();
    }

    private bool isActivate = false;

    public bool IsActivate
    {
        get { return isActivate; }
        set
        {
            isActivate = value;
            if (isActivate)
            {
                animator.SetBool(AnimationString.isActivated, true);
                Debug.Log("Save point activated");
            }
        }
    }

    public void RespawnEnemiesAfterSpawn()
    {
        enemyManager.RespawnAllEnemies();
        Debug.Log("Respawn all enemy");
    }

    public void ActivateSavePoint()
    {
        if (IsActivate == false) 
        {
            IsActivate = true;
        }
    }
}

