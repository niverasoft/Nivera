using System;
using System.Collections.Generic;
using System.Text;

using AtlasLib.Utils;

namespace AtlasLib.Encryption
{
    public static class Encrypt
    {
        public static EncryptedObject DoEncrypt(string str)
        {
            byte[] key = new byte[str.Length];

            string hash = Scramble(key, str);

            return new EncryptedObject(hash, key);
        }

        public static string CombineStringWithKey(EncryptedObject encryptedObject)
        {
            string keyStr = Encoding.ASCII.GetString(encryptedObject.EncryptedKey);

            return $"{encryptedObject.EncryptedString}[[--]]{keyStr}";
        }

        public static EncryptedObject GetFromCombinedKey(string combinedKey)
        {
            string[] parts = combinedKey.Split(new string[] { "[[--]]" }, StringSplitOptions.RemoveEmptyEntries);

            byte[] key = Encoding.ASCII.GetBytes(parts[1]);
            string hash = parts[0];

            return new EncryptedObject(hash, key);
        }

        public static string DoUnencrypt(EncryptedObject encryptedObject)
        {
            return UnScramble(encryptedObject.EncryptedKey, encryptedObject.EncryptedString);
        }

        private static string Scramble(byte[] inKey, string inp)
        {
            int index = 0;

            List<int> doneIndexes = new List<int>();

            string hash = "";

            while (doneIndexes.Count != inp.Length)
            {
                int randomIndex = RandomGen.RandomInt(0, inp.Length);

                if (doneIndexes.Contains(randomIndex))
                    continue;

                inKey[index] = (byte)randomIndex;

                hash += inp[randomIndex];

                doneIndexes.Add(randomIndex);

                index++;
            }

            return hash;
        }

        private static string UnScramble(byte[] key, string hash)
        {
            string str = "";

            for (int i = 0; i < key.Length; i++)
            {
                int posInString = key[i];

                str += hash[posInString];
            }

            return str;
        }
    }
}