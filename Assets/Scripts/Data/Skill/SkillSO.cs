using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using TSoft.InGame;
using TSoft.InGame.GamePlaySystem;
using TSoft.InGame.Player;

namespace TSoft.Data.Skill
{
    [CreateAssetMenu(fileName = "Skill", menuName = "Data/Skill", order = 2)]
    public class SkillSO : ItemSO
    {
        public GameplayEffectSO effect;
        
        public GameObject skillParticleObj;

        public float skillDuration = 2.0f;
        
        public int skillRepeater = 1;

        public void ApplySkill(Gameplay gameplay)
        {
            if(effect)
                gameplay.AddEffect(effect);
        }

        public async UniTask PlaySkill(PlayerController player, MonsterController monster)
        {
            var basicDamage = player.Gameplay.GetAttr(GameplayAttr.BasicAttackPower);
            var skillDamage = player.Gameplay.GetAttr(GameplayAttr.SkillAttackPower);
          
            var dmg = basicDamage * skillDamage;

            for (var i = 0; i < skillRepeater; i++)
            {
                //파티클 시스템 가져오기
                if (player.particleDictionary.TryGetValue(player.CurrentPattern.PatternType, out var particleSystem))
                {
                    particleSystem.transform.position = monster.transform.position + Vector3.up; 
                    particleSystem.Play();
                }
            
                monster.TakeDamage((int)dmg);
                
                await UniTask.Delay(500);
            }
            
            await UniTask.Delay((int)skillDuration * 1000);
        }
    }
}
