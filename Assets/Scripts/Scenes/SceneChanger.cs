using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public enum DoorToSpawnAt{
        None,
        One,
        Two,
        Three,
        Four,
        Five
    }

    [Header("Scene To Spawn")]
    [SerializeField] private DoorToSpawnAt DoorToSpawnTo;
    [SerializeField] private SceneField sceneToLoad;

    [Space(10f)]
    [Header("This Scene")]
    public DoorToSpawnAt CurrentDoor;
    public Transform spawnPosition;

    public void ChangeScene()
    {
        SceneChangerManager.Instance.ChangeSceneFromDoor(sceneToLoad, DoorToSpawnTo);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();

        if (player != null)
        {
            PlayerInput playerInput = player.GetComponent<PlayerInput>();
            playerInput.enabled = false;
            ChangeScene();
        }
    }
}
