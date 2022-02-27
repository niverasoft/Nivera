using AtlasLib.Utils;
using AtlasLib.Unity;
using AtlasLib.Reflection;

using System;

using UnityEngine;

namespace AtlasLib
{
    public static class LibProperties
    {
        internal static LibUnityCompatModuleComponent libUnityCompatModuleComponent;
        internal static GameObject gameObject;

        internal const string AtlasGameObjectName = "Atlas_AtlasLib_UnityCompatModuleGameObject";
        internal const string AtlasGameObjectTag = "Atlas_AtlasLib_ControlObject";

        public static NLog.ILogger Logger;

        public static bool AddStackTraceToThrowHelper;
        public static bool HandleUnhandledExceptions;
        public static bool EnableDebugLog;
        public static bool EnableVerboseLog;

        public static bool UseUnityCompatModule;

        public static void Load()
        {
            if (HandleUnhandledExceptions)
            {
                if (AppDomain.CurrentDomain != null)
                {
                    AppDomain.CurrentDomain.UnhandledException += (x, y) =>
                    {
                        if (y.ExceptionObject != null)
                        {
                            AtlasHelper.Fatal(y.ExceptionObject.ConvertTo<Exception>());
                        }
                    };
                }
            }
        }

        public static void EnableUnityCompatibility(GameObject gameObject = null)
        {
            if (gameObject == null)
                gameObject = UnityEngine.Object.FindObjectOfType<LibUnityCompatModuleComponent>()?.gameObject ?? new GameObject(AtlasGameObjectName);

            LibProperties.gameObject = gameObject;
            LibProperties.libUnityCompatModuleComponent = gameObject.GetComponent<LibUnityCompatModuleComponent>() ?? gameObject.AddComponent<LibUnityCompatModuleComponent>();

            gameObject.tag = AtlasGameObjectTag;
            gameObject.SetActive(true);

            UnityEngine.Object.DontDestroyOnLoad(gameObject);
        }
    }
}