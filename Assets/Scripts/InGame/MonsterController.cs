using System;
using TSoft.Data.Monster;
using TSoft.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace TSoft.InGame
{
    public class MonsterController : MonoBehaviour
    {
        private MonsterData data;
        
        public MonsterData Data
        {
            get => data;
            set => data = value;
        }

        public UnityEvent onDamage;
        public UnityEvent onDead;

        public void TakeDamage(bool isDead)
        {
            if (isDead)
            {
                onDead?.Invoke();
            }
            else
            {
                onDamage?.Invoke();    
            }
        }
    }
}
