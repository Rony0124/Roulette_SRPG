using System;

namespace HF.Data.Card
{
    [Serializable]
    public class DeckData
    {
        public CardData[] cards;
        public ArtifactData[] artifacts;
    }
}
