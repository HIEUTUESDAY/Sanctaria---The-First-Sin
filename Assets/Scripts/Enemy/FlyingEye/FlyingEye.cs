using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    [SerializeField] private float flightSpeed = 5f;
    [SerializeField] private float waypointReachedDistance = 0.01f;

    [SerializeField] private GameObject bodyHitZone;
    [SerializeField] private Collider2D deathCollider;
    [SerializeField] private List<Transform> waypoints;

    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;

    Transform nextWaypoint;
    int waypointNum = 0;


    public bool canMove
    {
        get
        {
            return animator.GetBool(AnimationString.canMove);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (canMove)
            {
                this.Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void Flight()
    {
        // Fly to the next waypoint
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        // Check if have reached the waypoint already
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);
        rb.velocity = directionToWaypoint * flightSpeed;
        this.UpdateDirection();

        // Check if need to swith waypoint
        if(distance <= waypointReachedDistance)
        {
            // Switch to the next waypoint
            waypointNum++;

            if(waypointNum >= waypoints.Count)
            {
                // Loop baak to the first waypoint
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void UpdateDirection()
    {
        if(rb.velocity.x < 0)
        {
            // Flip 
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
        }
        else
        {
            // Flip 
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
        }
    }

    public void OnDeath()
    {
        bodyHitZone.SetActive(false);
        // Fall down when is dead
        rb.gravityScale = 2f;
        rb.velocity = new Vector2(0, rb.velocity.y);
        deathCollider.enabled = true;
    }
}
