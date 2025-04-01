using UnityEngine;

namespace HF.GameService.LAN
{
    public sealed class GameServiceServer_LAN : GameServiceServer
    {
        public override ServerInfo GetCurrentServerInfo()
        {
            return new ServerInfo
            {
                CreatedTime = createdTime,
                Description = "Something funny is going on"
            };
        }

        public override void StartServer()
        {
        }

        public void Advertise()
        {
            /*var networkDiscovery = FindObjectOfType<NetworkDiscovery>();
            if(networkDiscovery)
                networkDiscovery.AdvertiseServer();*/
        }
    }
}
