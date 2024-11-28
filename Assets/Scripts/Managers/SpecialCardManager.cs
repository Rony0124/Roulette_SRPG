using System.Collections.Generic;
using TSoft.Core;
using TSoft.InGame.CardSystem;

namespace TSoft.Managers
{
    public class SpecialCardManager : Singleton<SpecialCardManager>
    {
        public List<SpecialCard> specialCards;

        private void Awake()
        {
            specialCards = new List<SpecialCard>();
        }
    }
}
