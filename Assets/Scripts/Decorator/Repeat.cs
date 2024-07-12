using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Repeat : INode
{
    private readonly INode _children;

    private int _repeatCount;
    private int _currentCount = 0;

    public Repeat(INode children)
    {
        _children = children;
    }

    public INode.NodeState Evaluate()
    {
        if (_children == null)
            return INode.NodeState.Failure;
        
        if (_currentCount < _repeatCount)
        {
            _children.Evaluate();
            _currentCount++;

            return INode.NodeState.Running;
        }
        else
        {
            _currentCount = 0;
            return INode.NodeState.Success;
        }
    }
}
