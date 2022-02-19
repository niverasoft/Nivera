using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using AtlasLib.Utils;

namespace AtlasLib.Harmony
{
    public class PatchManager
    {
        private HarmonyLib.Harmony _harmonyInstance;

        public PatchManager()
        {
            _harmonyInstance = new HarmonyLib.Harmony(RandomGen.NextString());
        }

        public void PatchAll(Assembly assembly)
        {
            _harmonyInstance.PatchAll(assembly);
        }

        public void PatchAll()
        {
            _harmonyInstance.PatchAll(Assembly.GetCallingAssembly());
        }

        public List<MethodBase> GetPatchedMethods()
        {
            return _harmonyInstance.GetPatchedMethods().ToList();
        }
    }
}