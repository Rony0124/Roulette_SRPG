using Cysharp.Threading.Tasks;
using InGame;
using TSoft.InGame.GamePlaySystem;
using UnityEngine;

namespace TSoft.InGame.CardSystem.CE
{
    public class CE_DrawCard : CustomEffect
    {
        /*public override async UniTask ApplyEffect(InGameDirector director, Gameplay.AppliedGameplayEffect appliedEffect)
        {
            foreach (var modifier in appliedEffect.sourceEffect.gameplayEffect.modifiers)
            {
                var repeater = (int)modifier.gameplayMagnitude.magnitude;
                for (var i = 0; i < repeater; i++)
                {
                    director.Player.DrawCards();
                }
            }
            
            await UniTask.Delay(100);
        }*/
    }
}
