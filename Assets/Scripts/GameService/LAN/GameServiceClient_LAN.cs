/*
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace HF.GameService.LAN
{
    public sealed class GameServiceClient_LAN : GameServiceClient
    {
        [Tooltip("게임 연결에 사용 되는 연결 포트")]
        [SerializeField] private ushort port;
    
        private List<IPEndPoint> _addresses = new();
    
        private NetworkDiscovery _networkDiscovery;

        public override bool IsSearching => _networkDiscovery.IsSearching;
    
        protected override void InitializeSingleton()
        {
            base.InitializeSingleton();
    
            if (_networkDiscovery == null || !TryGetComponent(out _networkDiscovery)) 
                _networkDiscovery = FindAnyObjectByType<NetworkDiscovery>();
        
            _networkDiscovery.ServerFoundCallback += AddAddress;
        }
    
        private void AddAddress(NetworkDiscovery.DiscoveryInfo info)
        {
            if (_addresses.Contains(info.EndPoint)) 
                return;
        
            var stringInfo = info.KeyValuePairs[NetworkDiscovery.kServerInfoKey];
            var serverInfo = JsonUtility.FromJson<ServerInfo>(stringInfo);
            
            _addresses.Add(info.EndPoint); 
            OnServerFound?.Invoke(serverInfo);
        }
    
        public override void StopSearching()
        {
            if(_networkDiscovery.IsSearching || _networkDiscovery.IsAdvertising)
                _networkDiscovery.StopSearchingOrAdvertising();
        }

        public override void StartSearching()
        {
            _networkDiscovery.SearchForServers();
        }

        public override void Reset()
        {
            _addresses.Clear();
        }

        public override void Join(string ip)
        {
            StopSearching();

            networkManager.ClientManager.StartConnection(ip, port);
        }
    }

}
*/
