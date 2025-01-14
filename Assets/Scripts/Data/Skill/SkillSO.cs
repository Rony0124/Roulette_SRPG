using UnityEngine;
using TSoft.InGame;
using TSoft.InGame.CardSystem.CE;
using TSoft.InGame.GamePlaySystem;
using TSoft.InGame.Player;

namespace TSoft.Data.Skill
{
    [CreateAssetMenu(fileName = "Skill", menuName = "Data/Skill", order = 2)]
    public class SkillSO : ItemSO
    {
        public GameplayEffectSO[] gamePlayEffects;
        [SerializeReference]
        public CustomEffect effect;

        public GameObject skillParticleObj;

        public void PlaySkill(PlayerController player, MonsterController monster)
        {
            if (gamePlayEffects is { Length: > 0 })
            {
                //스킬 effect 데미지 계산
                foreach (var gameplayEffect in gamePlayEffects)
                {
                    player.Gameplay.ApplyEffectSelf(gameplayEffect);
                }    
            }

            if (effect is not null)
            {
                //스킬 추과 효과 존재하면 적용
                effect.ApplyEffect(player);    
            }
            
            //파티클 시스템 가져오기
            if (player.particleDictionary.TryGetValue(player.CurrentPattern.PatternType, out var particleSystem))
            {
                particleSystem.transform.position = monster.transform.position + Vector3.up; 
                particleSystem.Play();    
            }
        }
    }
}
