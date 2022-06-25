using System.Net;

namespace Nivera.Utils
{
    public static class IpUtils
    {
        public const string IpRetriveService = "https://api.my-ip.io/ip.txt";

        public static string RetrieveCurrentIp()
        {
            WebClient webClient = new WebClient();

            return new WebClient().DownloadString(IpRetriveService);
        }
    }
}
