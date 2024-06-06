using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private Vector2 knockback = Vector2.zero;

    [SerializeField] private int attackType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // See if it can be hit
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.parent.rotation.y == 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            // Calculate hit direction
            Vector2 hitDirection = (collision.transform.position - transform.position).normalized;

            // Hit the target
            bool hitTarget = damageable.Hit(attackDamage, deliveredKnockback, hitDirection, attackType);
        }
    }
}
