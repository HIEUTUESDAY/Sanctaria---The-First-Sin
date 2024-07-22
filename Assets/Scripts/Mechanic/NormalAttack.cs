using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage = 10f;
    private float damageBuff = 0f;
    [SerializeField] private Vector2 knockback = Vector2.zero;
    [SerializeField] private int attackType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            IDamageableBase damageable = collision.GetComponent<IDamageableBase>();
            
            if (transform.parent.gameObject.CompareTag("Player"))
            {
                damageBuff = transform.parent.GetComponent<Player>().damageBuff;
            }

            if (damageable != null && damageable.IsAlive)
            {
                Vector2 deliveredKnockback = transform.parent.rotation.y == 0 ? knockback : new Vector2(-knockback.x, knockback.y);
                Vector2 hitDirection = (collision.transform.position - transform.position).normalized;

                damageable.TakeDamage(attackDamage + damageBuff, deliveredKnockback, hitDirection, attackType);
            }
        }
    }
}
