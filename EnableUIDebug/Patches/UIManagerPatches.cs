using Colossal.UI;
using HarmonyLib;

namespace EnableUIDebug.Patches
{
    [HarmonyPatch(typeof(UIManager.Settings), "SetDefault")]
    public class UIManager_SetDefaultPatch
    {
        public static void Postfix(UIManager.Settings __instance)
        {
            __instance.enableDebugger = true;
        }
    }
}
