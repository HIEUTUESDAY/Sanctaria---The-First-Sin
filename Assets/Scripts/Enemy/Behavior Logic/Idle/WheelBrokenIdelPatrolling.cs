using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "Idle-Patrol Around", menuName = "Enemy Logic/Idle Logic/Patrol Around")]
public class WheelBrokenIdlePatrolling : EnemyIdleSOBase
{
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float minStopTime = 2f;
    [SerializeField] private float maxStopTime = 6f;

    private bool CanMove { get => enemy.Animator.GetBool(AnimationString.canMove); }
    private bool isFlipping = false;

    private DetectionZone GroundZone;
    private DetectionZone AttackZone;

    private Transform FacingSpotPoint;
    private Transform BehindSpotPoint;
    [SerializeField] private float facingSpotRange = 15f;
    [SerializeField] private float behindSpotRange = 5f;

    public override void DoAnimationTriggerEventLogic(Enemy.AniamtionTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

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
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        CheckForTarget(facingSpotRange, behindSpotRange);

        if (AttackZone.detectedCols.Count > 0)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }
    }

    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();

        Move();
        WallHitFlip();
        NoGroundFlip();
    }

    public override void ResetValues()
    {
        base.ResetValues();

        isFlipping = false;
        enemy.StopAllCoroutines();
    }

    private void Move()
    {
        if (CanMove && GroundZone.detectedCols.Count > 0)
        {
            Vector2 direction = (enemy.IsFacingRight ? Vector2.right : Vector2.left).normalized;
            Vector2 moveDirection = new Vector2(patrolSpeed * direction.x, enemy.RB.velocity.y);
            enemy.MoveEnemy(moveDirection);
            enemy.Animator.SetBool(AnimationString.isMoving, true);
        }
        else
        {
            enemy.MoveEnemy(Vector2.zero);
            enemy.Animator.SetBool(AnimationString.isMoving, false);
        }
    }

    private void WallHitFlip()
    {
        if (enemy.TouchingDirections.IsGrounded && enemy.TouchingDirections.IsOnWall && !isFlipping)
        {
            enemy.StartCoroutine(WallHitFlipCoroutine());
        }
    }

    private void NoGroundFlip()
    {
        if (enemy.TouchingDirections.IsGrounded && GroundZone.detectedCols.Count <= 0f && !isFlipping)
        {
            enemy.StartCoroutine(NoGroundFlipCoroutine());
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
        RaycastHit2D facingHitTarget = Physics2D.Linecast(FacingSpotPoint.position, facingEndPos, LayerMask.GetMask("Ground", "Player"));
        RaycastHit2D behindHitTarget = Physics2D.Linecast(BehindSpotPoint.position, behindEndPos, LayerMask.GetMask("Ground", "Player"));

        if(facingHitTarget.collider != null)
        {
            if (facingHitTarget.collider.gameObject.CompareTag("Player"))
            {
                enemy.StateMachine.ChangeState(enemy.ChaseState);
            }
        }
        else if (behindHitTarget.collider != null)
        {
            if (behindHitTarget.collider.gameObject.CompareTag("Player"))
            {
                enemy.StateMachine.ChangeState(enemy.ChaseState);
            }
        }
    }

    private IEnumerator WallHitFlipCoroutine()
    {
        isFlipping = true;
        enemy.FlipEnemy();
        yield return new WaitForSeconds(Random.Range(minStopTime, maxStopTime));
        isFlipping = false;
        Move();
    }

    private IEnumerator NoGroundFlipCoroutine()
    {
        isFlipping = true;
        yield return new WaitForSeconds(Random.Range(minStopTime, maxStopTime));
        enemy.FlipEnemy();
        isFlipping = false;
        Move();
    }

}
