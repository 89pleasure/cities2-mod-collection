using Game.UI.Menu;
using HarmonyLib;

namespace EditorEnabled.Patches
{
    [HarmonyPatch(typeof(MenuUISystem), "IsEditorEnabled")]
    class MenuUISystem_IsEditorEnabledPatch
    {
        static bool Prefix(ref bool __result)
        {
            __result = true;

            return false; // Ignore original function
        }
    }
}
