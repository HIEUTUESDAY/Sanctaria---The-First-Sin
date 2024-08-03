using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerManager : MonoBehaviour
{
    public static SceneChangerManager Instance;

    private static bool loadFromDoor;

    private SceneChanger.DoorToSpawnAt doorToSpawnTo;

    private Transform doorSpawnPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static void ChangeSceneFromDoorUse(SceneField myScene, SceneChanger.DoorToSpawnAt doorToSpawnAt)
    {
        loadFromDoor = true;
        Instance.StartCoroutine(Instance.FadeOutThenChangeScene(myScene, doorToSpawnAt));
    }

    private IEnumerator FadeOutThenChangeScene(SceneField myScene, SceneChanger.DoorToSpawnAt doorToSpawnAt = SceneChanger.DoorToSpawnAt.None)
    {
        // run load scene
        Player.Instance.ResetPlayerAnimation();
        Player.Instance.CanMove = false;
        SceneLoadManager.Instance.StartFadeOut();
        // keep loading
        while (SceneLoadManager.Instance.IsFadingOut)
        {
            yield return null;
        }

        doorToSpawnTo = doorToSpawnAt;
        SceneManager.LoadScene(myScene);

    }

    private IEnumerator ActivatePlayerControl()
    {
        while (SceneLoadManager.Instance.IsFadingIn)
        {
            yield return null;
        }

        Player.Instance.CanMove = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneLoadManager.Instance.StartFadeIn();

        if (loadFromDoor)
        {
            StartCoroutine(ActivatePlayerControl());
            // wrap player to correct position
            FindDoor(doorToSpawnTo);
            Player.Instance.transform.position = doorSpawnPosition.position;
            loadFromDoor = false;
        }

        MapRoomManager.Instance.RevealRoom();
    }

    private void FindDoor(SceneChanger.DoorToSpawnAt doorSpawnNumber)
    {
        SceneChanger[] doors = FindObjectsOfType<SceneChanger>();

        for(int i = 0; i < doors.Length; i++)
        {
            if (doors[i].CurrentDoor == doorSpawnNumber)
            {
                doorSpawnPosition = doors[i].spawnPosition.transform;
                return;
            }
        }
    }
}
