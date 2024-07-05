using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderAttack : MonoBehaviour
{
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private Vector2 knockback = Vector2.zero;
    [SerializeField] private float damageCooldown = 3f;
    [SerializeField] private int attackType;
    private bool canDealDamage = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canDealDamage)
        {
            // See if it can be hit
            IDamageableBase damageable = collision.GetComponent<IDamageableBase>();

            if (damageable != null)
            {
                Vector2 deliveredKnockback = transform.parent.rotation.y == 0 ? knockback : new Vector2(-knockback.x, knockback.y);

                // Calculate hit direction
                Vector2 hitDirection = (collision.transform.position - transform.position).normalized;

                // Hit the target
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
