using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class DeathBringer : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float minStopTime = 3f;
    [SerializeField] private float maxStopTime = 9f;

    public DetectionZone attackZone;
    public DetectionZone groundZone;
    public DetectionZone spotZone;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    Damageable damageable;
    Transform playerTransform;

    public enum WalkableDirection { Right, Left }

    private WalkableDirection _walkDirection;
    private Vector2 WalkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set {
            if (_walkDirection != value)
            {
                if (value == WalkableDirection.Right)
                {
                    // If currently facing left, rotate 180 degrees to face right
                    Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
                    transform.rotation = Quaternion.Euler(rotator);
                    WalkDirectionVector = Vector2.right;
                }
                else if(value == WalkableDirection.Left)
                {
                    // If currently facing right, rotate 180 degrees to face left
                    Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
                    transform.rotation = Quaternion.Euler(rotator);
                    WalkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value; }
    }

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

    public bool _canMove = true;
    public bool CanMove
    {
        get
        {
            return _canMove;
        }
        private set
        {
            _canMove = value;
            animator.SetBool(AnimationString.canMove, value);
        }
    }

    public float AttackCooldown 
    {
        get
        {
            return animator.GetFloat(AnimationString.attackCooldown);
        } 
        private set
        {
            animator.SetFloat(AnimationString.attackCooldown, Mathf.Max(value, 0));
        } 
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        // Find the player object by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // Check if the player object is found
        if (playerObject != null)
        {
            // Get the transform component of the player
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object not found in the scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.TargetDetection();
        this.AttackDelay();
    }

    private void FixedUpdate()
    {
        this.Move();
        this.ChangeDirection();
    }

    private void Move()
    {
        if (!damageable.LockVelocity)
        {
            if (_canMove)
            {
                rb.velocity = new Vector2(walkSpeed * WalkDirectionVector.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void Flip()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Current walkable direction is not set to a legal values of right or left");
        }
    }

    private void ChangeDirection()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall && damageable.LockVelocity == false)
        {
            CoroutineManager.Instance.StartManagedCoroutine(HitWallFlip());
        }
    }

    private void TargetDetection()
    {
        HasTarget = attackZone.detectedCols.Count > 0;
    }

    private void AttackDelay()
    {
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        if (playerTransform != null)
        {
            // Calculate the direction from the enemy to the player
            Vector2 direction = transform.position - playerTransform.position;
            direction.Normalize();

            rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);

            // Ensure that the enemy turn back when get hit from behind and not in attacking
            if (direction.x > 0 && !animator.GetBool(AnimationString.onAttack))
            {
                WalkDirection = WalkableDirection.Left;
            }
            else if (direction.x < 0 && !animator.GetBool(AnimationString.onAttack))
            {
                WalkDirection = WalkableDirection.Right;
            }
        }
        else
        {
            Debug.LogError("Player transform is null!");
        }
    }

    public void OnNoGroundDetected()
    {
        if (touchingDirections.IsGrounded)
        {
            CoroutineManager.Instance.StartManagedCoroutine(NoGroundFlip());
        }
    }

    private IEnumerator HitWallFlip()
    {
        Flip();
        CanMove = false;
        float stopTime = UnityEngine.Random.Range(minStopTime, maxStopTime);

        yield return new WaitForSeconds(stopTime);
        CanMove = true;
    }

    private IEnumerator NoGroundFlip()
    {
        CanMove = false;
        float stopTime = UnityEngine.Random.Range(minStopTime, maxStopTime);

        yield return new WaitForSeconds(stopTime);
        CanMove = true;
        Flip();
    }
}
