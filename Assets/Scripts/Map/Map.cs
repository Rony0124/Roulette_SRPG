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
        
        public float DistanceBetweenFirstAndLastLayers()
        {
            Node bossNode = GetBossNode();
            Node firstLayerNode = nodes.FirstOrDefault(n => n.point.y == 0);

            if (bossNode == null || firstLayerNode == null)
                return 0f;

            return bossNode.position.y - firstLayerNode.position.y;
        }
    }
}

