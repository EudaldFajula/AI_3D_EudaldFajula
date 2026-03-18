using UnityEngine;

[CreateAssetMenu(fileName = "RunState", menuName = "Scriptable Objects/RunState")]
public class RunStateSO : NodeSO
{
    public override bool EnterCondition(EnemyController ec) => ec.run.check;

    public override bool ExitCondition(EnemyController ec)
    {
        return !ec.run.check || ec.die.check;
    }

    public override void OnStart(EnemyController ec)
    {
        ec.GetComponent<Animator>().SetBool(ec.run.name, true);
    }

    public override void OnUpdate(EnemyController ec)
    {
        base.OnUpdate(ec);
        ec.GetComponent<ChaseBehaviour>().Run(ec.target.transform, ec.transform);

        // Rotar en dirección contraria al jugador
        Vector3 direction = (ec.transform.position - ec.target.transform.position).normalized;
        direction.y = 0f;
        if (direction != Vector3.zero)
            ec.transform.rotation = Quaternion.LookRotation(direction);
    }

    public override void OnExit(EnemyController ec)
    {
        ec.GetComponent<Animator>().SetBool(ec.run.name, false);
        ec.GetComponent<ChaseBehaviour>().StopChasing();
    }
}