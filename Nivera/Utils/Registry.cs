using System.Diagnostics;
using System.Text;
using System;

using Newtonsoft.Json;

namespace Nivera.Utils
{
    public static class Registry
    {
        public static RegistryKey OpenKey(string name)
        {
            string appname = GetCallingRegistryKey();

            var rg = Microsoft.Win32.Registry.CurrentUser;

            var key = rg.OpenSubKey(appname);

            return new RegistryKey(key, name);
        }

        private static string GetCallingRegistryKey()
        {
            foreach (var stackTrace in new StackTrace().GetFrames())
            {
                if (stackTrace.GetMethod().DeclaringType == typeof(Registry))
                    continue;

                return $"{stackTrace.GetMethod().DeclaringType.Assembly}RG{LibProperties.LibraryVersion}";
            }

            return $"NiveraRGDEF{LibProperties.LibraryVersion}";
        }
    }

    public class RegistryKey
    {
        private Microsoft.Win32.RegistryKey key;

        internal RegistryKey(Microsoft.Win32.RegistryKey key, string name)
        {
            this.key = key;

            Name = name;
        }

        public string Name { get; set; }

        public T GetValue<T>()
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF32.GetString(Convert.FromBase64String(Encoding.UTF32.GetString(key.GetValue(Name) as byte[]))));
        }

        public void SetValue(object value)
        {
            key.SetValue(Name, Convert.ToBase64String(Encoding.UTF32.GetBytes(JsonConvert.SerializeObject(value))));
        }
    }
}