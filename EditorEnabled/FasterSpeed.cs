using System.Collections.Generic;
using Game.Simulation;
using HarmonyLib;
using System.Reflection.Emit;

namespace EditorEnabled
{
    [HarmonyPatch(typeof(SimulationSystem), "OnUpdate")]
    public class SimulationSystem_OnUpdatePatch
    {
        public static void Prefix(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Call)
                {
                    instruction.operand = 1f;
                }
            }
        }
    }
}
