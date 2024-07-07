using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointManager : MonoBehaviour
{
    private Animator animator;
    private EnemyManager enemyManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyManager = FindObjectOfType<EnemyManager>();
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
            }
        }
    }

    public void SaveCheckPoint()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            Checkpoint checkpoint = new Checkpoint(SceneManager.GetActiveScene().name, transform.position);
            gameManager.SaveGame(player, checkpoint);
        }
    }

    public void RespawnEnemiesAfterSpawn()
    {
        enemyManager.RespawnAllEnemies();
    }

    public void ActivateCheckPoint()
    {
        if (IsActivate == false)
        {
            IsActivate = true;
        }
    }
}
