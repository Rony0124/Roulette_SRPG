using System.Collections.Generic;
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
        public List<Modifier> modifiers;
        [SerializeReference]
        public CustomEffect effect;

        public GameObject skillParticleObj;

        public void PlaySkill(PlayerController player, MonsterController monster)
        {
            var dmg = player.CurrentDmg;
            foreach (var modifier in modifiers)
            {
                if (modifier.modifierOp == ModifierOpType.Add)
                {
                    dmg += modifier.magnitude;
                }
                else if(modifier.modifierOp == ModifierOpType.Multiply)
                {
                    dmg *= modifier.magnitude;
                }
                else
                {
                    dmg = modifier.magnitude;
                }
            }
          
            player.CurrentDmg = dmg;

            if (effect is not null)
            {
                //스킬 추과 효과 존재하면 적용
                effect.ApplyEffect(player, monster);
            }
            
            //파티클 시스템 가져오기
            if (player.particleDictionary.TryGetValue(player.CurrentPattern.PatternType, out var particleSystem))
            {
                particleSystem.transform.position = monster.transform.position + Vector3.up; 
                particleSystem.Play();    
            }
            
            monster.TakeDamage((int)player.CurrentDmg);
        }
    }
}
