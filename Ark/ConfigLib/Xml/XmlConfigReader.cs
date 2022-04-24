using System;
using System.IO;
using System.Xml.Serialization;

using ArkLib.Utils;

namespace ArkLib.ConfigLib.Xml
{
    public class XmlConfigReader : IConfigReader
    {
        public object ReadConfig(Type type, string path)
        {
            Assert.FileExists(path);

            string str = File.ReadAllText(path);

            XmlSerializer xmlSerializer = new XmlSerializer(type);

            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            streamWriter.Write(str);

            object obj = xmlSerializer.Deserialize(memoryStream);

            streamWriter.Close();
            memoryStream.Close();

            return obj;
        }
    }
}
