using System;
using System.Text;
using System.Net.Sockets;

using LiteNetLib;

using Newtonsoft.Json;

using Nivera.Utils;
using Nivera.Encryption;

namespace Nivera.Networking
{
    public class NetworkConnection
    {
        public const int SessionKeyMaxLen = 512;

        public event Action<SocketError> OnConnectionError;
        public event Action<NetPeer> OnConnectionEstablished;
        public event Action<DisconnectInfo> OnConnectionTerminated;
        public event Action<NetworkPacket> OnDataReceived;
        public event Action<NetworkPacket> OnDataSent;
        public event Action<string> OnKeyReceived;
        public event Action<string> OnKeySent;
        public event Action<string> OnKeyGenerated;

        public bool isConnected { get => netPeer != null && netPeer.ConnectionState == ConnectionState.Connected; }

        public NetPeer netPeer { get; private set; }
        public NetManager netManager { get; private set; }
        public string sessionKey { get; private set; }
        public string myIp { get; }
        public int latency { get; private set; }
        public bool serverMode { get; private set; }

        public NetworkConnection(NetManager netManager, EventBasedNetListener eventBasedNetListener, bool isServer, string key = null, NetPeer netPeer = null)
        {
            myIp = IpUtils.RetrieveCurrentIp();

            this.netPeer = netPeer;
            this.netManager = netManager;
            this.serverMode = isServer;

            if (serverMode)
            {
                if (string.IsNullOrEmpty(key))
                    sessionKey = EncryptionKey.GenerateKey(SessionKeyMaxLen);
                else
                    sessionKey = key;

                OnKeyGenerated(key);
            }

            eventBasedNetListener.NetworkReceiveEvent += (x, e, z, u) =>
            {
                if (netPeer == null)
                    return;

                if (x.Id != netPeer.Id)
                    return;

                Assert.Equals(u, DeliveryMethod.ReliableOrdered);

                if (e.GetInt() == 0)
                {
                    byte[] keyBuffer = new byte[e.GetInt()];

                    e.GetBytes(keyBuffer, keyBuffer.Length);

                    sessionKey = Encoding.UTF32.GetString(keyBuffer);

                    OnKeyReceived(sessionKey);
                }
                else
                {
                    byte[] buffer = new byte[e.GetInt()];

                    e.GetBytes(buffer, buffer.Length);

                    string encrypted = Encoding.UTF32.GetString(buffer);

                    NetworkPacket networkPacket = JsonConvert.DeserializeObject<NetworkPacket>(Encryption.Encryption.Decrypt(sessionKey, encrypted));

                    OnDataReceived(networkPacket);
                }
            };

            eventBasedNetListener.NetworkLatencyUpdateEvent += (x, e) =>
            {
                if (netPeer == null)
                    return;

                Assert.Equals(x.Id, netPeer.Id);

                latency = e;
            };

            eventBasedNetListener.PeerConnectedEvent += (x) =>
            {
                if (netPeer != null)
                    return;

                if (serverMode)
                {
                    Send(new NetworkPacket
                    {
                        Headers = new System.Collections.Generic.Dictionary<string, string>
                        {
                            ["Sender"] = "SERVER",
                            ["AuthKey"] = sessionKey,
                            ["RequestType"] = "None",
                            ["SenderOperation"] = "SendKey"
                        }
                    });
                }

                netPeer = x;

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
            if (serverMode && networkTransport.Headers["SenderOperation"] == "SendKey")
            {
                LiteNetLib.Utils.NetDataWriter netDataWriter = new LiteNetLib.Utils.NetDataWriter(true);

                byte[] keyBuffer = Utility.CompressByteArray(Encoding.UTF32.GetBytes(sessionKey));

                netDataWriter.Put(0);
                netDataWriter.Put(keyBuffer.Length);
                netDataWriter.Put(keyBuffer);

                OnKeySent(sessionKey);
            }
            else
            {
                if (string.IsNullOrEmpty(sessionKey))
                {
                    ThrowHelper.LogAndThrow("You cannot send any packets without a session key.");

                    return;
                }

                LiteNetLib.Utils.NetDataWriter netDataWriter = new LiteNetLib.Utils.NetDataWriter(true);

                byte[] data = Utility.CompressByteArray(Encoding.UTF32.GetBytes(Encryption.Encryption.EncryptWithKey(sessionKey, JsonConvert.SerializeObject(networkTransport))));

                netDataWriter.Put(1);
                netDataWriter.Put(data.Length);
                netDataWriter.Put(data);

                OnDataSent(networkTransport);
            }
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