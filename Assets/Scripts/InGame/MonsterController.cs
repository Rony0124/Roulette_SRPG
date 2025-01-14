using System;
using MoreMountains.Feedbacks;
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
        
        public MMFeedbacks DamageFeedback;

        [SerializeField] private float textAddPositionY;

       // public UnityEvent onDamage;
        public UnityEvent onDead;

        private void Awake()
        {
            gameplay = GetComponent<Gameplay>();
            gameplay.Init();
        }

        public bool TakeDamage(int damage)
        {
            var currentHp = GamePlay.GetAttr(GameplayAttr.Heart);
            Debug.Log("currentHp hp : " + currentHp );
            Debug.Log("damage : " + damage );
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
                var textPos = transform.position + (Vector3.up * textAddPositionY);
                DamageFeedback?.PlayFeedbacks(textPos , damage);
              //  onDamage?.Invoke();    
            }

            return isDead;
        }
    }
}

