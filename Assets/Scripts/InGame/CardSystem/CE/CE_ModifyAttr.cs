using Cysharp.Threading.Tasks;
using TSoft.InGame.GamePlaySystem;

namespace TSoft.InGame.CardSystem.CE
{
    public class CE_ModifyAttr : CustomEffect
    {
        public override async UniTask ApplyEffect(InGameDirector director, GameplayEffectSO sourceEffect)
        {
            var gameplay = director.Player.Gameplay;
            foreach (var appliedModifier in sourceEffect.appliedModifiers)
            {
                gameplay.attrAppliedModifiers.Add(appliedModifier);    
            }
            
            gameplay.UpdateAttributes();

            await UniTask.CompletedTask;
        }
        
        public override async UniTask UndoEffect(InGameDirector director, GameplayEffectSO sourceEffect)
        {
            var gameplay = director.Player.Gameplay;
            foreach (var appliedModifier in sourceEffect.appliedModifiers)
            {
                gameplay.attrAppliedModifiers.Remove(appliedModifier);    
            }
            
            gameplay.UpdateAttributes();

            await UniTask.CompletedTask;
        }
    }
}
