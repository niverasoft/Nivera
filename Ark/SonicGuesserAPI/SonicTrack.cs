using System;
using System.Collections.Generic;

namespace ArKLib.SonicGuesserAPI
{
    public class SonicTrack
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
        public string IconURL { get; set; }
        public string SongID { get; set; }
        public byte[] SongData { get; set; }

        public Dictionary<SonicDifficulty, List<Tuple<TimeSpan, TimeSpan>>> TimeStamps { get; set; }
    }
}
