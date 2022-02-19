using System;
using System.IO;

using AtlasLib.Utils;

using System.Xml.Serialization;

namespace AtlasLib.ConfigLib.Xml
{
    public class XmlConfigWriter : IConfigWriter
    {
        public string WriteConfig(Type configType, object configObject = null)
        {
            Assert.NotNull(configObject);

            XmlSerializer xmlSerializer = new XmlSerializer(configType);

            return WriteAndReadFromStream(xmlSerializer, configObject);
        }

        private string WriteAndReadFromStream(XmlSerializer xmlSerializer, object obj)
        {
            MemoryStream memoryStream = new MemoryStream();

            xmlSerializer.Serialize(memoryStream, obj);

            StreamReader streamReader = new StreamReader(memoryStream);

            string str = streamReader.ReadToEnd();

            streamReader.Close();
            memoryStream.Close();

            return str;
        }
    }
}
