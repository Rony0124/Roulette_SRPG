using Cysharp.Threading.Tasks;
using TSoft.InGame.GamePlaySystem;

namespace TSoft.InGame.CardSystem.CE
{
    public class CE_GrantGold : CustomEffect
    {
        public override async UniTask ApplyEffect(InGameDirector director, Gameplay.AppliedGameplayEffect appliedEffect)
        {
            var currentGold = GameSave.Instance.Gold;
        
            foreach (var modifier in appliedEffect.sourceEffect.gameplayEffect.modifiers)
            {
                var magnitude = (int)modifier.gameplayMagnitude.magnitude;
            
                switch (modifier.modifierOp)
                {
                    case ModifierOpType.Add:
                        currentGold += magnitude;
                        break;
                    case ModifierOpType.Multiply:
                        currentGold *= magnitude;
                        break;
                    case ModifierOpType.Divide:
                        currentGold /= magnitude;
                        break;
                }
            }
        
            GameSave.Instance.SetGold(currentGold);
        
            await UniTask.CompletedTask;
        }
    }
}
