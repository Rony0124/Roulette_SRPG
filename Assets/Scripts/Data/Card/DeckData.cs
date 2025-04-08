using System;
using TSoft.Data.Card;

namespace HF.Data.Card
{
    [Serializable]
    public class DeckData
    {
        public CardData[] cards;
        public ArtifactData[] artifacts;
    }
}
