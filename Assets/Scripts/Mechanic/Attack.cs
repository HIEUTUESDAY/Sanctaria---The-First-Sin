using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private float attackDamage = 10;
    [SerializeField] private Vector2 knockback = Vector2.zero;
    [SerializeField] private int attackType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageableBase damageable = collision.GetComponent<IDamageableBase>();

        if (collision != null)
        {
            Vector2 deliveredKnockback = transform.parent.rotation.y == 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            // Calculate hit direction
            Vector2 hitDirection = (collision.transform.position - transform.position).normalized;

            damageable.TakeDamage(attackDamage, deliveredKnockback, hitDirection, attackType);
        }
    }
}
