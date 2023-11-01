﻿using Cities2ModCollection.Systems;
using Game.Simulation;
using Game;
using HarmonyLib;

namespace Cities2ModCollection.Patches
{
    /// <summary>
    /// Overrides tree growth system so trees skip to adult phase ASAP.
    /// The adult phase lasts as long as child-elder would last usually.
    /// </summary>
    /// 
    [HarmonyPatch(typeof(TreeGrowthSystem), "OnCreate")]
    public class TreeGrowthSystem_OnCreatePatch
    {
        static bool Prefix(TreeGrowthSystem __instance)
        {
            __instance.World.GetOrCreateSystemManaged<TreeGrowthSystem_Custom>();
            __instance.World.GetOrCreateSystemManaged<UpdateSystem>().UpdateAt<TreeGrowthSystem_Custom>(SystemUpdatePhase.GameSimulation);
            return false; // Ignore original function
        }
    }

    [HarmonyPatch(typeof(TreeGrowthSystem), "OnUpdate")]
    public class TreeGrowthSystem_OnUpdatePatch
    {
        static bool Prefix(TreeGrowthSystem __instance)
        {
            return false; // Ignore original function
        }
    }

    [HarmonyPatch(typeof(TreeGrowthSystem), "OnCreateForCompiler")]
    public class TreeGrowthSystem_OnCreateForCompilerPatch
    {
        static bool Prefix(TreeGrowthSystem __instance)
        {
            return false; // Ignore original function
        }
    }
}
