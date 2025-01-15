using System;
using MoreMountains.Feedbacks;
using TSoft.Data.Monster;
using TSoft.InGame.GamePlaySystem;
using UnityEngine;

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

        private bool isDead;
        public bool IsDead => isDead; 
        
        [SerializeField] private MMFeedbacks DamageFeedback;
        [SerializeField] private MMFeedbacks DeathFeedback;
        [SerializeField] private MMFeedbacks DamageTextFeedback;

        private void Awake()
        {
            gameplay = GetComponent<Gameplay>();
            gameplay.Init();
        }

        public void TakeDamage(int damage, bool ignoreFeedback = false)
        {
            DamageTextFeedback?.PlayFeedbacks(transform.position , damage);
            
            var currentHp = GamePlay.GetAttr(GameplayAttr.Heart);
            currentHp = Math.Max(0, currentHp - damage);
            
            GamePlay.SetAttr(GameplayAttr.Heart, currentHp);
            isDead = currentHp <= 0;
            
            if (isDead)
            {
                if (ignoreFeedback)
                    return;
                    
                DeathFeedback?.PlayFeedbacks();
            }
            else
            {
                onDamaged?.Invoke(currentHp);

                if (!ignoreFeedback)
                {
                    DamageFeedback?.PlayFeedbacks();    
                }
            }
        }
    }
}

