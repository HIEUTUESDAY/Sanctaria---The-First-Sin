using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ShopDoorSceneChanger : MonoBehaviour
{
    [Header("Scene To Spawn")]
    [SerializeField] private SceneField sceneToLoad;

    [Space(10f)]
    [Header("This Scene")]
    public Transform spawnPosition;

    [SerializeField] private GameObject interactIcon;

    public void ChangeScene()
    {
        SceneChangerManager.Instance.ChangeSceneFromShop(sceneToLoad);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactIcon.SetActive(false);
        }
    }
}
