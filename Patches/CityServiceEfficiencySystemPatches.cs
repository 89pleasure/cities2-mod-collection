using Cities2ModCollection.Systems;
using Game;
using Game.Buildings;
using HarmonyLib;

namespace Cities2ModCollection.Patches
{
    [HarmonyPatch(typeof(CityServiceEfficiencySystem), "OnCreate")]
    class CityServiceEfficiencySystem_OnCreatePatch
    {
        static bool Prefix(CityServiceEfficiencySystem __instance)
        {
            __instance.World.GetOrCreateSystemManaged<CityServiceEfficiencySystem_Custom>();
            __instance.World.GetOrCreateSystemManaged<UpdateSystem>()
                .UpdateAt<CityServiceEfficiencySystem_Custom>(SystemUpdatePhase.ModificationEnd);

            return false;
        }
    }

    [HarmonyPatch(typeof(CityServiceEfficiencySystem), "OnUpdate")]
    public class CityServiceEfficiencySystem_OnUpdatePatch
    {
        static bool Prefix(CityServiceEfficiencySystem __instance)
        {
            return false; // Ignore original function
        }
    }

    [HarmonyPatch(typeof(CityServiceEfficiencySystem), "OnCreateForCompiler")]
    public class CityServiceEfficiencySystem_OnCreateForCompilerPatch
    {
        static bool Prefix(CityServiceEfficiencySystem __instance)
        {
            return false; // Ignore original function
        }
    }

}
