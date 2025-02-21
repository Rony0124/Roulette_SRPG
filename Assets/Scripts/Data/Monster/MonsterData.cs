using System;
using UnityEngine;

namespace TSoft.Data.Monster
{
    public enum MonsterType
    {
        None,
        Barrack,
        Forest,
        Grave,
        Fire,
        Ice,
        Thunder
    }
    
    [Serializable]
    public struct MonsterData
    {
        public string name;
        public MonsterType monsterType;
        public Sprite monsterMontage;
    }
}
