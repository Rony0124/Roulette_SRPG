using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BTRunner
{
    private readonly INode _rootNode;

    public BTRunner(INode rootNode)
    {
        _rootNode = rootNode;
    }

    public void Operate()
    {
        _rootNode.Evaluate();
    }
}
