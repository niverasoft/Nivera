using System;
using System.Linq;
using System.Collections.Generic;

using AtlasLib.Utils;

namespace AtlasLib.Encryption
{
    public class EncryptionBitmap
    {
        private const int DefaultBitmapLength = 1024;

        private byte[] _bitmap;
        private int _setSize;

        private EncryptionBitmap(byte[] bitmap)
        {
            _setSize = bitmap.Length;
            _bitmap = new byte[bitmap.Length];

            Array.Copy(bitmap, _bitmap, bitmap.Length);

            ValidateBitmap();
        }

        private EncryptionBitmap(int size) : this(RandomGen.RandomBytes(size, size)) { }

        public static EncryptionBitmap DefaultBitmap()
            => new EncryptionBitmap(DefaultBitmapLength);

        public static EncryptionBitmap SizeOf(int size)
            => new EncryptionBitmap(size);

        public static EncryptionBitmap FromBytes(byte[] bitmap)
            => new EncryptionBitmap(bitmap);

        public static byte[] ToBytes(EncryptionBitmap encryptionBitmap)
            => encryptionBitmap._bitmap;

        public static void Resize(EncryptionBitmap encryptionBitmap, int newSize)
        {
            encryptionBitmap._setSize = newSize;
            encryptionBitmap.ValidateBitmap();
        }

        internal EncryptedObject EncryptWith(byte[] encryptKey, string strToEncrypt)
        {
            encryptKey = encryptKey ?? new byte[strToEncrypt.Length];

            if (encryptKey.Length < strToEncrypt.Length)
                ThrowHelper.LogAndThrow<ArgumentOutOfRangeException>();

            string hash = "";

            byte curIndex = 0;
            int curBitmap = 0;

            int[] reqIndexes = GetRequiredIndexes(strToEncrypt.Length);

            List<int> finishedIndexes = new List<int>();

            while (!AreIndexesFinished(finishedIndexes, reqIndexes))
            {
                int indexInArray = _bitmap[curBitmap];

                byte ranIndex = RandomGen.RandomByte(0, (byte)strToEncrypt.Length);

                if (finishedIndexes.Contains(ranIndex))
                    continue;

                encryptKey[indexInArray] = ranIndex;

                hash += strToEncrypt[ranIndex];

                curIndex++;
                curBitmap++;

                finishedIndexes.Add(curIndex);
            }

            return new EncryptedObject(hash, encryptKey);
        }

        internal string DecrypthWith(EncryptedObject encryptedObject)
        {
            char[] origCharArray = new char[encryptedObject.EncryptedKey.Length];

            for (int i = 0; i < encryptedObject.EncryptedKey.Length; i++)
            {
                int indexInArray = _bitmap[i];

                origCharArray[encryptedObject.EncryptedKey[indexInArray]] = encryptedObject.EncryptedString[encryptedObject.EncryptedKey[indexInArray]];
            }

            return new string(origCharArray);
        }

        internal int[] GetRequiredIndexes(int strLen)
        {
            List<int> indexes = new List<int>();

            for (int i = 0; i < strLen; i++)
            {
                indexes.Add(i);
            }

            return indexes.ToArray();
        }

        internal bool AreIndexesFinished(List<int> indexList, int[] reqIndexes)
        {
            if (indexList.Count < reqIndexes.Length)
                return false;

            foreach (int index in reqIndexes)
            {
                if (indexList.Contains(index))
                    continue;
                else
                    return false;
            }

            return true;
        }

        internal void ValidateBitmap()
        {
            bool validationSuccess = true;

            for (int i = 0; i < _setSize; i++)
            {
                byte b = _bitmap[i];

                if (_bitmap.Count(x => x == b) > 1)
                {
                    validationSuccess = false;

                    break;
                }
            }

            if (!validationSuccess)
            {
                _bitmap = RandomGen.RandomBytes(_setSize, _setSize);

                ValidateBitmap();
            }
        }
    }
}