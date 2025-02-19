using System;
using UnityEngine;

namespace TSoft.Data.Monster
{
    public enum MonsterType
    {
        None,
        Barrack,
        Forest
    }
    
    [Serializable]
    public struct MonsterData
    {
        public string name;
        public MonsterType monsterType;
        public Sprite monsterMontage;
    }
}
