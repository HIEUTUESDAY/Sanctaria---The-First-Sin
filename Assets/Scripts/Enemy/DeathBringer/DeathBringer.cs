using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class DeathBringer : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float minStopTime = 3f;
    [SerializeField] private float maxStopTime = 9f;
    [SerializeField] private float facingSpotRange = 20f;
    [SerializeField] private float behindSpotRange = 2f;
    [SerializeField] private float chaseDuration = 2f;

    private float chaseTimer;
    private float stopTimeRemaining; 
    private bool isFlipping;
    private bool waitForFlip;
    private Vector3 lastPlayerPosition;

    [SerializeField] private DetectionZone attackZone;
    [SerializeField] private DetectionZone groundZone;
    [SerializeField] Transform facingSpotPoint;
    [SerializeField] Transform behindSpotPoint;
    [SerializeField] private GameObject bodyHitZone;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    Damageable damageable;
    Transform playerTransform;

    private enum WalkableDirection { Right, Left }

    private WalkableDirection _walkDirection;
    private Vector2 WalkDirectionVector = Vector2.right;

    private WalkableDirection WalkDirection
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
                    IsFacingRight = !IsFacingRight;
                }
                else if(value == WalkableDirection.Left)
                {
                    // If currently facing right, rotate 180 degrees to face left
                    Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
                    transform.rotation = Quaternion.Euler(rotator);
                    WalkDirectionVector = Vector2.left;
                    IsFacingRight = !IsFacingRight;
                }
            }
            _walkDirection = value; }
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            _isFacingRight = value;
        }
    }

    public bool _spotTarget = false;

    public bool SpotTarget
    {
        get { return _spotTarget; }
        private set
        {
            _spotTarget = value;
            animator.SetBool(AnimationString.spotTarget, value);
            if (value)
            {
                chaseTimer = chaseDuration;
                lastPlayerPosition = playerTransform.position;
                if (isFlipping)
                {
                    isFlipping = false;
                    CanMove = true;
                }
            }
        }
    }

    public bool _isMoving = false;

    public bool IsMoving
    {
        get { return _isMoving; }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationString.isMoving, value);
        }
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
        this.GetPlayerTranform();
    }

    void Update()
    {
        this.SpotTargetDetection(facingSpotRange, behindSpotRange);
        this.HasTargetDetection();
        this.AttackDelay();
        this.ChasingDurtionCounter();
    }

    private void FixedUpdate()
    {
        if (chaseTimer > 0)
        {
            this.ChasingTarget();
        }
        else
        {
            this.Move();
        }

        this.WallHitFlip();
        this.NoGroundFlip();
        this.FlippingCheck();
    }

    private void Move()
    {
        if (!damageable.LockVelocity)
        {
            if (CanMove && !HasTarget && groundZone.detectedCols.Count > 0)
            {
                rb.velocity = new Vector2(walkSpeed * WalkDirectionVector.x, rb.velocity.y);
                IsMoving = true;
            }
            else
            {
                rb.velocity = Vector2.zero;
                IsMoving = false;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            IsMoving = false;
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

    private void HandleFlip(bool waitBeforeFlip)
    {
        if (SpotTarget)
        {
            return;
        }

        CanMove = false;
        stopTimeRemaining = UnityEngine.Random.Range(minStopTime, maxStopTime);
        isFlipping = true;

        if (waitBeforeFlip)
        {
            waitForFlip = true;
        }
        else if(!waitBeforeFlip)
        {
            Flip();
        }
    }

    private void WallHitFlip()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall && !HasTarget && !isFlipping)
        {
            HandleFlip(false); // Flip first, then wait
        }
    }

    private void NoGroundFlip()
    {
        if (groundZone.detectedCols.Count <= 0 && touchingDirections.IsGrounded && !HasTarget && !isFlipping)
        {
            HandleFlip(true); // Wait first, then flip
        }
    }

    private void FlippingCheck()
    {
        if (isFlipping && !SpotTarget)
        {
            stopTimeRemaining -= Time.fixedDeltaTime;
            if (stopTimeRemaining <= 0)
            {
                if (waitForFlip)
                {
                    Flip();
                }

                CanMove = true;
                isFlipping = false;
                waitForFlip = false;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void SpotTargetDetection(float facingDistance, float behindDistance)
    {
        float facingCastDistance;
        float behindCastDistance;

        if (IsFacingRight)
        {
            facingCastDistance = facingDistance;
            behindCastDistance = behindDistance;
        }
        else
        {
            facingCastDistance = -facingDistance;
            behindCastDistance = -behindDistance;
        }

        Vector2 facingEndPos = facingSpotPoint.position + Vector3.right * facingCastDistance;
        Vector2 behindEndPos = behindSpotPoint.position + Vector3.left * behindCastDistance;
        RaycastHit2D facingHitTarget = Physics2D.Linecast(facingSpotPoint.position, facingEndPos, LayerMask.GetMask("Ground", "Player"));
        RaycastHit2D behindHitTarget = Physics2D.Linecast(behindSpotPoint.position, behindEndPos, LayerMask.GetMask("Ground", "Player"));

        if (facingHitTarget.collider != null)
        {
            if (facingHitTarget.collider.gameObject.CompareTag("Player"))
            {
                SpotTarget = true;
                Debug.DrawLine(facingSpotPoint.position, facingHitTarget.point, Color.green);
            }
            else
            {
                SpotTarget = false;
                Debug.DrawLine(facingSpotPoint.position, facingHitTarget.point, Color.blue);
            }
        }
        else
        {
            Debug.DrawLine(facingSpotPoint.position, facingEndPos, Color.red);
        }

        if (behindHitTarget.collider != null)
        {
            if (behindHitTarget.collider.gameObject.CompareTag("Player"))
            {
                SpotTarget = true;
                Debug.DrawLine(behindSpotPoint.position, behindHitTarget.point, Color.green);
                if (!damageable.LockVelocity)
                {
                    this.Flip();
                }
            }
            else
            {
                SpotTarget = false;
                Debug.DrawLine(behindSpotPoint.position, behindHitTarget.point, Color.blue);
            }
        }
        else
        {
            Debug.DrawLine(behindSpotPoint.position, behindEndPos, Color.red);
        }

    }

    private void HasTargetDetection()
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

    private void ChasingTarget()
    {
        if (playerTransform != null && (SpotTarget || chaseTimer > 0))
        {
            Vector3 targetPosition = SpotTarget ? playerTransform.position : lastPlayerPosition;
            Vector2 directionToTarget = targetPosition - transform.position;

            bool isTargetBehind = (WalkDirection == WalkableDirection.Right && directionToTarget.x < 0) ||
                                  (WalkDirection == WalkableDirection.Left && directionToTarget.x > 0);

            if (!damageable.LockVelocity)
            {
                if (CanMove && !HasTarget && groundZone.detectedCols.Count > 0)
                {
                    Vector2 direction = directionToTarget.normalized;
                    rb.velocity = new Vector2(chaseSpeed * direction.x, rb.velocity.y);
                    IsMoving = true;

                    if (isTargetBehind)
                    {
                        Flip();
                    }
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    IsMoving = false;
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
                IsMoving = false;
            }
        }
    }

    private void ChasingDurtionCounter()
    {
        if (!SpotTarget)
        {
            if (chaseTimer > 0)
            {
                Vector2 direction = (lastPlayerPosition - transform.position).normalized;
                rb.velocity = new Vector2(chaseSpeed * direction.x, rb.velocity.y);
                chaseTimer -= Time.deltaTime;
            }
            else
            {
                lastPlayerPosition = Vector3.zero;
            }
        }
    }

    private void GetPlayerTranform()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {

            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object not found in the scene!");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        if (playerTransform != null)
        {
            Vector2 direction = transform.position - playerTransform.position;
            direction.Normalize();

            rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);

            if (direction.x > 0 && !animator.GetBool(AnimationString.onAttack))
            {
                WalkDirection = WalkableDirection.Left;
            }
            else if (direction.x < 0 && !animator.GetBool(AnimationString.onAttack))
            {
                WalkDirection = WalkableDirection.Right;
            }
        }
    }

    public void OnDeath()
    {
        bodyHitZone.SetActive(false);
    }
}
