using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivera.Utils
{
    public static class Utility
    {
        public static byte[] CompressByteArray(byte[] input)
        {
            MemoryStream strm = new MemoryStream();
            GZipStream GZipStrem = new GZipStream(strm, CompressionMode.Compress, true);

            GZipStrem.Write(input, 0, input.Length);
            GZipStrem.Flush();
            strm.Flush();

            byte[] ByteArrayToreturn = strm.GetBuffer();

            GZipStrem.Close();
            strm.Close();

            return ByteArrayToreturn;
        }

        public static byte[] DecompressByteArray(byte[] input)
        {
            MemoryStream strm = new MemoryStream(input);

            GZipStream GZipStrem = new GZipStream(strm, CompressionMode.Decompress, true);
            List<byte> ByteListUncompressedData = new List<byte>();

            int bytesRead = GZipStrem.ReadByte();

            while (bytesRead != -1)
            {
                ByteListUncompressedData.Add((byte)bytesRead);
                bytesRead = GZipStrem.ReadByte();
            }

            GZipStrem.Flush();
            strm.Flush();

            GZipStrem.Close();
            strm.Close();

            return ByteListUncompressedData.ToArray();
        }

        public static long GetByteArraySize(byte[] array)
        {
            long total = 0;

            foreach (byte b in array)
            {
                total += b;
            }

            return total;
        }
    }
}
