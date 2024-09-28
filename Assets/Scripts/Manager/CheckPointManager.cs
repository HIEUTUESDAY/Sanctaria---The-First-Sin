using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    [Header("This scene DATA")]
    [SerializeField] private SceneData thisSceneData;
    [Space(5)]

    [Header("This checkpoint Area")]
    [SerializeField] private CheckpointArea checkpointArea;

    [SerializeField] private GameObject interactIcon;

    public enum CheckpointArea
    {
        Forest,
        Castle,
        Dungeon,
        Village,
        Mountain
    }

    private Animator animator;
    [SerializeField] private bool isActivated;

    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(AnimationString.isActivated, isActivated);
    }

    public void ActiveCheckpoint()
    {
        isActivated = true;
        StartCoroutine(SaveCheckpointAndRespawnEnemies());
    }

    public CheckPointData SaveCheckpoint()
    {
        CheckPointData checkPointData = new CheckPointData();

        checkPointData.isActived = isActivated;
        checkPointData.position = new float[] { gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z };

        return checkPointData;
    }

    public void LoadCheckpoint(string currentScene)
    {
        List<SceneData> sceneDataList = SceneDataManager.Instance.sceneDataList;

        foreach (SceneData sceneData in sceneDataList)
        {
            if (currentScene.Equals(sceneData.sceneName))
            {
                thisSceneData = sceneData;
                SetActiveCheckpoint();
                return;
            }
        }
    }

    private void SetActiveCheckpoint()
    {
        if (thisSceneData != null)
        {
            isActivated = thisSceneData.checkPoint.isActived;
        }
        else
        {
            isActivated = false;
        }
    }

    public IEnumerator SaveCheckpointAndRespawnEnemies()
    {
        while (!thisSceneData.checkPoint.isActived)
        {
            yield return null;
        }

        GameManager gameManager = GameManager.Instance;
        SceneDataManager sceneDataManager = SceneDataManager.Instance;
        sceneDataManager.SaveSceneData(SceneManager.GetActiveScene().name);

        if (gameManager != null && sceneDataManager != null)
        {
            PlayerCheckpointData playerCheckpointData = new PlayerCheckpointData( checkpointArea.ToString(), SceneManager.GetActiveScene().name, transform.position);
            gameManager.gameData.playerCheckpointData = playerCheckpointData;
            sceneDataManager.RespawnEnemiesInAllScenes();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactIcon.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactIcon.gameObject.SetActive(false);
        }
    }
}

