using Cysharp.Threading.Tasks;
using InGame;
using TSoft.InGame.GamePlaySystem;
using TSoft.Managers.Event;

namespace TSoft.InGame.CardSystem.CE
{
    public class CE_ModifyAttr : CustomEffect
    {
        private const int DefaultDuration = 1;
        
        /*
        public override async UniTask ApplyEffect(InGameDirector director, Gameplay.AppliedGameplayEffect appliedEffect)
        {
            var gameplay = director.Player.Gameplay;
            
            EventManager.Instance.GameEvent.Raise(appliedEffect.sourceEffect.Id);
            
            foreach (var appliedModifier in appliedEffect.appliedModifiers)
            {
                gameplay.attrAppliedModifiers.Add(appliedModifier);
                EventManager.Instance.DmgAdderEvent.Raise(appliedModifier);
            }

            await UniTask.WaitForSeconds(appliedEffect.sourceEffect.hasDuration ? appliedEffect.sourceEffect.duration : DefaultDuration);
            
            gameplay.UpdateAttributes();
        }*/
    }
}
