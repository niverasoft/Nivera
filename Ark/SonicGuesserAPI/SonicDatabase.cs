using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArKLib.IO;
using ArKLib.Utils;

using UnityEngine;
using Newtonsoft.Json;

namespace ArKLib.SonicGuesserAPI
{
    public static class SonicDatabase
    {
        private static WebClient webClient = new WebClient();

        public static bool IsDownloaded { get; set; }
        public static string DatabasePath => $"{Application.dataPath}/Songs";

        public static Dictionary<SonicDifficulty, Dictionary<string, string>> Songs { get; set; }

        public static void DownloadDatabase()
        {
            if (!File.Exists($"{Application.dataPath}/database"))
                webClient.DownloadFile("", $"{Application.dataPath}/database");

            webClient.Dispose();
            webClient = null;
            webClient = new WebClient();

            string str = webClient.DownloadString(Encoding.UTF8.GetString(File.ReadAllBytes($"{Application.dataPath}/database")));

            byte[] database = Encoding.UTF8.GetBytes(str);
            byte[] raw = DataFile.Decompress(database);

            Songs = JsonConvert.DeserializeObject<Dictionary<SonicDifficulty, Dictionary<string, string>>>(Encoding.UTF8.GetString(raw));

            webClient.Dispose();
            webClient = null;
        }

        public static SonicTrack DownloadSong(string url)
        {
            webClient = new WebClient();

            string str = webClient.DownloadString(url);
            byte[] songRaw = Encoding.UTF8.GetBytes(str);
            byte[] decomp = DataFile.Decompress(songRaw);

            webClient.Dispose();
            webClient = null;

            return JsonConvert.DeserializeObject<SonicTrack>(Encoding.UTF8.GetString(decomp));
        }

        public static void SaveSong(SonicTrack sonicTrack)
        {
            DataFile.WriteTo($"{DatabasePath}/{sonicTrack.SongID}.ark", sonicTrack);
        }

        public static SonicTrack LoadSong(KeyValuePair<string, string> pair)
        {
            if (File.Exists($"{DatabasePath}/{pair.Key}.ark"))
            {
                return DataFile.ReadFrom<SonicTrack>($"{DatabasePath}/{pair.Key}.ark");
            }
            else
            {
                return DownloadSong(pair.Value);
            }
        }

        public static SonicTrack GetRandomSong(SonicDifficulty sonicDifficulty)
        {
            var songList = Songs[sonicDifficulty];

            foreach (var track in SonicSave.LoadedSave.CompletedTracks)
            {
                songList.Remove(track);
            }

            return LoadSong(songList.ElementAt(RandomGen.RandomInt(0, songList.Count)));
        }
    }
}