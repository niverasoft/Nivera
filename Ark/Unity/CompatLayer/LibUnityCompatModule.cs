using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using ArKLib.Utils;

namespace ArKLib.Unity.CompatLayer
{
    internal static class LibUnityCompatModule
    {
        internal static GameObject gameObject;
        internal static LibUnityCompatModuleComponent libUnityCompatModuleComponent;

        internal static List<Assembly> loadedAssemblies = new List<Assembly>();
        internal static List<Action> updateMethods = new List<Action>();

        internal static Assembly AssemblyCSharpAssembly;

        public static bool IsActive => gameObject != null && libUnityCompatModuleComponent != null;

        internal static void EnableUnityCompatibility()
        {
            gameObject = UnityEngine.Object.FindObjectOfType<LibUnityCompatModuleComponent>()?.gameObject ?? new GameObject(Constants.ArkGameObjectName);
            libUnityCompatModuleComponent = gameObject.GetComponent<LibUnityCompatModuleComponent>() ?? gameObject.AddComponent<LibUnityCompatModuleComponent>();

            gameObject.tag = Constants.ArkGameObjectTag;
            gameObject.SetActive(true);

            UnityEngine.Object.DontDestroyOnLoad(gameObject);
        }

        internal static void RegisterUpdateMethod(Action action)
        {
            updateMethods.Add(action);
        }

        internal static void OnStart()
        {
            if (!LibProperties.System_UseUnityCompatModule)
                return;

            ArkLog.Info("Loading UnityCompatModule");

            if (AppDomain.CurrentDomain != null)
            {
                ArkLog.Verbose("App Domain found");
                ArkLog.Verbose(AppDomain.CurrentDomain.SetupInformation.ApplicationName);
                ArkLog.Verbose(AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName);
                ArkLog.Verbose("Loading assemblies");

                loadedAssemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies());

                ArkLog.Verbose($"Loaded {loadedAssemblies.Count} assemblies");

                AppDomain.CurrentDomain.AssemblyLoad += (x, e) =>
                {
                    ArkLog.Verbose($"Assembly loaded: {e.LoadedAssembly.GetName().FullName}");

                    loadedAssemblies.Add(e.LoadedAssembly);
                };

                ArkLog.Verbose($"Loaded {loadedAssemblies.Count} assemblies");

                AssemblyCSharpAssembly = loadedAssemblies.FirstOrDefault(x => x.GetName().FullName.Contains("Assembly-CSharp"));

                if (AssemblyCSharpAssembly != null)
                    ArkLog.Error("Failed to find Assembly-CSharp.dll. Is this an IL2CPP game?");
                else
                    ArkLog.Verbose("Assembly-CSharp.dll found");
            }
        }

        internal static void OnUpdate()
        {
            foreach (var method in updateMethods)
            {
                method();
            }
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
