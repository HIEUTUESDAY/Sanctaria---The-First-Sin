using System.Collections;
using UnityEngine;

public class VerdialesPrayer : MonoBehaviour
{
    public GameObject[] projectilePrefab; // Reference to the projectile prefab
    public Transform spawnPoint; // Point where the projectile will be spawned

    public void ActivatePrayer()
    {
        StartCoroutine(PrayerRoutine());
    }

    private IEnumerator PrayerRoutine()
    {
        // Spawn projectiles at intervals
        for (int i = 0; i < projectilePrefab.Length; i++)
        {
            Instantiate(projectilePrefab[i], spawnPoint.position, spawnPoint.rotation);
            yield return null;
        }
    }
}
