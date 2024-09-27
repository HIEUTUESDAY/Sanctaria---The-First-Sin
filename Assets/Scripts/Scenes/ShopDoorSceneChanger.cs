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

    public void ChangeScene()
    {
        SceneChangerManager.Instance.ChangeSceneFromShop(sceneToLoad);
    }
}
