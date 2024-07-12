using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SelectorNode : INode
{
    List<INode> _childs;

    public SelectorNode(List<INode> childs)
    {
        _childs = childs;
    }

    public INode.NodeState Evaluate()
    {
        if (_childs == null)
            return INode.NodeState.Failure;

        foreach (var child in _childs)
        {
            switch (child.Evaluate())
            {
                case INode.NodeState.Running:
                    return INode.NodeState.Running;
                case INode.NodeState.Success:
                    return INode.NodeState.Success;
            }
        }

        return INode.NodeState.Failure;
    }
}
