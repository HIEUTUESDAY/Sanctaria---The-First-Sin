using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossTenPiedad Attack", menuName = "Boss Logic/Attack Logic/ TenPiedad")]
public class TenPiedadAttack : EnemyAttackSOBase
{
    private DetectionZone AttackZone;

    private bool CanMove { get { return enemy.Animator.GetBool(AnimationString.canMove); } }

    public float AttackCooldown
    {
        get { return enemy.Animator.GetFloat(AnimationString.attackCooldown); }
        private set
        {
            enemy.Animator.SetFloat(AnimationString.attackCooldown, Mathf.Max(value, 0));
        }
    }

    public bool IsOnAttack
    {
        get { return enemy.Animator.GetBool(AnimationString.isOnAttack); }
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

        AttackPlayer();
        SwitchToChaseState();
        ResetAttackCooldown();
    }

    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();

        enemy.RB.velocity = Vector2.zero;
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }

    private void AttackPlayer()
    {
        if (enemy.IsAlive) 
        {
            if (AttackZone.attackableCols.Count > 0 && CanMove)
            {
                ChooseAttack();
            }
        }
    }

    private void ChooseAttack()
    {
        int randomAttack = Random.Range(0, 2);

        if (randomAttack == 0)
        {
            enemy.Animator.SetTrigger(AnimationString.handAttackTrigger);
        }
        else
        {
            enemy.Animator.SetTrigger(AnimationString.footAttackTrigger);
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
        if (!IsOnAttack && CanMove)
        {
            enemy.StateMachine.ChangeState(enemy.ChaseState);
        }
    }
}
