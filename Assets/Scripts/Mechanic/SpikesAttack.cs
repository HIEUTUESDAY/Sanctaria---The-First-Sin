using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Player"))
            {
                Player player = collision.GetComponent<Player>();
                if (player != null)
                {
                    player.IsKilledBySpikes = true;
                    player.CurrentHealth -= player.MaxHealth;
                }
            }

            IDamageableBase damageable = collision.GetComponent<IDamageableBase>();

            if (damageable != null && damageable.IsAlive)
            {
                damageable.IsAlive = false;
            }
        }
    }
}
