using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSplashManager : MonoBehaviour
{
    [SerializeField] private GameObject[] hitSplashPrefab;

    public void ShowHitSplash(Vector3 position, Vector2 hitDirection, int attackType)
    {
        GameObject bloodSplash = Instantiate(hitSplashPrefab[attackType], position, Quaternion.identity);

        // Determine the direction and flip the sprite
        if (hitDirection.x < 0)
        {
            bloodSplash.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            bloodSplash.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        // Destroy the blood splash after a short time
        Destroy(bloodSplash, 0.35f); 
    }

}
