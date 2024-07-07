using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private ScriptableLevelConnection connection;
    [SerializeField] private string targetSceneName;
    [SerializeField] private Transform spawnPosition;

    private void Start()
    {
        if (connection == ScriptableLevelConnection.ActiveConnection)
        {
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.transform.position = spawnPosition.position;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();

        if (player != null)
        {
            ScriptableLevelConnection.ActiveConnection = connection;
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
