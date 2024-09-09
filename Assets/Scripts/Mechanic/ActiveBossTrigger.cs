using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBossTrigger : MonoBehaviour
{
    [SerializeField] private BossTenPiedad bossTenPiedad;
    [SerializeField] private GameObject bossGround;
    [SerializeField] private Collider2D bossCameraBoundary;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (bossTenPiedad.StateMachine.CurrentEnemyState == bossTenPiedad.IdleState)
            {
                TenPiedadIdle tenPiedadIdle = bossTenPiedad.EnemyIdleBaseInstance as TenPiedadIdle;

                if (tenPiedadIdle != null)
                {
                    tenPiedadIdle.hasTarget = true;

                    bossGround.SetActive(true);

                    CinemachineVirtualCamera activeCamera = FindObjectOfType<CinemachineVirtualCamera>();

                    if (activeCamera != null)
                    {
                        CinemachineConfiner2D confiner = activeCamera.GetComponent<CinemachineConfiner2D>();

                        if (confiner != null)
                        {
                            confiner.m_BoundingShape2D = bossCameraBoundary;
                        }
                    }

                    Destroy(gameObject);
                }
            }
        }
    }
}
