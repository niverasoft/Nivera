using System;
using System.Net.Sockets;
using System.Collections.Generic;

using LiteNetLib;

namespace Nivera.Networking
{
    public class NetworkServer
    { 
        public const int MilliTimeout = 10000;

        public event Action<ConnectionRequest> OnConnectionRequest;
        public event Action<SocketError> OnConnectionError;
        public event Action<NetPeer, NetworkConnection> OnConnectionEstablished;
        public event Action<NetPeer, DisconnectInfo> OnConnectionTerminated;

        private static List<NetworkConnection> connectedClients { get; } = new List<NetworkConnection>();

        public EventBasedNetListener eventBasedNetListener { get; }
        public NetManager netManager { get; }

        public string sessionKey { get; set; }

        public bool isRunning { get => netManager != null && netManager.IsRunning; }

        public NetworkServer(string sessionKey = null)
        {
            sessionKey = sessionKey == null ? Encryption.EncryptionKey.GenerateKey(NetworkConnection.SessionKeyMaxLen) : sessionKey;

            eventBasedNetListener = new EventBasedNetListener();
            netManager = new NetManager(eventBasedNetListener);
            netManager.AutoRecycle = true;
            netManager.BroadcastReceiveEnabled = true;
            netManager.DisconnectOnUnreachable = true;
            netManager.DisconnectTimeout = MilliTimeout;
            netManager.EnableStatistics = true;
            netManager.IPv6Mode = IPv6Mode.Disabled;
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
                var connection = new NetworkConnection(netManager, eventBasedNetListener, true, sessionKey, x);

                OnConnectionEstablished(x, connection);

                connectedClients.Add(connection);
            };

            eventBasedNetListener.PeerDisconnectedEvent += (x, e) =>
            {
                OnConnectionTerminated(x, e);

                connectedClients.Find(z => z.netPeer.Id == x.Id).Disconnect();
                connectedClients.RemoveAt(connectedClients.FindIndex(z => z.netPeer.Id == x.Id));
            };
        }

        public void Start()
        {
            netManager.Start();
        }

        public void Stop()
        {
            connectedClients.ForEach(x => x.Disconnect());

            netManager.Stop();
        }
    }
}