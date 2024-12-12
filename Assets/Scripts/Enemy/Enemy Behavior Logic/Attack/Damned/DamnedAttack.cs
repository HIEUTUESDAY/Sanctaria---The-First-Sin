using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Damned Attack - Throw Stone", menuName = "Enemy Logic/Attack Logic/Damned - Throw Stone")]
public class DamnedAttack : EnemyAttackSOBase
{
    private DetectionZone AttackZone;
    private Vector3 targetPosition;

    public bool HasTarget
    {
        get { return enemy.Animator.GetBool(AnimationString.hasTarget); }
        private set
        {
            enemy.Animator.SetBool(AnimationString.hasTarget, value);
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

        FacingTargetPosition();
        AttackPlayer();
        ResetAttackCooldown();
        SwitchToIdleState();
    }

    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();
    }

    public override void ResetValues()
    {
        base.ResetValues();
        HasTarget = false;
    }

    private void AttackPlayer()
    {
        if (AttackZone.attackableCols.Count > 0)
        {
            HasTarget = true;
        }
    }

    private void ResetAttackCooldown()
    {
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    private void SwitchToIdleState()
    {
        if (AttackZone.attackableCols.Count <= 0)
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
        }
    }

    private void FacingTargetPosition()
    {
        targetPosition = playerTransform.position;
        Vector2 directionToTarget = targetPosition - transform.position;
        bool isTargetBehind = (enemy.IsFacingRight && directionToTarget.x < 0) || (!enemy.IsFacingRight && directionToTarget.x > 0);

        if (isTargetBehind)
        {
            enemy.FlipEnemy();
        }
    }
}
