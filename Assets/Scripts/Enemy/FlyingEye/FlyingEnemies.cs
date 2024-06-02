using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemies : MonoBehaviour
{
    [SerializeField] private float flightSpeed = 2f;
    [SerializeField] private float waypointReachedDistance = 0.01f;
    public DetectionZone biteDetectionZone;
    public Collider2D deathCollider;
    public List<Transform> waypoints;

    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;

    Transform nextWaypoint;
    int waypointNum = 0;

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationString.hasTarget, value);
        }
    }
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
        this.TargetDetection();
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
        if (distance <= waypointReachedDistance)
        {
            // Switch to the next waypoint
            waypointNum++;

            if (waypointNum >= waypoints.Count)
            {
                // Loop baak to the first waypoint
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void TargetDetection()
    {
        HasTarget = biteDetectionZone.detectedCols.Count > 0;
    }

    private void UpdateDirection()
    {
        Vector3 localScale = transform.localScale;
        if (transform.localScale.x > 0)
        {
            // Facing right
            if (rb.velocity.x < 0)
            {
                // Flip 
                transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
            }
        }
        else
        {
            // Facing left
            if (rb.velocity.x > 0)
            {
                // Flip 
                transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
            }
        }
    }

    public void OnDeath()
    {
        // Fall down when is dead
        rb.gravityScale = 2f;
        rb.velocity = new Vector2(0, rb.velocity.y);
        deathCollider.enabled = true;
    }
}
