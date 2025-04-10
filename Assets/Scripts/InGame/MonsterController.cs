using System;
using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using TSoft.Data.Monster;
using TSoft.InGame.GamePlaySystem;
using TSoft.InGame.Player;
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
        
        [SerializeField] private MMFeedbacks IntroFeedback;
        [SerializeField] private MMFeedbacks DamageFeedback;
        [SerializeField] private MMFeedbacks DeathFeedback;
        [SerializeField] private MMFeedbacks DamageTextFeedback;

        private void Awake()
        {
            gameplay = GetComponent<Gameplay>();
            gameplay.Init();
        }

        public async UniTask OnIntro()
        {
            if (!IntroFeedback)
                return;
            
            IntroFeedback.PlayFeedbacks();

            await UniTask.WaitForSeconds(IntroFeedback.TotalDuration);
        }

        public void AttackPlayer(PlayerController player)
        {
            Debug.Log("Attacking player");
            
            CombatController.Instance.EndTurn().Forget();
        }

        public void TakeDamage(int damage, bool ignoreFeedback = false)
        {
            DamageTextFeedback.PlayFeedbacks(transform.position , damage);
            
            var currentHp = GamePlay.GetAttr(GameplayAttr.Heart);
            currentHp = Math.Max(0, currentHp - damage);
            
            GamePlay.SetAttr(GameplayAttr.Heart, currentHp);
            isDead = currentHp <= 0;
            
            onDamaged?.Invoke(currentHp);
            
            if (isDead)
            {
                if (ignoreFeedback)
                    return;
                    
                DeathFeedback.PlayFeedbacks();
            }
            else
            {
                if (!ignoreFeedback)
                {
                    DamageFeedback.PlayFeedbacks();    
                }
            }
        }
    }
}

