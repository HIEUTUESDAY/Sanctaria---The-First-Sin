using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "WheelBroken Attack - Basic", menuName = "Enemy Logic/Attack Logic/ WheelBroken - Basic")]
public class WheelBrokenAttackBasic : EnemyAttackSOBase
{
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
        get{ return enemy.Animator.GetFloat(AnimationString.attackCooldown); }
        private set
        {
            enemy.Animator.SetFloat(AnimationString.attackCooldown, Mathf.Max(value, 0));
        }
    }

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);

        AttackZone = transform.Find("Attack Detection Zone").GetComponent<DetectionZone>();
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();

        enemy.StopAllCoroutines();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        AttackPlayer();
        ResetAttackCooldown();
        SwitchToChaseState();
     
    }

    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();
    }

    public override void ResetValues()
    {
        base.ResetValues();
        HasTarget = false;
        IsMoving = true;
    }

    private void AttackPlayer()
    {
        if (AttackZone.attackableCols.Count > 0 && CanMove)
        {
            HasTarget = true;
            enemy.MoveEnemy(Vector2.zero);
            IsMoving = false;
        }
    }

    private void ResetAttackCooldown()
    {
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    private void SwitchToChaseState()
    {
        if (AttackZone.attackableCols.Count <= 0 && CanMove)
        {
            enemy.StateMachine.ChangeState(enemy.ChaseState);
        }
    }
}
