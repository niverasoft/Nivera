using System.Collections.Generic;

namespace Nivera.Encryption
{
    public static class Encryption
    {
        public static (string, string) EncryptNetwork(string input)
        {
            string key = EncryptionKey.GenerateNetworkKey();

            return (key, EncryptWithKey(key, input));
        }

        public static (string, string) Encrypt(string input)
        {
            string key = EncryptionKey.GenerateNormalKey();

            return (key, EncryptWithKey(key, input));
        }

        public static string EncryptWithKey(string key, string input)
        {
            byte[] keys = EncryptionKey.KeyToBytes(key);

            string encrypted = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (keys[i] > input.Length)
                {
                    if (i == 0)
                        continue;
                    else
                    {
                        i--;
                        continue;
                    }
                }

                encrypted += input[keys[i]];
            }

            return encrypted;
        }

        public static string Decrypt(string key, string encrypted)
        {
            byte[] keys = EncryptionKey.KeyToBytes(key);

            List<char> str = new List<char>(encrypted.Length);

            for (int i = 0; i < encrypted.Length; i++)
            {
                str.Add(' ');
            }

            for (int i = 0; i < encrypted.Length; i++)
            {
                str[keys[i]] = encrypted[i];
            }

            return new string(str.ToArray());
        }
    }
}
