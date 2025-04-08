using Cysharp.Threading.Tasks;
using InGame;
using UnityEngine;

namespace TSoft.InGame.CardSystem.CE
{
    public class CE_EnhanceCardOnHand : CustomEffect
    {
        /*public override async UniTask ApplyEffect(InGameDirector director)
        {
            var player = director.Combat.Player;
            player.isSelectingCardOnHand = true;
            player.onClickCard = TryClickCard;
            
            await UniTask.CompletedTask;
        }*/

        private bool TryClickCard(PokerCard card)
        {
            var data = card.cardData;
            if (data.type == CardType.Joker)
            {
                Debug.Log("Warning joker");
                return false;
            }
            
            if(data.type == CardType.None)
            {
                Debug.Log("Warning none");
                return false;
            }

            Debug.Log("Enhance Card!");
            return true;
        }
    }
}
