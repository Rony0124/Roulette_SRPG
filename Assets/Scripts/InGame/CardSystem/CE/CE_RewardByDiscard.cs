using Cysharp.Threading.Tasks;
using TSoft.InGame.GamePlaySystem;
using TSoft.Save;

namespace TSoft.InGame.CardSystem.CE
{
    public class CE_RewardByDiscard : CustomEffect
    {
        public override async UniTask ApplyEffect(InGameDirector director, Gameplay.AppliedGameplayEffect appliedEffect)
        {
            var currentGold = GameSave.Instance.Gold;
            var currentEnergy = director.Combat.Player.Gameplay.GetAttr(GameplayAttr.Energy);
        
            foreach (var modifier in appliedEffect.sourceEffect.gameplayEffect.modifiers)
            {
                if (modifier.attrType != GameplayAttr.None)
                {
                    continue;
                }

                var magnitude = (int)currentEnergy * (int)modifier.gameplayMagnitude.magnitude;
            
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
