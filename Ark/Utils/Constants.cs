using System;

using ArKLib.Encryption;

namespace ArKLib.Utils
{
    public static class Constants
    {
        public static readonly Type VoidType = typeof(void);
        public static readonly Type ObjectType = typeof(object);
        public static readonly Type BoolType = typeof(bool);

        public const string ArkGameObjectName = "Ark_ArKLib_UnityCompatModuleGameObject";
        public const string ArkGameObjectTag = "Ark_ArKLib_ControlObject";

        public static readonly EncryptionBitmap LibraryBitmap = EncryptionBitmap.SizeOf(4096);
    }
}
