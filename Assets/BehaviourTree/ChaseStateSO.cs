using UnityEngine;

[CreateAssetMenu(fileName = "ChaseState", menuName = "Scriptable Objects/ChaseState")]
public class ChaseStateSO : NodeSO
{
    public override bool EnterCondition(EnemyController ec) => ec.chase.check;

    public override bool ExitCondition(EnemyController ec)
    {
        return !ec.chase.check || ec.run.check || ec.attack.check || ec.die.check;
    }

    public override void OnStart(EnemyController ec)
    {
        ec.agent.isStopped = false;
        ec.agent.speed = ec.chaseSpeed;
        ec.GetComponent<Animator>()?.SetBool(ec.chase.name, true);
    }

    public override void OnUpdate(EnemyController ec)
    {
        ec.agent.SetDestination(ec.target.transform.position);
        base.OnUpdate(ec);
    }

    public override void OnExit(EnemyController ec)
    {
        ec.agent.isStopped = true;
        ec.GetComponent<Animator>()?.SetBool(ec.chase.name, false);
    }
}