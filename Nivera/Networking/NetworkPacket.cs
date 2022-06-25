namespace Nivera.Networking
{
    public class NetworkPacket
    {
        public string Header { get; set; }
        public string SourceIp { get; set; }
        public string[] Args { get; set; }
        public string[] Content { get; set; }

        public byte Channel { get; set; }
    }
}
