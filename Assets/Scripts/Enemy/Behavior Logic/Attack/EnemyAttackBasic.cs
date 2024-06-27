using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "Attack-Basic", menuName = "Enemy Logic/Attack Logic/Basic")]
public class EnemyAttackBasic : EnemyAttackSOBase
{
    private DetectionZone AttackZone;

    public float AttackCooldown
    {
        get
        {
            return enemy.Animator.GetFloat(AnimationString.attackCooldown);
        }
        private set
        {
            enemy.Animator.SetFloat(AnimationString.attackCooldown, Mathf.Max(value, 0));
        }
    }

    public override void DoAnimationTriggerEventLogic(Enemy.AniamtionTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
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

        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }

        if (AttackZone.detectedCols.Count <= 0)
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
        }
        else
        {
            enemy.MoveEnemy(Vector2.zero);
            enemy.Animator.SetBool(AnimationString.isMoving, false);
            enemy.Animator.SetBool(AnimationString.hasTarget, true);
        }
    }

    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();
    }

    public override void ResetValues()
    {
        base.ResetValues();
        enemy.Animator.SetBool(AnimationString.hasTarget, false);
        AttackCooldown = 0;
    }
}
