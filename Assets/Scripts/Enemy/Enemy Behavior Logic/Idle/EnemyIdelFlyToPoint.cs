using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle-Fly To Point", menuName = "Enemy Logic/Idle Logic/Fly To Point")]
public class EnemyIdelFlyToPoint : EnemyIdleSOBase
{
    private WayPoint WayPoint;
    [SerializeField] private float flightSpeed = 10f;
    [SerializeField] private float waypointReachedDistance = 1f;
    private float distanceToWayPoint;

    public bool CanMove
    {
        get
        {
            return enemy.Animator.GetBool(AnimationString.canMove);
        }
    }

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);

        WayPoint = transform.GetComponent<WayPoint>();
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        if(WayPoint != null ) 
        {
            WayPoint.pointNumber = 0;
            WayPoint.nextPoint = WayPoint.pointList[WayPoint.pointNumber];
        }
       
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
    }

    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();

        if (enemy.IsAlive && WayPoint != null && WayPoint.pointList.Count > 0)
        {
            if (CanMove)
            {
                FlyToWayPoint();
            }
            else
            {
                enemy.MoveEnemy(Vector2.zero);
            }
        }
    }

    private void FlyToWayPoint()
    {
        if (WayPoint.nextPoint == null)
            return;

        // Fly to the next waypoint
        Vector2 directionToWaypoint = (WayPoint.nextPoint.position - enemy.transform.position).normalized;

        // Check if we have reached the waypoint already
        distanceToWayPoint = Vector2.Distance(WayPoint.nextPoint.position, enemy.transform.position);

        Vector2 flyDirection = directionToWaypoint * flightSpeed;
        enemy.MoveEnemy(flyDirection);
        ChangeDirection();

        // Check if we need to switch waypoint
        if (distanceToWayPoint <= waypointReachedDistance)
        {
            // Switch to the next waypoint
            WayPoint.pointNumber++;

            if (WayPoint.pointNumber >= WayPoint.pointList.Count)
            {
                // Loop back to the first waypoint
                WayPoint.pointNumber = 0;
            }

            WayPoint.nextPoint = WayPoint.pointList[WayPoint.pointNumber];
        }
    }

    private void ChangeDirection()
    {
        if (enemy.RB.velocity.x < 0 && enemy.IsFacingRight)
        {
            enemy.FlipEnemy();
        }
        else if (enemy.RB.velocity.x > 0 && !enemy.IsFacingRight)
        {
            enemy.FlipEnemy();
        }
    }
}
