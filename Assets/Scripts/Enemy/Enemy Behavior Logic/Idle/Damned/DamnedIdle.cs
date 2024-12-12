using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Damned Idle - Wait For Player", menuName = "Enemy Logic/Idle Logic/Damned - Wait For Player")]
public class DamnedIdle : EnemyIdleSOBase
{

    public float AttackCooldown
    {
        get { return enemy.Animator.GetFloat(AnimationString.attackCooldown); }
        private set
        {
            enemy.Animator.SetFloat(AnimationString.attackCooldown, Mathf.Max(value, 0));
        }
    }

    private DetectionZone AttackZone;

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

        DetectTarget();
    }

    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();

        ResetAttackCooldown();
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }

    private void DetectTarget()
    {
        if (AttackZone.attackableCols.Count > 0)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
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
}
