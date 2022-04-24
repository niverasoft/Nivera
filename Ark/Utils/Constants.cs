using System;

using ArkLib.Encryption;

namespace ArkLib.Utils
{
    public static class Constants
    {
        public static readonly Type VoidType = typeof(void);
        public static readonly Type ObjectType = typeof(object);
        public static readonly Type BoolType = typeof(bool);

        public const string ArkGameObjectName = "Ark_ArkLib_UnityCompatModuleGameObject";
        public const string ArkGameObjectTag = "Ark_ArkLib_ControlObject";

        public static readonly EncryptionBitmap LibraryBitmap = EncryptionBitmap.SizeOf(4096);
    }
}
