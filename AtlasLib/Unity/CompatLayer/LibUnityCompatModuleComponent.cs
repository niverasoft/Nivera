using UnityEngine;

namespace AtlasLib.Unity.CompatLayer
{
    internal class LibUnityCompatModuleComponent : MonoBehaviour
    {
        void Start()
        {
            AtlasLogger.Verbose($"UnityCompatModuleComponent started.");

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
