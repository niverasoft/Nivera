namespace AtlasLib.Encryption
{
    public class EncryptedObject
    {
        internal EncryptedObject(string str, byte[] key)
        {
            EncryptedString = str;
            EncryptedKey = key;
        }

        public string EncryptedString { get; }
        public byte[] EncryptedKey { get; }
    }
}
