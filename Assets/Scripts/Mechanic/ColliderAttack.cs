using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderAttack : MonoBehaviour
{
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private Vector2 knockback = Vector2.zero;
    [SerializeField] private float damageCooldown = 3f;
    private bool canDealDamage = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canDealDamage)
        {
            // See if it can be hit
            Damageable damageable = collision.GetComponent<Damageable>();

            if (damageable != null)
            {
                Vector2 deliveredKnockback = transform.parent.rotation.y == 0 ? knockback : new Vector2(-knockback.x, knockback.y);

                // Hit the target
                bool hitTarget = damageable.Hit(attackDamage, deliveredKnockback);

                if (hitTarget)
                {
                    StartCoroutine(DamageCooldown());
                }
            }
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDealDamage = false;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(damageCooldown);
        GetComponent<Collider2D>().enabled = true;
        canDealDamage = true;
    }
}
