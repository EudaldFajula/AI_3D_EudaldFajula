using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed;

    private NavMeshAgent agent;
    private Rigidbody rb;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    public void Chase(Transform target, Transform self)
    {
        if (agent != null)
        {
            agent.isStopped = false;
            agent.speed = speed;
            agent.SetDestination(target.position);
        }
        else
        {
            Vector3 direction = (target.position - self.position).normalized;
            direction.y = 0f;
            rb.linearVelocity = direction * speed;
        }
    }

    public void Run(Transform target, Transform self)
    {
        if (agent != null)
        {
            agent.isStopped = false;
            agent.speed = speed;
            // Calcula posición opuesta al objetivo
            Vector3 fleeDirection = (self.position - target.position).normalized;
            agent.SetDestination(self.position + fleeDirection * 10f);
        }
        else
        {
            Vector3 direction = (target.position - self.position).normalized;
            direction.y = 0f;
            rb.linearVelocity = -direction * speed;
        }
    }

    public void StopChasing()
    {
        if (agent != null)
            agent.isStopped = true;
        else if (rb != null)
            rb.linearVelocity = Vector3.zero;
    }
}