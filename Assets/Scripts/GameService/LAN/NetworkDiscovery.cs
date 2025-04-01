/*
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HF.GameService;
using Unity.Netcode;
using UnityEngine;

namespace HF
{
   public sealed class NetworkDiscovery : MonoBehaviour
	{
		public class DiscoveryInfo
		{
			public readonly IPEndPoint EndPoint;
			public readonly IReadOnlyDictionary<string, string> KeyValuePairs;
			private float m_timeWhenReceived = 0f;
			public float TimeSinceReceived => Time.realtimeSinceStartup - m_timeWhenReceived;

			public DiscoveryInfo (IPEndPoint endPoint, Dictionary<string, string> keyValuePairs)
			{
				this.EndPoint = endPoint;
				this.KeyValuePairs = keyValuePairs;
				m_timeWhenReceived = Time.realtimeSinceStartup;
			}
		}
		
		//deprecated
		private static readonly byte[] OkBytes = { 1 };
		
		//이걸로 대체
		readonly Dictionary<string, string> m_responseData =
			new Dictionary<string, string> (System.StringComparer.InvariantCulture);
		
		private NetworkManager _networkManager;
		
		[SerializeField]
		[Tooltip("Secret to use when advertising or searching for servers.")]
		private string secret;
		private byte[] _secretBytes;
		
		[SerializeField]
		[Tooltip("Port to use when advertising or searching for servers.")]
		private ushort port;
		
		[SerializeField]
		[Tooltip("How long (in seconds) to wait for a response when advertising or searching for servers.")]
		private float searchTimeout;
		
		[SerializeField]
		private bool automatic;
		
		private SynchronizationContext _mainThreadSynchronizationContext;
		
		private CancellationTokenSource _cancellationTokenSource;
		
		public event Action<DiscoveryInfo> ServerFoundCallback;
		
		public bool CanAdvertise { get; private set; }
		
		public bool IsAdvertising { get; private set; }
		
		public bool IsSearching { get; private set; }
		
		private float SearchTimeout => searchTimeout < 1.0f ? 1.0f : searchTimeout;

		public const string kServerInfoKey = "ServerInfo";

		private void Awake()
		{
			if (_networkManager == null || !TryGetComponent(out _networkManager)) 
				_networkManager = FindAnyObjectByType<NetworkManager>();
			
			if (_networkManager != null)
			{
				LogInformation($"Using NetworkManager on {gameObject.name}.");

				_secretBytes = Encoding.UTF8.GetBytes(secret);
  
				_mainThreadSynchronizationContext = SynchronizationContext.Current;
				
				RegisterResponseData(kServerInfoKey, "add game mode");
			}
			else
			{
				LogError($"No NetworkManager found on {gameObject.name}. Component will be disabled.");

				enabled = false;
			}
		}

		private void OnEnable()
		{
			if (!automatic) return;

			_networkManager.ServerManager.OnServerConnectionState += ServerConnectionStateChangedEventHandler;
			
			_networkManager.ClientManager.OnClientConnectionState += ClientConnectionStateChangedEventHandler;
		}

		private void OnDisable()
		{
			Shutdown();
		}

		private void OnDestroy()
		{
			Shutdown();
		}

		private void OnApplicationQuit()
		{
			Shutdown();
		}

		private void Update()
		{
			bool isServer = false;
			
			#if UNITY_SERVER
			isServer = true;
			#endif
			
			#if UNITY_EDITOR
			isServer = WildDevPreferences.asServer;
			#endif
			if (isServer)
			{
				if (_networkManager.ServerManager.Started)
				{
					if (!IsAdvertising && CanAdvertise)
					{
						AdvertiseServer();
					}
				}
				
			}
		}

		/// <summary>
		/// Shuts down the NetworkDiscovery.
		/// </summary>
		private void Shutdown()
		{
			if (_networkManager != null)
			{
				_networkManager.ServerManager.OnServerConnectionState -= ServerConnectionStateChangedEventHandler;
				
				_networkManager.ClientManager.OnClientConnectionState -= ClientConnectionStateChangedEventHandler;
			}

			StopSearchingOrAdvertising();
		}

		private void ServerConnectionStateChangedEventHandler(ServerConnectionStateArgs args)
		{
			if (args.ConnectionState == LocalConnectionState.Started)
			{
				Debug.Log("Advertising...");
				AdvertiseServer();
			}
			else if (args.ConnectionState == LocalConnectionState.Stopped)
			{
				StopSearchingOrAdvertising();
			}
		}

		private void ClientConnectionStateChangedEventHandler(ClientConnectionStateArgs args)
		{
			if (_networkManager.IsServerStarted) return;

			if (args.ConnectionState == LocalConnectionState.Started)
			{
				StopSearchingOrAdvertising();
			}
			else if (args.ConnectionState == LocalConnectionState.Stopped)
			{
				SearchForServers();
			}
		}

		public void InitAdvertising()
		{
			CanAdvertise = true;

			AdvertiseServer();
		}
		
		public void AdvertiseServer()
		{
			if (IsAdvertising)
			{
				LogWarning("Server is already being advertised.");

				return;
			}

			_cancellationTokenSource = new CancellationTokenSource();

			AdvertiseServerAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
		}
		
		public void SearchForServers()
		{
			if (IsSearching)
			{
				LogWarning("Already searching for servers.");

				return;
			}

			_cancellationTokenSource = new CancellationTokenSource();

			SearchForServersAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
		}

		/// <summary>
		/// Stops searching or advertising.
		/// </summary>
		public void StopSearchingOrAdvertising()
		{
			if (_cancellationTokenSource == null)
			{
				LogWarning("Not searching or advertising.");

				return;
			}

			_cancellationTokenSource.Cancel();

			_cancellationTokenSource.Dispose();

			_cancellationTokenSource = null;
		}

		/// <summary>s
		/// Advertises the server on the local network.
		/// </summary>
		/// <param name="cancellationToken">Used to cancel advertising.</param>
		private async Task AdvertiseServerAsync(CancellationToken cancellationToken)
		{
			UdpClient udpClient = null;

			try
			{
				LogInformation("Started advertising server.");

				IsAdvertising = true;

				while (!cancellationToken.IsCancellationRequested)
				{
					udpClient ??= new UdpClient(port);

					LogInformation("Waiting for request...");

					Task<UdpReceiveResult> receiveTask = udpClient.ReceiveAsync();

					Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(SearchTimeout), cancellationToken);

					Task completedTask = await Task.WhenAny(receiveTask, timeoutTask);

					if (completedTask == receiveTask)
					{
						UdpReceiveResult result = receiveTask.Result;

						string receivedSecret = Encoding.UTF8.GetString(result.Buffer);

						if (receivedSecret == secret)
						{
							LogInformation($"Received request from {result.RemoteEndPoint}.");
							
							//현재 데이터를 추가해준다
							if (GameService.Instance != null)
							{
								var gameServiceLan = GameService.Instance as GameServiceServer;

								if (gameServiceLan != null)
								{
									var info = gameServiceLan.GetCurrentServerInfo();
									m_responseData[kServerInfoKey] = JsonUtility.ToJson(info);
								}
							}

							byte[] bytes = ConvertDictionaryToByteArray( m_responseData );
							//await udpClient.SendAsync(OkBytes, OkBytes.Length, result.RemoteEndPoint);
							await udpClient.SendAsync(bytes, bytes.Length, result.RemoteEndPoint);
						}
						else
						{
							LogWarning($"Received invalid request from {result.RemoteEndPoint}.");
						}
					}
					else
					{
						LogInformation("Timed out. Retrying...");

						udpClient.Close();

						udpClient = null;
					}
				}

				LogInformation("Stopped advertising server.");
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, this);
			}
			finally
			{
				IsAdvertising = false;

				LogInformation("Closing UDP client...");

				udpClient?.Close();
			}
		}

		/// <summary>
		/// Searches for servers on the local network.
		/// </summary>
		/// <param name="cancellationToken">Used to cancel searching.</param>
		private async Task SearchForServersAsync(CancellationToken cancellationToken)
		{
			UdpClient udpClient = null;

			try
			{
				LogInformation("Started searching for servers.");

				IsSearching = true;

				IPEndPoint broadcastEndPoint = new(IPAddress.Broadcast, port);

				while (!cancellationToken.IsCancellationRequested)
				{
					udpClient ??= new UdpClient();

					LogInformation("Sending request...");

					await udpClient.SendAsync(_secretBytes, _secretBytes.Length, broadcastEndPoint);

					LogInformation("Waiting for response...");

					Task<UdpReceiveResult> receiveTask = udpClient.ReceiveAsync();

					Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(SearchTimeout), cancellationToken);

					Task completedTask = await Task.WhenAny(receiveTask, timeoutTask);

					if (completedTask == receiveTask)
					{
						UdpReceiveResult result = receiveTask.Result;

						if (result.Buffer.Length > 0 && result.Buffer != null)
						{
							LogInformation($"Received response from {result.RemoteEndPoint}.");
							var dict = ConvertByteArrayToDictionary (result.Buffer);
							foreach (var kvp in dict)
							{
								Debug.Log($"[test] found server key : {kvp.Key}");
								Debug.Log($"[test] found server val : {kvp.Value}");
							}

							var info = new DiscoveryInfo(result.RemoteEndPoint, dict);
							
							_mainThreadSynchronizationContext.Post(delegate { ServerFoundCallback?.Invoke(info); }, null);
						}
						else
						{
							LogWarning($"Received invalid response from {result.RemoteEndPoint}.");
						}
					}
					else
					{
						LogInformation("Timed out. Retrying...");

						udpClient.Close();

						udpClient = null;
					}
				}

				LogInformation("Stopped searching for servers.");
			}
			catch (SocketException socketException)
			{
				if (socketException.SocketErrorCode == SocketError.AddressAlreadyInUse)
				{
					LogError($"Unable to search for servers. Port {port} is already in use.");
				}
				else
				{
					Debug.LogException(socketException, this);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, this);
			}
			finally
			{
				IsSearching = false;

				udpClient?.Close();
			}
		}

		/// <summary>
		/// Logs a message if the NetworkManager can log.
		/// </summary>
		/// <param name="message">Message to log.</param>
		private void LogInformation(string message)
		{
			if (NetworkManagerExtensions.CanLog(LoggingType.Common)) Debug.Log($"[{nameof(NetworkDiscovery)}] {message}", this);
		}

		/// <summary>
		/// Logs a warning if the NetworkManager can log.
		/// </summary>
		/// <param name="message">Message to log.</param>
		private void LogWarning(string message)
		{
			if (NetworkManagerExtensions.CanLog(LoggingType.Warning)) Debug.LogWarning($"[{nameof(NetworkDiscovery)}] {message}", this);
		}

		/// <summary>
		/// Logs an error if the NetworkManager can log.
		/// </summary>
		/// <param name="message">Message to log.</param>
		private void LogError(string message)
		{
			if (NetworkManagerExtensions.CanLog(LoggingType.Error)) Debug.LogError($"[{nameof(NetworkDiscovery)}] {message}", this);
		}
		
		public void RegisterResponseData( string key, string value )
		{
			m_responseData[key] = value;
		}

		public void UnRegisterResponseData( string key )
		{
			m_responseData.Remove (key);
		}
		
		public static string ConvertDictionaryToString( Dictionary<string, string> dict )
		{
			return string.Join( "\n", dict.Select( pair => pair.Key + ": " + pair.Value ) );
		}

		public static Dictionary<string, string> ConvertStringToDictionary( string str )
		{
			var dict = new Dictionary<string, string>(System.StringComparer.InvariantCulture);
			string[] lines = str.Split("\n".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
			foreach(string line in lines)
			{
				int index = line.IndexOf(": ");
				if(index < 0)
					continue;
				dict[line.Substring(0, index)] = line.Substring(index + 2, line.Length - index - 2);
			}
			return dict;
		}

		public static byte[] ConvertDictionaryToByteArray( Dictionary<string, string> dict )
		{
			return ConvertStringToPacketData( ConvertDictionaryToString( dict ) );
		}

		public static Dictionary<string, string> ConvertByteArrayToDictionary( byte[] data )
		{
			return ConvertStringToDictionary( ConvertPacketDataToString( data ) );
		}

		public static byte[] ConvertStringToPacketData(string str)
		{
			byte[] data = new byte[str.Length * 2];
			for (int i = 0; i < str.Length; i++)
			{
				ushort c = str[i];
				data[i * 2] = (byte) ((c & 0xff00) >> 8);
				data[i * 2 + 1] = (byte) (c & 0x00ff);
			}
			return data;
		}

		public static string ConvertPacketDataToString(byte[] data)
		{
			char[] arr = new char[data.Length / 2];
			for (int i = 0; i < arr.Length; i++)
			{
				ushort b1 = data[i * 2];
				ushort b2 = data[i * 2 + 1];
				arr[i] = (char)((b1 << 8) | b2);
			}
			return new string(arr);
		}
	}
}
*/
