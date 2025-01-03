using System;
using TSoft.Data.Monster;
using TSoft.InGame.GamePlaySystem;
using TSoft.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace TSoft.InGame
{
    public class MonsterController : MonoBehaviour
    {
        private MonsterData data;
        private Gameplay gameplay;
       
        public MonsterData Data
        {
            get => data;
            set => data = value;
        }
        
        public Gameplay GamePlay => gameplay;

        public UnityEvent onDamage;
        public UnityEvent onDead;

        private void Awake()
        {
            gameplay = GetComponent<Gameplay>();
        }

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
