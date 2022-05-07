using UnityEngine;

namespace ArKLib.Unity.CompatLayer
{
    internal class LibUnityCompatModuleComponent : MonoBehaviour
    {
        void Start()
        {
            ArkLog.Verbose($"UnityCompatModuleComponent started.");

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
