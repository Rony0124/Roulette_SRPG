using System;
using HF.Data.Card;
using HF.Utils;
using TSoft.Data.Card;
using Unity.Netcode;

namespace HF.API
{
    [Serializable]
    public class UserDeckData : INetworkSerializable
    {
        public UserArtifactData[] artifacts;
        public UserCardData[] cards;

        public UserDeckData()
        {
            cards = Array.Empty<UserCardData>();
            artifacts = Array.Empty<UserArtifactData>();
        }

        public UserDeckData(DeckData deck)
        {
            artifacts = new UserArtifactData[deck.artifacts.Length];
            cards = new UserCardData[deck.cards.Length];
            
            for (int i = 0; i < deck.artifacts.Length; i++)
            {
                artifacts[i] = new UserArtifactData(deck.artifacts[i]);
            }
            
            for (int i = 0; i < deck.cards.Length; i++)
            {
                cards[i] = new UserCardData(deck.cards[i]);
            }
        }
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            NetUtil.NetSerializeArray(serializer, ref artifacts);
            NetUtil.NetSerializeArray(serializer, ref cards);
        }

        public static UserDeckData Default
        {
            get
            {
                var deck = new UserDeckData
                {
                    cards = Array.Empty<UserCardData>(),
                    artifacts = Array.Empty<UserArtifactData>()
                };
                return deck;
            }
        }
    }
    
    [Serializable]
    public class UserArtifactData : INetworkSerializable
    {
        public byte[] tid;

        public UserArtifactData() { }
        
        public UserArtifactData(ArtifactData artifact)
        {
            tid = artifact != null ? artifact.Id.Value.ToByteArray() : new byte[16];
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref tid);
        }
    }
    
    [Serializable]
    public class UserCardData : INetworkSerializable
    {
        public byte[] tid;

        public UserCardData() { }
        
        public UserCardData(CardData card)
        {
            tid = card != null ? card.Id.Value.ToByteArray() : new byte[16];
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref tid);
        }
    }
}
