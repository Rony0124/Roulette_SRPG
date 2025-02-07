using Cysharp.Threading.Tasks;
using TSoft.InGame.Player;
using UnityEngine;

namespace TSoft.InGame.CardSystem.CE
{
    public class CE_Repeat : CustomEffect
    {
        public override async UniTask ApplyEffect(InGameDirector director)
        {
            var player = director.Player;
            var monster = director.CurrentMonster;
            
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
