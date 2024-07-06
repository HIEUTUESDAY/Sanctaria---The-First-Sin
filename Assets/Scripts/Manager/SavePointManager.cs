using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePointManager : MonoBehaviour
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

    public void SaveGame()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            Checkpoint checkpoint = new Checkpoint(SceneManager.GetActiveScene().name, transform.position);
            gameManager.SetCurrentCheckpoint(checkpoint);
            gameManager.SaveGame(player);
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
