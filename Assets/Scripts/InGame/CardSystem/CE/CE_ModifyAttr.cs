using Cysharp.Threading.Tasks;
using TSoft.InGame.GamePlaySystem;

namespace TSoft.InGame.CardSystem.CE
{
    public class CE_ModifyAttr : CustomEffect
    {
        public override async UniTask ApplyEffect(InGameDirector director, Gameplay.AppliedGameplayEffect sourceEffect)
        {
            var gameplay = director.Player.Gameplay;
            foreach (var appliedModifier in sourceEffect.appliedModifiers)
            {
                gameplay.attrAppliedModifiers.Add(appliedModifier);    
            }
            
            gameplay.UpdateAttributes();

            await UniTask.CompletedTask;
        }
        
        public override async UniTask UndoEffect(InGameDirector director, Gameplay.AppliedGameplayEffect sourceEffect)
        {
            var gameplay = director.Player.Gameplay;

            /*switch (sourceEffect.sourceEffect.lifeCycle.end)
            {
                case GameplayPolicyType.OnRoundFinished :
                    gameplay.appliedEffects_OnRoundBegin.Remove(sourceEffect);
                    break;
                case GameplayPolicyType.OnTurnFinished :
                    gameplay.appliedEffects_OnTurnBegin.Remove(sourceEffect);
                    break;
            }*/

            await UniTask.CompletedTask;
        }
    }
}
