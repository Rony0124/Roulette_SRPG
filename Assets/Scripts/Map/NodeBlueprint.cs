using System;
using TSoft.Data;
using TSoft.Data.Monster;
using UnityEngine;

namespace TSoft.Map
{
    public enum NodeType
    {
        MinorEnemy,
        EliteEnemy,
        Treasure,
        Store,
        Boss
    }
    
    [CreateAssetMenu]
    public class NodeBlueprint : ScriptableObject
    {
        public Sprite sprite;
        public NodeType nodeType;
        public MonsterDataSO monsterData;
    }
}

