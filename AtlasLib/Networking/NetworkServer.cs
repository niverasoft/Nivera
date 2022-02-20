using System;
using System.Net.Sockets;
using System.Collections.Generic;

using LiteNetLib;

namespace AtlasLib.Networking
{
    public class NetworkServer
    {
        public const int MilliTimeout = 10000;

        public event Action<ConnectionRequest> OnConnectionRequest;
        public event Action<SocketError> OnConnectionError;
        public event Action<NetPeer> OnConnectionEstablished;
        public event Action<NetPeer, DisconnectInfo> OnConnectionTerminated;

        private static List<NetworkClient> connectedClients { get; } = new List<NetworkClient>();

        public EventBasedNetListener eventBasedNetListener { get; }
        public NetManager netManager { get; }

        public bool isRunning { get => netManager != null && netManager.IsRunning; }

        public NetworkServer()
        {
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
                OnConnectionEstablished(x);
            };

            eventBasedNetListener.PeerDisconnectedEvent += (x, e) =>
            {
                OnConnectionTerminated(x, e);
            };
        }

        public void Start()
        {
            netManager.Start();
        }

        public void Stop()
        {
            connectedClients.ForEach(x => x.Stop());

            netManager.Stop();
        }
    }
}
