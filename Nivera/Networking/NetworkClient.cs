using LiteNetLib;

namespace Nivera.Networking
{
    public class NetworkClient
    {
        public const int MilliTimeout = 10000;

        public NetworkConnection connection { get; }
        public NetManager netManager { get; }
        public EventBasedNetListener eventBasedNetListener { get; }

        public bool isConnected { get => connection != null && connection.isConnected; }

        public NetworkClient()
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

            netManager.Start();

            connection = new NetworkConnection(netManager, eventBasedNetListener, false);
        }

        public void Stop()
        {
            netManager.DisconnectAll();
            netManager.Stop();
        }
    }
}
