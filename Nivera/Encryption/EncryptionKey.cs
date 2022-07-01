using System;
using System.Text;

using Nivera.Utils;

namespace Nivera.Encryption
{
    public static class EncryptionKey
    {
        public const int NetworkKeySize = 2048;
        public const int NormalKeySize = 8092;

        public static string GenerateNetworkKey()
            => GenerateKey(NetworkKeySize);

        public static string GenerateNormalKey()
            => GenerateKey(NormalKeySize);

        public static string GenerateKey(int size)
            => BytesToKey(RandomGen.RandomKey(size));

        public static byte[] KeyToBytes(string key)
            => Convert.FromBase64String(Encoding.UTF32.GetString(Encoding.UTF32.GetBytes(key)));

        public static string BytesToKey(byte[] key)
            => Encoding.UTF32.GetString(Encoding.UTF32.GetBytes(Convert.ToBase64String(key)));
    }
}