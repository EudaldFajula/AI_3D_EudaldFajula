using UnityEngine;

[CreateAssetMenu(fileName = "DieState", menuName = "Scriptable Objects/DieState")]
public class DieStateSO : NodeSO
{
    public override bool EnterCondition(EnemyController ec) => ec.die.check;

    public override bool ExitCondition(EnemyController ec) => false;

    public override void OnStart(EnemyController ec)
    {
        ec.GetComponent<Animator>().SetBool(ec.die.name, true);
        ec.GetComponent<ChaseBehaviour>().StopChasing();

        // Desactivar físicas al morir
        ec.GetComponent<Rigidbody>().isKinematic = true;
    }

    public override void OnUpdate(EnemyController ec)
    {
        // No llama a base.OnUpdate para evitar ChangeState
    }

    public override void OnExit(EnemyController ec) { }
}