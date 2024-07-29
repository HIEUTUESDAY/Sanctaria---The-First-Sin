using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class VerdialesProjectileMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private bool moveForward;

    [SerializeField] private Vector2 topBoxSize;
    [SerializeField] private Vector2 bottomBoxSize;
    [SerializeField] private Vector2 rightBoxSize;
    [SerializeField] private Vector2 leftWallBoxSize;
    [SerializeField] private float topCastDistance;
    [SerializeField] private float bottomCastDistance;
    [SerializeField] private float rightCastDistance;
    [SerializeField] private float leftWallCastDistance;
    private bool topHit;
    private bool bottomHit;
    private bool rightHit;
    private bool leftHit;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Direction direction = Direction.None;
    [SerializeField] private Direction lastDirection = Direction.None;


    private Rigidbody2D RB;
    private Vector2 previousPosition;
    private float traveledDistance = 0f;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        previousPosition = transform.position;
    }

    private void Update()
    {
        // Update the traveled distance
        traveledDistance += Vector2.Distance(previousPosition, transform.position);
        previousPosition = transform.position;

        // Check if the projectile has traveled the max distance
        if (traveledDistance >= maxDistance)
        {
            Destroy(gameObject);
        }

        topHit = Physics2D.BoxCast(transform.position, topBoxSize, 0, -transform.up, topCastDistance, groundLayer);
        bottomHit = Physics2D.BoxCast(transform.position, bottomBoxSize, 0, -transform.up, bottomCastDistance, groundLayer);
        rightHit = Physics2D.BoxCast(transform.position, rightBoxSize, 0, -transform.right, rightCastDistance, groundLayer);
        leftHit = Physics2D.BoxCast(transform.position, leftWallBoxSize, 0, -transform.right, leftWallCastDistance, groundLayer);

        if (moveForward)
        {
            ForwasdStickToGround();
            ForwardGetBackToGround();
        }
        else
        {
            BackwardStickToGround();
            BackwardGetBackToGround();
        }
        
    }

    private enum Direction
    {
        None,
        Up,
        Down,
        Forward,
        Backward
    }

    private void ForwasdStickToGround()
    {
        if (topHit && !bottomHit && !rightHit && !leftHit)
        {
            direction = Direction.Backward;
        }
        else if (bottomHit && !topHit && !rightHit && !leftHit)
        {
            direction = Direction.Forward;
        }
        else if (rightHit && !topHit && !bottomHit && !leftHit)
        {
            direction = Direction.Up;
        }
        else if (leftHit && !topHit && !bottomHit && !rightHit)
        {
            direction = Direction.Down;
        }
        else if (!topHit && !bottomHit && !rightHit && !leftHit)
        {
            direction = Direction.None;
        }

        switch (direction)
        {
            case Direction.None:
                RB.velocity = Vector2.down * speed;
                break;

            case Direction.Up:
                lastDirection = Direction.Up;
                RB.velocity = Vector2.up * speed;

                if (rightHit && topHit && !leftHit && !bottomHit)
                {
                    direction = Direction.Backward;
                }
                break;

            case Direction.Down:
                lastDirection = Direction.Down;
                RB.velocity = Vector2.down * speed;

                if (leftHit && bottomHit && !rightHit && !topHit)
                {
                    direction = Direction.Forward;
                }
                break;

            case Direction.Forward:
                lastDirection = Direction.Forward;
                RB.velocity = Vector2.right * speed;

                if (bottomHit && rightHit && !topHit && !leftHit)
                {
                    direction = Direction.Up;
                }
                break;

            case Direction.Backward:
                lastDirection = Direction.Backward;
                RB.velocity = Vector2.left * speed;

                if (topHit && leftHit && !bottomHit && !rightHit)
                {
                    direction = Direction.Down;
                }
                break;

            default:
                break;
        }
    }

    private void ForwardGetBackToGround()
    {
        if (direction == Direction.None)
        {
            switch (lastDirection)
            {   
                case Direction.Up:
                    RB.velocity = Vector2.right * speed;
                    break;

                case Direction.Down:
                    RB.velocity = Vector2.left * speed;
                    break;

                case Direction.Forward:
                    RB.velocity = Vector2.down * speed;
                    break;

                case Direction.Backward:
                    RB.velocity = Vector2.up * speed;
                    break;

                default:
                    break;
            }
        }
    }

    private void BackwardStickToGround()
    {
        if (topHit && !bottomHit && !rightHit && !leftHit)
        {
            direction = Direction.Forward;
        }
        else if (bottomHit && !topHit && !rightHit && !leftHit)
        {
            direction = Direction.Backward;
        }
        else if (rightHit && !topHit && !bottomHit && !leftHit)
        {
            direction = Direction.Down;
        }
        else if (leftHit && !topHit && !bottomHit && !rightHit)
        {
            direction = Direction.Up;
        }
        else if (!topHit && !bottomHit && !rightHit && !leftHit)
        {
            direction = Direction.None;
        }

        switch (direction)
        {
            case Direction.None:
                RB.velocity = Vector2.down * speed;
                break;

            case Direction.Up:
                lastDirection = Direction.Up;
                RB.velocity = Vector2.up * speed;

                if (leftHit && topHit && !bottomHit && !rightHit)
                {
                    direction = Direction.Forward;
                }
                break;

            case Direction.Down:
                lastDirection = Direction.Down;
                RB.velocity = Vector2.down * speed;

                if (rightHit && bottomHit && !leftHit && !topHit)
                {
                    direction = Direction.Backward;
                }
                break;

            case Direction.Forward:
                lastDirection = Direction.Forward;
                RB.velocity = Vector2.right * speed;

                if (topHit && rightHit && !bottomHit && !leftHit)
                {
                    direction = Direction.Down;
                }
                break;

            case Direction.Backward:
                lastDirection = Direction.Backward;
                RB.velocity = Vector2.left * speed;

                if (bottomHit && leftHit && !topHit && !rightHit)
                {
                    direction = Direction.Up;
                }
                break;

            default:
                break;
        }
    }

    private void BackwardGetBackToGround()
    {
        if (direction == Direction.None)
        {
            switch (lastDirection)
            {
                case Direction.Up:
                    RB.velocity = Vector2.left * speed;
                    break;

                case Direction.Down:
                    RB.velocity = Vector2.right * speed;
                    break;

                case Direction.Forward:
                    RB.velocity = Vector2.up * speed;
                    break;

                case Direction.Backward:
                    RB.velocity = Vector2.down * speed;
                    break;

                default:
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * topCastDistance, topBoxSize);
        Gizmos.DrawWireCube(transform.position - transform.up * bottomCastDistance, bottomBoxSize);
        Gizmos.DrawWireCube(transform.position - transform.right * rightCastDistance, rightBoxSize);
        Gizmos.DrawWireCube(transform.position - transform.right * leftWallCastDistance, leftWallBoxSize);
    }
}
