using Unity.Netcode;
using UnityEngine;

namespace HF.GamePlay
{
    [System.Serializable]
    public enum GameType
    {
        Solo = 0,
        Multiplayer = 10,
    }
    
    [System.Serializable]
    public class GameSettings : INetworkSerializable
    {
        public string server_url;
        public string game_uid;
        public string scene; 
        public int nb_players;
        
        public GameType game_type = GameType.Solo; 
        
        public virtual bool IsOffline()
        {
            return game_type == GameType.Solo;
        }
        
        public virtual void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref server_url);
            serializer.SerializeValue(ref game_uid);
            serializer.SerializeValue(ref scene);
            serializer.SerializeValue(ref game_type);
            serializer.SerializeValue(ref nb_players);
        }
        
        public static GameSettings Default
        {
            get
            {
                GameSettings settings = new GameSettings();
                settings.server_url = "";
                settings.game_uid = "test";
                settings.game_type = GameType.Solo;
                settings.nb_players = 2;
                settings.scene = "Game";
                return settings;
            }
        }
    }
    
    
}
