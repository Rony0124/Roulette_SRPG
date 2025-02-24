using Cysharp.Threading.Tasks;
using TSoft.InGame.GamePlaySystem;
using UnityEngine;

namespace TSoft.InGame.CardSystem.CE
{
    public class CE_DrawCard : CustomEffect
    {
        public override async UniTask ApplyEffect(InGameDirector director, Gameplay.AppliedGameplayEffect sourceEffect)
        {
            foreach (var modifier in sourceEffect.sourceEffect.gameplayEffect.modifiers)
            {
                var repeater = (int)modifier.gameplayMagnitude.magnitude;
                for (var i = 0; i < repeater; i++)
                {
                    director.Player.DrawCards();
                }
            }
            
            await UniTask.Delay(100);
        }
    }
}
