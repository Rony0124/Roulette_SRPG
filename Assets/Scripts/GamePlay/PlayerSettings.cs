using Unity.Netcode;

namespace HF.GamePlay
{
    [System.Serializable]
    public class PlayerSettings : INetworkSerializable
    {
        public string username;
        public string avatar;
        public string cardback;
        public int ai_level;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref username);
            serializer.SerializeValue(ref avatar);
            serializer.SerializeValue(ref cardback);
            serializer.SerializeValue(ref ai_level);
        }

        public static PlayerSettings DefaultAI
        {
            get
            {
                PlayerSettings settings = new PlayerSettings();
                settings.username = "AI";
                settings.avatar = "";
                settings.cardback = "";
                settings.ai_level = 10;
                return settings;
            }
        }

    }
}
