using TSoft.InGame.Player;
using UnityEngine;

namespace TSoft.InGame.CardSystem.CE
{
    public class CE_Shuffle : CustomEffect
    {
        public override void ApplyEffect(PlayerController player, MonsterController monster)
        {
            player.RetrieveAllCards();
            player.ShuffleCurrent();
            player.DrawCards();
        }
    }
}
