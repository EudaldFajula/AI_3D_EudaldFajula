using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

// Condición: ¿el jugador está dentro del rango de detección?
public class PlayerInRange : Node
{
    private Transform enemy;
    private Transform player;
    private float range;

    public PlayerInRange(Transform enemy, Transform player, float range)
    {
        this.enemy = enemy; this.player = player; this.range = range;
    }

    public override NodeState Evaluate()
    {
        float dist = Vector3.Distance(enemy.position, player.position);
        return dist <= range ? NodeState.Success : NodeState.Failure;
    }
}

// Condición: ¿el jugador está fuera del rango de chase?
public class PlayerOutOfChaseRange : Node
{
    private Transform enemy;
    private Transform player;
    private float range;

    public PlayerOutOfChaseRange(Transform enemy, Transform player, float range)
    {
        this.enemy = enemy; this.player = player; this.range = range;
    }

    public override NodeState Evaluate()
    {
        float dist = Vector3.Distance(enemy.position, player.position);
        return dist > range ? NodeState.Success : NodeState.Failure;
    }
}

// Acción: perseguir al jugador
public class ChasePlayer : Node
{
    private NavMeshAgent agent;
    private Transform player;
    private float chaseSpeed;

    public ChasePlayer(NavMeshAgent agent, Transform player, float chaseSpeed)
    {
        this.agent = agent; this.player = player; this.chaseSpeed = chaseSpeed;
    }

    public override NodeState Evaluate()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
        return NodeState.Running;
    }
}

// Condición: ¿hay waypoints disponibles?
public class HasWaypoints : Node
{
    private Transform[] waypoints;
    public HasWaypoints(Transform[] waypoints) { this.waypoints = waypoints; }

    public override NodeState Evaluate()
    {
        return waypoints.Length > 0 ? NodeState.Success : NodeState.Failure;
    }
}

// Acción: moverse al waypoint actual
public class PatrolToWaypoint : Node
{
    private NavMeshAgent agent;
    private Transform[] waypoints;
    private int[] indexRef; // array de 1 elemento para pasar por referencia
    private float patrolSpeed;

    public PatrolToWaypoint(NavMeshAgent agent, Transform[] waypoints, int[] indexRef, float patrolSpeed)
    {
        this.agent = agent; this.waypoints = waypoints;
        this.indexRef = indexRef; this.patrolSpeed = patrolSpeed;
    }

    public override NodeState Evaluate()
    {
        agent.speed = patrolSpeed;
        agent.SetDestination(waypoints[indexRef[0]].position);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            return NodeState.Success; // llegó, listo para avanzar

        return NodeState.Running;
    }
}

// Acción: avanzar al siguiente waypoint
public class GoToNextWaypoint : Node
{
    private Transform[] waypoints;
    private int[] indexRef;

    public GoToNextWaypoint(Transform[] waypoints, int[] indexRef)
    {
        this.waypoints = waypoints; this.indexRef = indexRef;
    }

    public override NodeState Evaluate()
    {
        indexRef[0] = (indexRef[0] + 1) % waypoints.Length;
        return NodeState.Success;
    }
}