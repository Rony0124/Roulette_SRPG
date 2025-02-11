using Cysharp.Threading.Tasks;
using TSoft.InGame.GamePlaySystem;

namespace TSoft.InGame.CardSystem.CE
{
    public class CE_ClearSelectedCard : CustomEffect
    {
        public override async UniTask ApplyEffect(InGameDirector director, Gameplay.AppliedGameplayEffect sourceEffect)
        {
            var player = director.Player;
            
            //카드 삭제
            foreach (var selectedCard in player.CurrentPokerCardSelected)
            {
                selectedCard.Dissolve(0.3f);
                        
                player.Discard(selectedCard);
            }
            
            player.CurrentPokerCardSelected.Clear();
            
            await UniTask.CompletedTask;
        }
    }
}
