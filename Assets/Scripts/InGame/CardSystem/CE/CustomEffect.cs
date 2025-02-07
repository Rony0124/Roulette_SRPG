using System.Collections.Generic;
using TSoft.InGame.GamePlaySystem;
using Cysharp.Threading.Tasks;

namespace TSoft.InGame.CardSystem.CE
{
    public class CustomEffect
    {
        public virtual async UniTask ApplyEffect(InGameDirector director)
        {
            await UniTask.CompletedTask;
        }
        
        public virtual async UniTask ApplyEffect(InGameDirector director, Gameplay.AppliedGameplayEffect sourceEffect)
        {
            await UniTask.CompletedTask;
        }
        
        public virtual async UniTask UndoEffect(InGameDirector director, Gameplay.AppliedGameplayEffect sourceEffect)
        {
            await UniTask.CompletedTask;
        }
    }
}

