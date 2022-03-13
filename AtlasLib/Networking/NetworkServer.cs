using System;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

using LiteNetLib;
using LiteNetLib.Utils;

using AtlasLib.Encryption;

using Newtonsoft.Json;

namespace AtlasLib.Networking
{
    public class NetworkServer
    { 
        public const int MilliTimeout = 10000;

        public event Action<ConnectionRequest> OnConnectionRequest;
        public event Action<SocketError> OnConnectionError;
        public event Action<NetPeer> OnConnectionEstablished;
        public event Action<NetPeer, DisconnectInfo> OnConnectionTerminated;
        public event Action<NetPeer, NetworkTransport> OnDataReceived;
        public event Action<NetPeer, NetworkTransport> OnDataSent;

        private static List<NetworkClient> connectedClients { get; } = new List<NetworkClient>();

        private EncryptionBitmap encryptionBitmap { get; set; }

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

            encryptionBitmap = EncryptionBitmap.DefaultBitmap();

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

                SendKey(x);
            };

            eventBasedNetListener.PeerDisconnectedEvent += (x, e) =>
            {
                OnConnectionTerminated(x, e);
            };

            eventBasedNetListener.NetworkReceiveEvent += (x, e, z) =>
            {
                byte[] buffer = new byte[e.GetInt()];

                e.GetBytes(buffer, buffer.Length);

                EncryptedObject encryptedObject = JsonConvert.DeserializeObject<EncryptedObject>(Encoding.ASCII.GetString(buffer));
                NetworkTransport networkTransport = JsonConvert.DeserializeObject<NetworkTransport>(Encryptor.Decrypt(encryptionBitmap, encryptedObject));

                OnDataReceived(x, networkTransport);
            };
        }

        public void SendKey(NetPeer netPeer)
        {
            NetDataWriter netDataWriter = new NetDataWriter();

            byte[] bitmap = EncryptionBitmap.ToBytes(encryptionBitmap);

            netDataWriter.Put(1);
            netDataWriter.Put(bitmap.Length);
            netDataWriter.Put(bitmap);

            netPeer.Send(netDataWriter.Data, DeliveryMethod.ReliableOrdered);

            OnDataSent(netPeer, null);
        }

        public void Send(NetPeer netPeer, NetworkTransport networkTransport)
        {
            byte[] data = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(Encryptor.Encrypt(encryptionBitmap, JsonConvert.SerializeObject(networkTransport))));

            NetDataWriter netDataWriter = new NetDataWriter();

            netDataWriter.Put(0);
            netDataWriter.Put(data.Length);
            netDataWriter.Put(data);

            netPeer.Send(netDataWriter.Data, DeliveryMethod.ReliableOrdered);

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
