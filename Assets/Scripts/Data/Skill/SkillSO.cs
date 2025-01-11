using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace TSoft.Data.Skill
{
    [CreateAssetMenu(fileName = "Skill", menuName = "Data/Skill", order = 2)]
    public class SkillSO : ItemSO
    {
        public GameplayEffectSO[] effects;
        [SerializeReference]
        public CustomEffect effect;

        public void PlaySkill(PlayerControllelr player)
        {
            //스킬 effect 데미지 계산
            foreach (var effect in effects)
            {
                player.Gameplay.ApplyEffectSelf(effect);
            }
            
            //스킬 추과 효과 존재하면 적용
            effect.ApplyEffect(player);
        }
    }
}
