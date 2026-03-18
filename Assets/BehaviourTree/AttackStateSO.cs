using UnityEngine;

[CreateAssetMenu(fileName = "AttackState", menuName = "Scriptable Objects/AttackState")]
public class AttackStateSO : NodeSO
{
    public override bool EnterCondition(EnemyController ec) => ec.attack.check;

    public override bool ExitCondition(EnemyController ec)
    {
        return !ec.attack.check || ec.run.check || ec.die.check;
    }

    public override void OnStart(EnemyController ec)
    {
        ec.GetComponent<ChaseBehaviour>().StopChasing();
        ec.GetComponent<Animator>().SetBool(ec.attack.name, true);
    }

    public override void OnUpdate(EnemyController ec)
    {
        base.OnUpdate(ec);

        // Seguir mirando al jugador mientras ataca
        Vector3 direction = (ec.target.transform.position - ec.transform.position).normalized;
        direction.y = 0f;
        if (direction != Vector3.zero)
            ec.transform.rotation = Quaternion.LookRotation(direction);
    }

    public override void OnExit(EnemyController ec)
    {
        ec.GetComponent<Animator>().SetBool(ec.attack.name, false);
    }
}