
using Cities1Controls.System;
using Game;
using Game.Audio;
using HarmonyLib;

namespace Cities1Controls.Patches
{
    [HarmonyPatch(typeof(AudioManager), "OnGameLoadingComplete")]
    class AudioManager_OnGameLoadingComplete
    {
        static void Postfix(AudioManager __instance, Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
        {
            if (!mode.IsGameOrEditor())
                return;

            UnityEngine.Debug.Log("Game loaded!");

            __instance.World.GetOrCreateSystem<Cities1ControlSystem>();
        }
    }
}