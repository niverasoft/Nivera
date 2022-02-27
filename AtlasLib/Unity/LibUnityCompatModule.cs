using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtlasLib.Utils;

namespace AtlasLib.Unity
{
    internal static class LibUnityCompatModule
    {
        internal static List<Assembly> loadedAssemblies = new List<Assembly>();

        internal static Assembly AssemblyCSharpAssembly;

        internal static void OnStart()
        {
            if (!LibProperties.UseUnityCompatModule)
                return;

            AtlasHelper.Info("Loading UnityCompatModule");

            if (AppDomain.CurrentDomain != null)
            {
                AtlasHelper.Verbose("App Domain found");
                AtlasHelper.Verbose(AppDomain.CurrentDomain.SetupInformation.ApplicationName);
                AtlasHelper.Verbose(AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName);
                AtlasHelper.Verbose("Loading assemblies");

                loadedAssemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies());

                AtlasHelper.Verbose($"Loaded {loadedAssemblies.Count} assemblies");

                AppDomain.CurrentDomain.AssemblyLoad += (x, e) =>
                {
                    AtlasHelper.Verbose($"Assembly loaded: {e.LoadedAssembly.GetName().FullName}");

                    loadedAssemblies.Add(e.LoadedAssembly);
                };

                AtlasHelper.Verbose($"Loaded {loadedAssemblies.Count} assemblies");

                AssemblyCSharpAssembly = loadedAssemblies.FirstOrDefault(x => x.GetName().FullName.Contains("Assembly-CSharp"));

                if (AssemblyCSharpAssembly != null)
                    AtlasHelper.Error("Failed to find Assembly-CSharp.dll. Is this an IL2CPP game?");
                else
                    AtlasHelper.Verbose("Assembly-CSharp.dll found");
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
