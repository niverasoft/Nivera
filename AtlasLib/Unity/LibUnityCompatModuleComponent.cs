using UnityEngine;

using AtlasLib.Utils;

namespace AtlasLib.Unity
{
    internal class LibUnityCompatModuleComponent : MonoBehaviour
    {
        void Start()
        {
            AtlasHelper.Verbose($"UnityCompatModuleComponent started.");

            LibUnityCompatModule.OnStart();
        }

        void Update()
        {
            LibUnityCompatModule.OnUpdate();
        }

        void FixedUpdate()
        {
            LibUnityCompatModule.OnFixedUpdate();
        }

        void LateUpdate()
        {
            LibUnityCompatModule.OnLateUpdate();
        }

        void OnDestroy()
        {
            LibUnityCompatModule.OnDestroy();
        }
    }
}
