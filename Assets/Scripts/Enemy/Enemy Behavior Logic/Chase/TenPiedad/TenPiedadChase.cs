using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossTenPiedad Chase", menuName = "Boss Logic/Chase Logic/ TenPiedad")]
public class TenPiedadChase : EnemyChaseSOBase
{
    [SerializeField] private float chaseSpeed = 4f;
    private Vector3 targetPosition;
    private DetectionZone AttackZone;

    private bool CanMove { get { return enemy.Animator.GetBool(AnimationString.canMove); } }

    public bool IsMoving
    {
        get { return enemy.Animator.GetBool(AnimationString.isMoving); }
        private set
        {
            enemy.Animator.SetBool(AnimationString.isMoving, value);
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

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);

        AttackZone = transform.Find("AttackDetectionZone").GetComponent<DetectionZone>();
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

    }

    private void ChasingTarget()
    {
        if (enemy.IsAlive)
        {
            targetPosition = Player.Instance.transform.position;
            Vector2 directionToTarget = targetPosition - transform.position;
            bool isTargetBehind = (enemy.IsFacingRight && directionToTarget.x < 0) || (!enemy.IsFacingRight && directionToTarget.x > 0);

            if (isTargetBehind && CanMove)
            {
                enemy.FlipEnemy();
            }

            if (CanMove)
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
        else
        {
            enemy.MoveEnemy(Vector2.zero);
            IsMoving = false;
        }
    }

    private void SwitchToAttackState()
    {
        if (AttackZone.detectedCols.Count > 0 && AttackCooldown <= 0)
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
