namespace ArKLib.Networking
{
    public class NetworkTransport
    {
        public string Header { get; set; }
        public string[] Args { get; set; }
        public string[] Content { get; set; }

        public byte Channel { get; set; }

        public NetworkConnectionInfo ConnectionInfo { get; set; }
    }
}
