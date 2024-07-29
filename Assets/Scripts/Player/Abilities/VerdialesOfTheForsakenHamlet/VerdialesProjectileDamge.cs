using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerdialesProjectileDamge : MonoBehaviour
{
    [SerializeField] private float damage = 50f;
    [SerializeField] private float damageInterval = 0.5f;
    [SerializeField] private Vector2 knockback = Vector2.zero;
    [SerializeField] private int attackType;

    private bool isDealingDamage = false;
    private Collider2D hitCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            IDamageableBase damageable = collision.GetComponent<IDamageableBase>();

            if (damageable != null && damageable.IsAlive && !isDealingDamage)
            {
                hitCollider = collision;
                StartCoroutine(DealContinuousDamage(damageable, collision));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && hitCollider != null && collision == hitCollider)
        {
            StopCoroutine(DealContinuousDamage(null, null));
            isDealingDamage = false;
            hitCollider = null;
        }
    }

    private IEnumerator DealContinuousDamage(IDamageableBase damageable, Collider2D collider)
    {
        isDealingDamage = true;

        while (isDealingDamage && damageable != null && damageable.IsAlive)
        {
            Vector2 deliveredKnockback = transform.rotation.z == 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            Vector2 hitDirection = (collider.transform.position - transform.position).normalized;

            damageable.TakeDamage(damage, deliveredKnockback, hitDirection, attackType);
            yield return new WaitForSeconds(damageInterval);
        }

        isDealingDamage = false;
    }
}
