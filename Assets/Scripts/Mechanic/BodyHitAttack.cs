using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHitAttack : MonoBehaviour
{
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private Vector2 knockback = Vector2.zero;
    [SerializeField] private float damageCooldown = 3f;
    [SerializeField] private int attackType;
    private bool canDealDamage = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            IDamageableBase damageable = collision.GetComponent<IDamageableBase>();

            if (damageable != null && damageable.IsAlive && canDealDamage)
            {
                Vector2 deliveredKnockback = transform.parent.rotation.y == 0 ? knockback : new Vector2(-knockback.x, knockback.y);
                Vector2 hitDirection = (collision.transform.position - transform.position).normalized;

                damageable.TakeDamage(attackDamage, deliveredKnockback, hitDirection, attackType);
                StartCoroutine(DamageCooldown());
            }
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDealDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDealDamage = true;
    }
}
