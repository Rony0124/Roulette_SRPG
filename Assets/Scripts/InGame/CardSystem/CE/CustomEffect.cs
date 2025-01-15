using TSoft.InGame.Player;

namespace TSoft.InGame.CardSystem.CE
{
    public abstract class CustomEffect
    {
        public abstract void ApplyEffect(PlayerController player, MonsterController monster);
    }
}
