using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TSoft.Map
{
    public class Map
    {
        public List<Node> nodes;
        public List<Vector2Int> path;
        public string bossNodeName;
        public string configName;
        
        public Map(string configName, string bossNodeName, List<Node> nodes, List<Vector2Int> path)
        {
            this.configName = configName;
            this.bossNodeName = bossNodeName;
            this.nodes = nodes;
            this.path = path;
        }
        
        public Node GetBossNode()
        {
            return nodes.FirstOrDefault(n => n.nodeType == NodeType.Boss);
        }
        
        public Node GetNode(Vector2Int point)
        {
            return nodes.FirstOrDefault(n => n.point.Equals(point));
        }
    }
}

