using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossTenPiedad Idle", menuName = "Boss Logic/Idle Logic/TenPiedad")]
public class TenPiedadIdle : EnemyIdleSOBase
{
    public bool CanMove { get { return enemy.Animator.GetBool(AnimationString.canMove); } }

    public bool hasTarget = false;

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);

    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();

        UIManager.Instance.bossHealthBar.SetActive(true);
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        WaitForPlayer();
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

    private void WaitForPlayer()
    {
        if (hasTarget)
        {
            Player.Instance.PlayerInput.enabled = false;
            enemy.Animator.SetTrigger(AnimationString.activeBossTrigger);

            if (CanMove)
            {
                enemy.StateMachine.ChangeState(enemy.ChaseState);
                Player.Instance.PlayerInput.enabled = true;
            }
        }
        else
        {
            return;
        }
    }
}
