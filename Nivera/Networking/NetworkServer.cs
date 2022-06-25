using System;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

using LiteNetLib;

using Newtonsoft.Json;

namespace Nivera.Networking
{
    public class NetworkServer
    { 
        public const int MilliTimeout = 10000;

        public event Action<ConnectionRequest> OnConnectionRequest;
        public event Action<SocketError> OnConnectionError;
        public event Action<NetPeer> OnConnectionEstablished;
        public event Action<NetPeer, DisconnectInfo> OnConnectionTerminated;
        public event Action<NetPeer, NetworkPacket> OnDataReceived;
        public event Action<NetPeer, NetworkPacket> OnDataSent;

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
                OnConnectionEstablished(x);
            };

            eventBasedNetListener.PeerDisconnectedEvent += (x, e) =>
            {
                OnConnectionTerminated(x, e);
            };

            eventBasedNetListener.NetworkReceiveEvent += (x, e, z, u) =>
            {
                byte[] buffer = new byte[e.GetInt()];

                e.GetBytes(buffer, buffer.Length);

                NetworkPacket networkTransport = JsonConvert.DeserializeObject<NetworkPacket>(Encoding.UTF8.GetString(buffer));

                OnDataReceived(x, networkTransport);
            };
        }

        public void Send(NetPeer netPeer, NetworkPacket networkTransport)
        {
            netPeer.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(networkTransport)), DeliveryMethod.ReliableOrdered);

            OnDataSent(netPeer, networkTransport);
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
