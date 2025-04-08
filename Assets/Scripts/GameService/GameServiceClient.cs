using System;

namespace HF.GameService
{
    public abstract class GameServiceClient : TSoft.GameService.GameService
    {
        public Action<ServerInfo> OnServerFound;
    
        /*private PlayerConnection _playerConnection; 
        public PlayerConnection PlayerConnection
        {
            get => _playerConnection;
            set
            {
                _playerConnection = value;
                DontDestroyOnLoad(_playerConnection.gameObject);
            }
        
        }*/
    
        public abstract bool IsSearching { get; }
    
        public abstract void StopSearching();

        public abstract void StartSearching();

        public abstract void Reset();

        public abstract void Join(string target);
    }
}
