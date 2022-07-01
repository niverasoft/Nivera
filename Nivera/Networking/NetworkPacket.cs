using System.Collections.Generic;

namespace Nivera.Networking
{
    public class NetworkPacket
    {
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Args { get; set; } = new Dictionary<string, string>();
        public string[] Content { get; set; } = new string[] { };
    }
}