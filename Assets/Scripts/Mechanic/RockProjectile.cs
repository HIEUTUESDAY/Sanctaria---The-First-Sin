using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockProjectile : MonoBehaviour
{
    public float throwForce = 10f;
    public float gravity = -9.8f;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private Vector3 velocity;
    private bool isThrown = false;

    [SerializeField] private int attackDamage = 10;
    [SerializeField] private Vector2 knockback = Vector2.zero;
    [SerializeField] private int attackType;

    private void Start()
    {
        Initialize(Player.Instance.transform.position);
    }

    private void Update()
    {
        if (isThrown)
        {
            velocity.y += gravity * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
        }
    }

    public void Initialize(Vector3 playerPosition)
    {
        startPosition = transform.position;
        targetPosition = playerPosition;
        CalculateTrajectory();
        isThrown = true;
    }

    private void CalculateTrajectory()
    {
        Vector3 direction = targetPosition - startPosition;
        float horizontalDistance = new Vector3(direction.x, 0, direction.z).magnitude;
        float verticalDistance = direction.y;
        float time = horizontalDistance / throwForce;
        float initialVerticalVelocity = (verticalDistance - 0.5f * gravity * time * time) / time;
        Vector3 horizontalVelocity = new Vector3(direction.x, 0, direction.z).normalized * throwForce;
        velocity = new Vector3(horizontalVelocity.x, initialVerticalVelocity, horizontalVelocity.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            IDamageableBase damageable = collision.GetComponent<IDamageableBase>();

            if (damageable != null && damageable.IsAlive)
            {
                Vector2 deliveredKnockback = transform.rotation.y == 0 ? knockback : new Vector2(-knockback.x, knockback.y);
                Vector2 hitDirection = (collision.transform.position - transform.position).normalized;

                damageable.TakeDamage(attackDamage, deliveredKnockback, hitDirection, attackType);
            }

            isThrown = false;
            Destroy(gameObject);
        }
    }
}
