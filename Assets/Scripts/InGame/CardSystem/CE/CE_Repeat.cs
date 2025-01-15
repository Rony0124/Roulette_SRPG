using Cysharp.Threading.Tasks;
using TSoft.InGame.Player;
using UnityEngine;

namespace TSoft.InGame.CardSystem.CE
{
    public class CE_Repeat : CustomEffect
    {
        public override void ApplyEffect(PlayerController player, MonsterController monster)
        {
            Repeat(player, monster).Forget();
        }

        private async UniTaskVoid Repeat(PlayerController player, MonsterController monster)
        {
            await UniTask.WaitForSeconds(0.3f);
            
            if (player.particleDictionary.TryGetValue(player.CurrentPattern.PatternType, out var particleSystem))
            {
                particleSystem.transform.position = monster.transform.position + Vector3.up; 
                particleSystem.Play();    
            }
            
            monster.TakeDamage((int)player.CurrentDmg, true);
        }
    }
}
