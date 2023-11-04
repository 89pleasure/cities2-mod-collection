using Game.UI.Localization;
using Game.UI.Tooltip;
using HarmonyLib;

namespace Cities1Unit.Patches
{
    [HarmonyPatch(typeof(NetCourseTooltipSystem), "OnUpdate")]
    class NetworkPatchesNetCourseTooltipSystem_OnUpdatePatch
    {
        static void Postfix(NetCourseTooltipSystem __instance)
        {
            var m_Length = Traverse.Create(__instance).Field("m_Length").GetValue<FloatTooltip>();

            if (m_Length != null)
            {
                m_Length.value /= 8f; // Convert to Cities 1 units
                m_Length.unit = "floatTwoFractions"; // Change to a generic unit type to stop showing m/ft
                m_Length.label = LocalizedString.Value("u"); // Adjust the label to say 'u'
            }
        }
    }
}
