using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using AtlasLib.Utils;

namespace AtlasLib.Unity.CompatLayer
{
    internal static class LibUnityCompatModule
    {
        internal static GameObject gameObject;
        internal static LibUnityCompatModuleComponent libUnityCompatModuleComponent;

        internal static List<Assembly> loadedAssemblies = new List<Assembly>();

        internal static Assembly AssemblyCSharpAssembly;

        internal static void EnableUnityCompatibility()
        {
            gameObject = UnityEngine.Object.FindObjectOfType<LibUnityCompatModuleComponent>()?.gameObject ?? new GameObject(Constants.AtlasGameObjectName);
            libUnityCompatModuleComponent = gameObject.GetComponent<LibUnityCompatModuleComponent>() ?? gameObject.AddComponent<LibUnityCompatModuleComponent>();

            gameObject.tag = Constants.AtlasGameObjectTag;
            gameObject.SetActive(true);

            UnityEngine.Object.DontDestroyOnLoad(gameObject);
        }

        internal static void OnStart()
        {
            if (!LibProperties.System_UseUnityCompatModule)
                return;

            AtlasLogger.Info("Loading UnityCompatModule");

            if (AppDomain.CurrentDomain != null)
            {
                AtlasLogger.Verbose("App Domain found");
                AtlasLogger.Verbose(AppDomain.CurrentDomain.SetupInformation.ApplicationName);
                AtlasLogger.Verbose(AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName);
                AtlasLogger.Verbose("Loading assemblies");

                loadedAssemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies());

                AtlasLogger.Verbose($"Loaded {loadedAssemblies.Count} assemblies");

                AppDomain.CurrentDomain.AssemblyLoad += (x, e) =>
                {
                    AtlasLogger.Verbose($"Assembly loaded: {e.LoadedAssembly.GetName().FullName}");

                    loadedAssemblies.Add(e.LoadedAssembly);
                };

                AtlasLogger.Verbose($"Loaded {loadedAssemblies.Count} assemblies");

                AssemblyCSharpAssembly = loadedAssemblies.FirstOrDefault(x => x.GetName().FullName.Contains("Assembly-CSharp"));

                if (AssemblyCSharpAssembly != null)
                    AtlasLogger.Error("Failed to find Assembly-CSharp.dll. Is this an IL2CPP game?");
                else
                    AtlasLogger.Verbose("Assembly-CSharp.dll found");
            }
        }

        internal static void OnUpdate()
        {

        }

        internal static void OnFixedUpdate()
        {

        }

        internal static void OnLateUpdate()
        {

        }

        internal static void OnDestroy()
        {

        }
    }
}
