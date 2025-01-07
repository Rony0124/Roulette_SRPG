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
        public Action<float> onDamaged;
        
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

        public bool TakeDamage(int damage)
        {
            var currentHp = GamePlay.GetAttr(GameplayAttr.Heart);
            currentHp = Math.Max(0, currentHp - damage);
            Debug.Log("remaining hp : " + currentHp );
            
            GamePlay.SetAttr(GameplayAttr.Heart, currentHp);
            var isDead = currentHp <= 0;
            if (isDead)
            {
                onDead?.Invoke();
            }
            else
            {
                onDamaged?.Invoke(currentHp);
                onDamage?.Invoke();    
            }

            return isDead;
        }
    }
}
