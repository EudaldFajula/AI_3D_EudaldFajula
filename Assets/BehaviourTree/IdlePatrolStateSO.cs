using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "IdlePatrolState", menuName = "Scriptable Objects/IdlePatrolState")]
public class IdlePatrolStateSO : NodeSO
{
    public override bool EnterCondition(EnemyController ec) => true;

    public override bool ExitCondition(EnemyController ec)
    {
        return ec.chase.check || ec.attack.check || ec.run.check || ec.die.check;
    }

    public override void OnStart(EnemyController ec)
    {
        ec.agent.speed = ec.patrolSpeed;
        ec.agent.isStopped = false;
        GoToCurrentWaypoint(ec);
    }

    public override void OnUpdate(EnemyController ec)
    {
        // Si llega al waypoint, avanza al siguiente
        if (!ec.agent.pathPending && ec.agent.remainingDistance <= ec.waypointStoppingDistance)
        {
            ec.GoToNextWaypoint();
            GoToCurrentWaypoint(ec);
        }

        base.OnUpdate(ec);
    }

    public override void OnExit(EnemyController ec)
    {
        ec.agent.isStopped = true;
    }

    private void GoToCurrentWaypoint(EnemyController ec)
    {
        if (ec.waypoints == null || ec.waypoints.Length == 0) return;
        ec.agent.SetDestination(ec.waypoints[ec.waypointIndex].position);
    }
}