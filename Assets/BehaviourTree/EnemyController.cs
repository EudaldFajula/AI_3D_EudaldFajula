using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Condiciones")]
    public Conditions attack;
    public Conditions chase;
    public Conditions die;
    public Conditions run;

    [Header("Referencias")]
    public GameObject target;
    public NodeSO root;
    public NodeSO currentState;

    [Header("Patrullaje")]
    public Transform[] waypoints;
    public float waypointStoppingDistance = 0.5f;
    [HideInInspector] public int waypointIndex = 0;

    [Header("Visión")]
    public float visionRange = 15f;
    public float visionAngle = 90f;       // ángulo total del cono de visión
    public LayerMask obstacleLayer;        // asigna el layer de paredes/obstáculos
    public Transform eyePoint;            // punto desde donde "ve" (ej: cabeza del enemigo)

    [Header("Stats")]
    public float attackDistance = 2f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public int HP = 5;

    [HideInInspector] public NavMeshAgent agent;

    private void Awake()
    {
        attack = new Conditions("Attack");
        chase = new Conditions("Chase");
        die = new Conditions("Dead");
        run = new Conditions("Run");

        agent = GetComponent<NavMeshAgent>();

        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        if (capsule != null)
            attackDistance = capsule.radius / 2f;

        ChangeState();
    }

    private void Update()
    {
        CheckVision();
        currentState?.OnUpdate(this);
    }

    // Comprueba si el enemigo ve al jugador
    private void CheckVision()
    {
        if (target == null) return;

        Vector3 origin = eyePoint != null ? eyePoint.position : transform.position;
        Vector3 directionToTarget = target.transform.position - origin;
        float distanceToTarget = directionToTarget.magnitude;

        bool canSee = false;

        if (distanceToTarget <= visionRange)
        {
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            if (angle <= visionAngle / 2f)
            {
                if (!Physics.Raycast(origin, directionToTarget.normalized, out RaycastHit hit, distanceToTarget, obstacleLayer))
                    canSee = true;
            }
        }

        // Solo llama a ChangeState si el valor realmente cambia
        if (canSee != chase.check)
        {
            chase.check = canSee;
            ChangeState();
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (target == null) return;
        attack.check = (target.transform.position - transform.position).magnitude <= attackDistance;
    }

    // El trigger solo asigna el target, la visión la gestiona CheckVision
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
            target = collision.gameObject;
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
            chase.check = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnHurt();
    }

    public void OnHurt()
    {
        HP--;
        if (HP < 2) run.check = true;
        if (HP <= 0) die.check = true;
    }

    public void GoToNextWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        waypointIndex = (waypointIndex + 1) % waypoints.Length;
    }

    public void ChangeState()
    {
        StartCoroutine(WaitToEndOfFrame());
    }

    private IEnumerator WaitToEndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        foreach (var node in root.children)
        {
            if (node.EnterCondition(this))
            {
                currentState?.OnExit(this);
                currentState = node;
                node.OnStart(this);
                break;
            }
        }
    }

    // Dibuja el cono de visión en el editor
    private void OnDrawGizmosSelected()
    {
        Vector3 origin = eyePoint != null ? eyePoint.position : transform.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, visionRange);

        Vector3 leftLimit = Quaternion.Euler(0, -visionAngle / 2f, 0) * transform.forward * visionRange;
        Vector3 rightLimit = Quaternion.Euler(0, visionAngle / 2f, 0) * transform.forward * visionRange;

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(origin, leftLimit);
        Gizmos.DrawRay(origin, rightLimit);

        // Null check antes de usar chase y target
        if (target != null && chase != null)
        {
            Gizmos.color = chase.check ? Color.green : Color.red;
            Gizmos.DrawLine(origin, target.transform.position);
        }
    }
}