using Cysharp.Threading.Tasks;
using TSoft.InGame.GamePlaySystem;
using TSoft.Managers;

namespace TSoft.InGame.CardSystem.CE
{
    public class CE_ModifyAttr : CustomEffect
    {
        private const int DefaultDuration = 1;
        
        public override async UniTask ApplyEffect(InGameDirector director, Gameplay.AppliedGameplayEffect sourceEffect)
        {
            var gameplay = director.Player.Gameplay;
            foreach (var appliedModifier in sourceEffect.appliedModifiers)
            {
                gameplay.attrAppliedModifiers.Add(appliedModifier);
                EventManager.Instance.DmgAdderEvent.Raise(appliedModifier);
            }

            await UniTask.WaitForSeconds(sourceEffect.sourceEffect.hasDuration ? sourceEffect.sourceEffect.duration : DefaultDuration);
            
            gameplay.UpdateAttributes();
        }
    }
}
