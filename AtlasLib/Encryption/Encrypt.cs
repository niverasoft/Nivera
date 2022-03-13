namespace AtlasLib.Encryption
{
    public static class Encryptor
    {
        public static EncryptedObject Encrypt(EncryptionBitmap encryptionBitmap, string str)
        {
            return encryptionBitmap.EncryptWith(null, str);
        }

        public static string Decrypt(EncryptionBitmap encryptionBitmap, EncryptedObject encryptedObject)
        {
            return encryptionBitmap.DecrypthWith(encryptedObject);
        }
    }
}