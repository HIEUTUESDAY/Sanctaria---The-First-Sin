using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public Vector2 groundBoxSize;
    public Vector2 wallBoxSize;
    public Vector2 ceilingBoxSize;
    public LayerMask groundLayer;
    public LayerMask jumpWallLayer;
    public float groundCastDistance;
    public float wallCastDistance;
    public float ceilingCastDistance;

    Animator animator;

    [SerializeField]
    private bool _isGrounded;

    public bool IsGrounded 
    { 
        get 
        {
            return _isGrounded;
        } 
        private set
        {
            _isGrounded = value;
            animator.SetBool(AnimationString.isGrounded, value);
        }
    }

    [SerializeField]
    private bool _isOnWall;
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    public bool IsOnWall
    {
        get
        {
            return _isOnWall;
        }
        private set
        {
            _isOnWall = value;
            animator.SetBool(AnimationString.isOnWall, value);
        }
    }

    [SerializeField]
    private bool _isOnJumpWall;

    public bool IsOnJumpWall
    {
        get
        {
            return _isOnJumpWall;
        }
        private set
        {
            _isOnJumpWall = value;
            animator.SetBool(AnimationString.isOnJumpWall, value);
        }
    }

    [SerializeField]
    private bool _isOnCeiling;

    public bool IsOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        private set
        {
            _isOnCeiling = value;
            animator.SetBool(AnimationString.isOnCeiling, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        IsGrounded = Physics2D.BoxCast(transform.position, groundBoxSize, 0, -transform.up, groundCastDistance, groundLayer);
        IsOnWall = Physics2D.BoxCast(transform.position, wallBoxSize, 0, -transform.right, wallCastDistance, groundLayer);
        IsOnJumpWall = Physics2D.BoxCast(transform.position, wallBoxSize, 0, -transform.right, wallCastDistance, jumpWallLayer);
        IsOnCeiling = Physics2D.BoxCast(transform.position, ceilingBoxSize, 0, -transform.up, ceilingCastDistance, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * groundCastDistance, groundBoxSize);
        Gizmos.DrawWireCube(transform.position - transform.right * wallCastDistance, wallBoxSize);
        Gizmos.DrawWireCube(transform.position - transform.up * ceilingCastDistance, ceilingBoxSize);
    }
}
