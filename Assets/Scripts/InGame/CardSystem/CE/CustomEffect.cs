using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TSoft.InGame.GamePlaySystem;
using TSoft.InGame.Player;

namespace TSoft.InGame.CardSystem.CE
{
    public class CustomEffect
    {
        public virtual async UniTask ApplyEffect(InGameDirector director)
        {
            await UniTask.CompletedTask;
        }
        
        public virtual async UniTask ApplyEffect(InGameDirector director, List<AppliedModifier> modifiers)
        {
            await UniTask.CompletedTask;
        }
    }
}

