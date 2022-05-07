using System.IO;
using System.Collections.Generic;

using UnityEngine;

using ArKLib.IO;

namespace ArKLib.SonicGuesserAPI
{
    public class SonicSave
    {
        public static string SavePath => $"{Application.dataPath}/sonicSave.ark";

        public static SonicSave LoadedSave { get; private set; }

        public List<string> CompletedTracks { get; set; } = new List<string>();

        public int Level { get; set; }
        public long Points { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }
        public bool IsNew { get; set; }

        public static void Save()
        {
            DataFile.WriteTo(SavePath, LoadedSave);
        }

        public static void Load()
        {
            if (!File.Exists(SavePath))
            {
                LoadedSave = new SonicSave();
                LoadedSave.IsNew = true;

                DataFile.WriteTo(SavePath, LoadedSave);
            }
            else
            {
                LoadedSave = DataFile.ReadFrom<SonicSave>(SavePath);
            }
        }
    }
}