using Game.SceneFlow;
using HarmonyLib;

namespace Cities2ModCollection.Patches
{
    [HarmonyPatch(typeof(GameManager), "ParseOptions")]
    class GameManager_ParseOptionsPatch
    {
        static void Postfix(GameManager __instance)
        {
            GameManager.Configuration configuration = __instance.configuration;

            if (configuration != null)
            {
                configuration.developerMode = true;

                UnityEngine.Debug.Log("Turned on Developer Mode! Press TAB for the dev/debug menu.");
            }
        }
    }
}
