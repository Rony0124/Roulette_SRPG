using System.Collections.Generic;
using TSoft.Core;
using TSoft.InGame.CardSystem;

namespace TSoft.Managers
{
    public class SpecialCardManager : Singleton<SpecialCardManager>
    {
        public List<SpecialCard> specialCards;

        protected override void Init()
        {
            specialCards = new List<SpecialCard>();
        }
    }
}
