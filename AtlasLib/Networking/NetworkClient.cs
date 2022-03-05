using System;
using System.Net.Sockets;

using LiteNetLib;

using AtlasLib.Utils;

namespace AtlasLib.Networking
{
    public class NetworkClient
    {
        public const int MilliTimeout = 10000;

        public event Action<ConnectionRequest> OnConnectionRequest;
        public event Action<SocketError> OnConnectionError;
        public event Action<NetPeer> OnConnectionEstablished;
        public event Action<NetPeer, DisconnectInfo> OnConnectionTerminated;

        public NetworkConnection connection { get; private set; }
        public NetManager netManager { get; }
        public NetPeer netPeer { get; private set; }
        public EventBasedNetListener eventBasedNetListener { get; }

        public bool isConnected { get => connection != null && connection.isConnected; }
        public bool isServer { get; }

        public string clientType { get; }

        public NetworkClient(string clientType, bool isServer)
        {
            this.clientType = clientType;
            this.isServer = isServer;

            eventBasedNetListener = new EventBasedNetListener();
            netManager = new NetManager(eventBasedNetListener);

            netManager.AutoRecycle = true;
            netManager.BroadcastReceiveEnabled = true;
            netManager.DisconnectOnUnreachable = true;
            netManager.DisconnectTimeout = MilliTimeout;
            netManager.EnableStatistics = true;
            netManager.IPv6Enabled = IPv6Mode.Disabled;
            netManager.MaxConnectAttempts = 5;

            eventBasedNetListener.ConnectionRequestEvent += (x) =>
            {
                OnConnectionRequest(x);
            };

            eventBasedNetListener.NetworkErrorEvent += (x, e) =>
            {
                OnConnectionError(e);
            };

            eventBasedNetListener.PeerConnectedEvent += (x) =>
            {
                netPeer = x;
                connection = new NetworkConnection(clientType, eventBasedNetListener, netPeer);
                OnConnectionEstablished(x);
            };

            eventBasedNetListener.PeerDisconnectedEvent += (x, e) =>
            {
                netPeer = null;
                connection = null;
                OnConnectionTerminated(x, e);
            };
        }

        public void Start(string address, int port)
        {
            netManager.Start();
            netManager.Connect(address, port, RandomGen.RandomString());
        }

        public void Stop()
        {
            Assert.NotNull(netPeer);

            netManager.DisconnectPeerForce(netPeer);
            netManager.Stop();
        }
    }
}
