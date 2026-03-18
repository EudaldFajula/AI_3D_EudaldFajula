using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeSO", menuName = "Scriptable Objects/NodeSO")]
public class NodeSO : ScriptableObject
{
    public List<NodeSO> children;

    public virtual bool EnterCondition(EnemyController ec) => true;
    public virtual bool ExitCondition(EnemyController ec) => true;

    public virtual void OnStart(EnemyController ec) { }

    public virtual void OnUpdate(EnemyController ec)
    {
        if (ExitCondition(ec))
            ec.ChangeState();
    }

    public virtual void OnExit(EnemyController ec) { }
}