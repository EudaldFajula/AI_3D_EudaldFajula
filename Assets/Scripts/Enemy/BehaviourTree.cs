using UnityEngine;

public enum NodeState { Running, Success, Failure }

public abstract class Node
{
    public abstract NodeState Evaluate();
}

public class Selector : Node
{
    private Node[] children;
    public Selector(params Node[] children) { this.children = children; }

    public override NodeState Evaluate()
    {
        foreach (var child in children)
        {
            var result = child.Evaluate();
            if (result != NodeState.Failure) return result;
        }
        return NodeState.Failure;
    }
}

public class Sequence : Node
{
    private Node[] children;
    public Sequence(params Node[] children) { this.children = children; }

    public override NodeState Evaluate()
    {
        foreach (var child in children)
        {
            var result = child.Evaluate();
            if (result != NodeState.Success) return result;
        }
        return NodeState.Success;
    }
}