using System;
using System.Text;
using System.Net.Sockets;

using LiteNetLib;

using Newtonsoft.Json;

using Nivera.Utils;
using Nivera.IO;

namespace Nivera.Networking
{
    public class NetworkConnection
    {
        public event Action<SocketError> OnConnectionError;
        public event Action<NetPeer> OnConnectionEstablished;
        public event Action<DisconnectInfo> OnConnectionTerminated;
        public event Action<NetworkPacket> OnDataReceived;
        public event Action<NetworkPacket> OnDataSent;

        public bool isConnected { get => netPeer != null && netPeer.ConnectionState == ConnectionState.Connected; }

        public NetPeer netPeer { get; private set; }
        public NetManager netManager { get; private set; }
        public string myIp { get; }
        public int latency { get; private set; }

        internal NetworkConnection(NetManager netManager, EventBasedNetListener eventBasedNetListener)
        {
            myIp = IpUtils.RetrieveCurrentIp();
            this.netManager = netManager;

            eventBasedNetListener.NetworkReceiveEvent += (x, e, z, u) =>
            {
                if (x.Id != netPeer.Id)
                    return;

                Assert.Equals(u, DeliveryMethod.ReliableOrdered);

                byte[] buffer = new byte[e.AvailableBytes];

                e.GetBytes(buffer, buffer.Length);

                NetworkPacket networkTransport = JsonConvert.DeserializeObject<NetworkPacket>(Encoding.UTF32.GetString(Utility.DecompressByteArray(buffer)));

                Assert.NotNull(networkTransport);

                OnDataReceived(networkTransport);
            };

            eventBasedNetListener.NetworkLatencyUpdateEvent += (x, e) =>
            {
                Assert.Equals(x.Id, netPeer.Id);

                latency = e;
            };

            eventBasedNetListener.PeerConnectedEvent += (x) =>
            {
                if (netPeer != null)
                    return;

                OnConnectionEstablished(x);
            };

            eventBasedNetListener.PeerDisconnectedEvent += (x, e) =>
            {
                if (x.Id != netPeer.Id)
                    return;

                OnConnectionTerminated(e);

                netPeer = null;
            };

            eventBasedNetListener.NetworkErrorEvent += (x, e) =>
            {
                OnConnectionError(e);
            };
        }

        public void Send(NetworkPacket networkTransport)
        {
            Assert.True(isConnected);

            byte[] data = Utility.CompressByteArray(Encoding.UTF32.GetBytes(JsonConvert.SerializeObject(networkTransport)));

            netPeer.Send(data, networkTransport.Channel, DeliveryMethod.ReliableOrdered);

            OnDataSent(networkTransport);
        }

        public void Connect(string ip, int port)
        {
            netPeer = netManager.Connect(ip, port, RandomGen.RandomBytesString());
        }

        public void Disconnect()
        {
            netManager.DisconnectPeerForce(netPeer);
        }
    }
}