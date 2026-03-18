using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public Transform[] waypoints;

    [Header("Distancias")]
    public float detectionRange = 10f;
    public float chaseRange = 20f;

    [Header("Velocidades")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    private NavMeshAgent agent;
    private Node behaviourTree;
    private int[] waypointIndex = { 0 }; // array para pasar índice por referencia

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        BuildTree();
    }

    void BuildTree()
    {
        behaviourTree = new Selector(

            // Rama 1: perseguir si el jugador está cerca
            new Sequence(
                new PlayerInRange(transform, player, detectionRange),
                new ChasePlayer(agent, player, chaseSpeed)
            ),

            // Rama 2: volver a patrullar si el jugador se alejó
            new Sequence(
                new PlayerOutOfChaseRange(transform, player, chaseRange),
                new HasWaypoints(waypoints),
                new PatrolToWaypoint(agent, waypoints, waypointIndex, patrolSpeed),
                new GoToNextWaypoint(waypoints, waypointIndex)
            )
        );
    }

    void Update()
    {
        behaviourTree.Evaluate();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}