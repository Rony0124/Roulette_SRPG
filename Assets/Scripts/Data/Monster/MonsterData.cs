using System;
using UnityEngine;

namespace TSoft.Data.Monster
{
    public enum MonsterType
    {
        Barrack
    }
    
    [Serializable]
    public struct MonsterData
    {
        public string name;
        public MonsterType monsterType;
    }
}
