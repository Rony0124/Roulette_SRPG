using System;

namespace HF.GameService
{
    public abstract class GameServiceServer : TSoft.GameService.GameService
    {
        protected long createdTime;
    
        protected override void InitializeSingleton()
        {
            base.InitializeSingleton();

            createdTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        public abstract ServerInfo GetCurrentServerInfo();

        public abstract void StartServer();
    }
}
