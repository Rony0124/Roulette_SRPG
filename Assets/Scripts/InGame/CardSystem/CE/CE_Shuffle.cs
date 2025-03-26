using Cysharp.Threading.Tasks;
using InGame;
using TSoft.InGame.Player;
using UnityEngine;

namespace TSoft.InGame.CardSystem.CE
{
    public class CE_Shuffle : CustomEffect
    {
        public override async UniTask ApplyEffect(InGameDirector director)
        {
            var player = director.Player;
            
            player.RetrieveAllCards();
            await UniTask.Delay(10);
            player.ShuffleCurrent();
            await UniTask.Delay(10);
            player.DrawCards();
        }
    }
}
