using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private LevelConnection connection;

    [SerializeField] private string targetSenceName;

    [SerializeField] private Transform spawnPosition;

    private void Start()
    {
        if(connection == LevelConnection.ActiveConnection)
        {
            FindObjectOfType<PlayerController>().transform.position = spawnPosition.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<PlayerController>();

        if (player != null)
        {
            LevelConnection.ActiveConnection = connection;
            SceneManager.LoadScene(targetSenceName);
        }
        
    }
}
