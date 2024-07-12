using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "WheelBroken Idle - Patrol Around", menuName = "Enemy Logic/Idle Logic/WheelBroken - Patrol Around")]
public class WheelBrokenIdlePatrolAround : EnemyIdleSOBase
{
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float minStopTime = 2f;
    [SerializeField] private float maxStopTime = 6f;
    [SerializeField] private float flipDelay = 2f;

    public bool CanMove { get{ return enemy.Animator.GetBool(AnimationString.canMove); } }
    private bool isFlipping = false;

    public bool IsMoving
    {
        get { return enemy.Animator.GetBool(AnimationString.isMoving); }
        private set
        {
            enemy.Animator.SetBool(AnimationString.isMoving, value);
        }
    }

    public bool SpotTarget
    {
        get { return enemy.Animator.GetBool(AnimationString.spotTarget); }
        private set
        {
            enemy.Animator.SetBool(AnimationString.spotTarget, value);
        }
    }

    public float AttackCooldown
    {
        get { return enemy.Animator.GetFloat(AnimationString.attackCooldown); }
        private set
        {
            enemy.Animator.SetFloat(AnimationString.attackCooldown, Mathf.Max(value, 0));
        }
    }

    private DetectionZone GroundZone;
    private DetectionZone AttackZone;

    private Transform FacingSpotPoint;
    private Transform BehindSpotPoint;
    [SerializeField] private float facingSpotRange = 15f;
    [SerializeField] private float behindSpotRange = 5f;

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
        GroundZone = transform.Find("Ground Detection Zone").GetComponent<DetectionZone>();
        AttackZone = transform.Find("Attack Detection Zone").GetComponent<DetectionZone>();
        FacingSpotPoint = transform.Find("Facing Spot Point").GetComponent<Transform>();
        BehindSpotPoint = transform.Find("Behind Spot Point").GetComponent<Transform>();
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();

        CoroutineManager.Instance.StopCoroutineManager(WallHitFlipCoroutine());
        CoroutineManager.Instance.StopCoroutineManager(NoGroundFlipCoroutine());
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        CheckForTarget(facingSpotRange, behindSpotRange);
        DetectTarget();
        HasTarget();
        WallHitFlip();
        NoGroundFlip();
    }

    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();

        Move();
        ResetAttackCooldown();
    }

    public override void ResetValues()
    {
        base.ResetValues();

        isFlipping = false;
        SpotTarget = false;
    }

    private void Move()
    {
        if (CanMove && GroundZone.detectedCols.Count > 0 && !isFlipping)
        {
            Vector2 direction = (enemy.IsFacingRight ? Vector2.right : Vector2.left).normalized;
            Vector2 moveDirection = new Vector2(patrolSpeed * direction.x, enemy.RB.velocity.y);
            enemy.MoveEnemy(moveDirection);
            IsMoving = true;
        }
        else
        {
            enemy.MoveEnemy(Vector2.zero);
            IsMoving = false;
        }
    }

    private void WallHitFlip()
    {
        if (enemy.TouchingDirections.IsGrounded && enemy.TouchingDirections.IsOnWall && !isFlipping)
        {
            CoroutineManager.Instance.StartCoroutineManager(WallHitFlipCoroutine());
        }
    }

    private void NoGroundFlip()
    {
        if (enemy.TouchingDirections.IsGrounded && GroundZone.detectedCols.Count <= 0 && !isFlipping)
        {
            CoroutineManager.Instance.StartCoroutineManager(NoGroundFlipCoroutine());
        }
    }

    private void CheckForTarget(float facingDistance, float behindDistance)
    {
        float facingCastDistance;
        float behindCastDistance;

        if (enemy.IsFacingRight)
        {
            facingCastDistance = facingDistance;
            behindCastDistance = behindDistance;
        }
        else
        {
            facingCastDistance = -facingDistance;
            behindCastDistance = -behindDistance;
        }

        Vector2 facingEndPos = FacingSpotPoint.position + Vector3.right * facingCastDistance;
        Vector2 behindEndPos = BehindSpotPoint.position + Vector3.left * behindCastDistance;

        RaycastHit2D facingHitTarget = Physics2D.Linecast(FacingSpotPoint.position, facingEndPos, LayerMask.GetMask("Player", "Ground"));
        RaycastHit2D behindHitTarget = Physics2D.Linecast(BehindSpotPoint.position, behindEndPos, LayerMask.GetMask("Player", "Ground"));

        if (facingHitTarget.collider != null)
        {
            if (facingHitTarget.collider.CompareTag("Player"))
            {
                SpotTarget = true;
            }
        }
        else
        {
            SpotTarget = false;
        }

        if (behindHitTarget.collider != null)
        {
            if (behindHitTarget.collider.CompareTag("Player"))
            {
                SpotTarget = true;
            }
        }
        else
        {
            SpotTarget = false;
        }
    }

    private void HasTarget()
    {
        if (AttackZone.attackableCols.Count > 0)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }
    }

    private void DetectTarget()
    {
        if (SpotTarget)
        {
            enemy.StateMachine.ChangeState(enemy.ChaseState);
        }
        else
        {
            return;
        }
    }

    private void ResetAttackCooldown()
    {
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    private IEnumerator WallHitFlipCoroutine()
    {
        isFlipping = true;
        enemy.FlipEnemy();
        yield return new WaitForSeconds(Random.Range(minStopTime, maxStopTime));
        Move();
        yield return new WaitForSeconds(flipDelay);
        isFlipping = false;
    }

    private IEnumerator NoGroundFlipCoroutine()
    {
        isFlipping = true;
        yield return new WaitForSeconds(Random.Range(minStopTime, maxStopTime));
        enemy.FlipEnemy();
        Move();
        yield return new WaitForSeconds(flipDelay);
        isFlipping = false;
    }

}
