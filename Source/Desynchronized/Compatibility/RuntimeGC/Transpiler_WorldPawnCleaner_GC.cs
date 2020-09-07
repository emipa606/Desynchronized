using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace Desynchronized.Compatibility.RuntimeGC
{
    /// <summary>
    /// Uses dynamic targetting to ensure better compatibility.
    /// </summary>
    [HarmonyPatch]
    public class Transpiler_WorldPawnCleaner_GC
    {
        private static bool PresenceOfRuntimeGC => LoadedModManager.RunningMods.Any((ModContentPack pack) => pack.Name.Contains("RuntimeGC"));

        public static MethodBase TargetMethod()
        {
            return AccessTools.Method("RuntimeGC.WorldPawnCleaner:GC");
        }

        /// <summary>
        /// DetectRuntimeGC
        /// </summary>
        /// <returns></returns>
        public static bool Prepare()
        {
            return PresenceOfRuntimeGC;
        }
        
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> ManipulateTalePawnList(IEnumerable<CodeInstruction> instructions)
        {
            // Find the 2nd "call" after the 6th "ldstr"
            // Insert a method after that position to manipulate the list.
            int occurencesLdstr = 0;
            int occurencesCall = 0;
            List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);
            int i = 0;

            while (occurencesLdstr < 6)
            {
                if (instructionList[i].opcode == OpCodes.Ldstr)
                {
                    occurencesLdstr++;
                }
                i++;
            }

            while (occurencesCall < 2)
            {
                if (instructionList[i].opcode == OpCodes.Call)
                {
                    occurencesCall++;
                }
                i++;
            }

            // i is at our insert position.
            // Insert new commands.
            List<CodeInstruction> insertionList = new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldloc_1),
                new CodeInstruction(OpCodes.Call, typeof(TalePawnListManipulator).GetMethod("ManipulateListOfPawnsUsedByTales"))
            };
            instructionList.InsertRange(i, insertionList);

            // Insertion complete.
            return instructionList;
        }
    }
}
