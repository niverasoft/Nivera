using System;
using LiteNetLib;
using Utf8Json;
using ArKLib.Utils;

namespace ArKLib.Networking
{
    public class NetworkConnection
    {
        public event Action<NetworkTransport> OnReceived;

        public bool isConnected { get => netPeer != null && netPeer.ConnectionState == ConnectionState.Connected; }

        public NetPeer netPeer { get; }
        public NetworkConnectionInfo networkConnectionInfo { get; }

        public int latency { get; private set; }

        internal NetworkConnection(string client, EventBasedNetListener eventBasedNetListener, NetPeer netPeer)
        {
            this.netPeer = netPeer;
            this.networkConnectionInfo = new NetworkConnectionInfo
            {
                Ip = netPeer.EndPoint.Address.ToString(),
                Port = netPeer.EndPoint.Port,
                Client = client
            };

            eventBasedNetListener.NetworkReceiveEvent += (x, e, z) =>
            {
                Assert.Equals(z, DeliveryMethod.ReliableOrdered);
                Assert.Equals(x.Id, netPeer.Id);

                byte[] buffer = new byte[e.AvailableBytes];

                e.GetBytes(buffer, buffer.Length);

                NetworkTransport networkTransport = JsonSerializer.Deserialize<NetworkTransport>(buffer);

                Assert.NotNull(networkTransport);

                OnReceived(networkTransport);
            };

            eventBasedNetListener.NetworkLatencyUpdateEvent += (x, e) =>
            {
                Assert.Equals(x.Id, netPeer.Id);

                latency = e;
            };
        }

        public void Send(NetworkTransport networkTransport)
        {
            Assert.True(isConnected);

            networkTransport.ConnectionInfo = networkConnectionInfo;

            byte[] data = JsonSerializer.Serialize(networkTransport);

            netPeer.Send(data, networkTransport.Channel, DeliveryMethod.ReliableOrdered);
        }
    }
}
