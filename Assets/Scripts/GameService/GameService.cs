using HF.Core;
using TSoft.Core;
using Unity.Netcode;

namespace HF.GameService
{
    public abstract class GameService : Singleton<GameService>
    {
        public class ServerInfo
        {
            public string Name;
            public ulong Id;
            public int PlayerNumber;
            public int Ping;
            public string Description;
            public long CreatedTime;
        }
    
        protected NetworkManager networkManager;
    
        public const string k_Serverinfo = "ServerInfo";
    
        protected override void InitializeSingleton()
        {
            base.InitializeSingleton();

            if (networkManager == null || !TryGetComponent(out networkManager)) 
                networkManager = FindAnyObjectByType<NetworkManager>();
        }
    }
}