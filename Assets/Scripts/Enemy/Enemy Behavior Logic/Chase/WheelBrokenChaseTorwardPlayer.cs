using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "WheelBroken Chase - Torward Player", menuName = "Enemy Logic/Chase Logic/ WheelBroken - Torward Player")]
public class WheelBrokenChaseTorwardPlayer : EnemyChaseSOBase
{ 
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float chaseDuration = 3f;
    private float chaseTimer;

    private Vector3 targetPosition;

    private bool CanMove { get { return enemy.Animator.GetBool(AnimationString.canMove); } }

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
    [SerializeField] private float facingSpotRange = 20f;
    [SerializeField] private float behindSpotRange = 10f;

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

        chaseTimer = chaseDuration;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        CheckForTarget(facingSpotRange, behindSpotRange);
        SetChaseDuration();
        ResetAttackCooldown();
        SwitchToAttackState();

    }

    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();

        ChasingTarget();
    }

    public override void ResetValues()
    {
        base.ResetValues();

        SpotTarget = false;
        chaseTimer = 0;
    }

    private void SetChaseDuration()
    {
        if (SpotTarget)
        {
            chaseTimer = chaseDuration;
        }
        else
        {
            chaseTimer -= Time.deltaTime;
        }

        if (chaseTimer <= 0)
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);  
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


    private void ChasingTarget()
    {
        if (chaseTimer > 0)
        {
            targetPosition = playerTransform.position;
            Vector2 directionToTarget = targetPosition - transform.position;
            bool isTargetBehind = (enemy.IsFacingRight && directionToTarget.x < 0) || (!enemy.IsFacingRight && directionToTarget.x > 0);

            if (isTargetBehind && CanMove)
            {
                enemy.FlipEnemy();
            }

            if(CanMove && enemy.TouchingDirections.IsGrounded && GroundZone.detectedCols.Count > 0 && !enemy.TouchingDirections.IsOnWall)
            {
                Vector2 direction = (targetPosition - transform.position).normalized;
                Vector2 moveDirection = new Vector2(chaseSpeed * direction.x, enemy.RB.velocity.y);
                enemy.MoveEnemy(moveDirection);
                IsMoving = true;
            }
            else
            {
                enemy.MoveEnemy(Vector2.zero);
                IsMoving = false;
            }
        }
    }

    private void SwitchToAttackState()
    {
        if (AttackZone.detectedCols.Count > 0)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }
    }

    private void ResetAttackCooldown()
    {
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }
}
