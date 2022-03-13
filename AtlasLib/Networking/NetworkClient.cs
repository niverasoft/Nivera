using System;
using System.Text;
using System.Net.Sockets;

using LiteNetLib;
using LiteNetLib.Utils;

using AtlasLib.Utils;
using AtlasLib.Encryption;

using Newtonsoft.Json;

namespace AtlasLib.Networking
{
    public class NetworkClient
    {
        public const int MilliTimeout = 10000;

        public event Action<SocketError> OnConnectionError;
        public event Action<NetPeer> OnConnectionEstablished;
        public event Action<DisconnectInfo> OnConnectionTerminated;
        public event Action<NetworkTransport> OnDataReceived;
        public event Action<NetworkTransport> OnDataSent;
        public event Action<EncryptionBitmap> OnKeyReceived;

        private EncryptionBitmap encryptionBitmap { get; set; }

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

            eventBasedNetListener.NetworkReceiveEvent += (x, e, z) =>
            {
                Assert.Equals(z, DeliveryMethod.ReliableOrdered, "Received an unreliable package.");

                byte[] buffer = null;

                if (e.GetInt() == 1)
                {
                    buffer = new byte[e.GetInt()];

                    e.GetBytes(buffer, buffer.Length);

                    encryptionBitmap = EncryptionBitmap.FromBytes(buffer);

                    OnKeyReceived(encryptionBitmap);
                }
                else
                {
                    buffer = new byte[e.GetInt()];

                    e.GetBytes(buffer, buffer.Length);

                    EncryptedObject encryptedObject = JsonConvert.DeserializeObject<EncryptedObject>(Encoding.ASCII.GetString(buffer));
                    NetworkTransport networkTransport = JsonConvert.DeserializeObject<NetworkTransport>(Encryptor.Decrypt(encryptionBitmap, encryptedObject));

                    OnDataReceived(networkTransport);
                }
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
                OnConnectionTerminated(e);
            };
        }

        public void Send(NetworkTransport networkTransport)
        {
            byte[] data = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(Encryptor.Encrypt(encryptionBitmap, JsonConvert.SerializeObject(networkTransport))));

            NetDataWriter netDataWriter = new NetDataWriter();

            netDataWriter.Put(data.Length);
            netDataWriter.Put(data);

            netPeer.Send(netDataWriter.Data, DeliveryMethod.ReliableOrdered);

            OnDataSent(networkTransport);
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
