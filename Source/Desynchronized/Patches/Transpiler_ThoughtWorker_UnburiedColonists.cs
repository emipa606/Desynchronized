using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;

namespace Desynchronized.Patches
{
    /*
    [HarmonyPatch(typeof(ThoughtWorker_ColonistLeftUnburied))]
    [HarmonyPatch("CurrentStateInternal", MethodType.Normal)]
    public class Transpiler_ThoughtWorker_UnburiedColonists
    {
        /// <summary>
        /// Inserts a "if map not same then continue" statement
        /// 
        /// v2.0.0 devnote: this seems unnecessary for vanilla v1.1.
        /// </summary>
        /// <param name="instructions"></param>
        /// <param name="generator"></param>
        /// <returns></returns>
        // [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Label labelContinue = generator.DefineLabel();
            bool patchIsComplete = false;
            int occurenceLdloc1 = 0;
            bool labelInsertComplete = false;

            foreach (CodeInstruction instruction in instructions)
            {
                if (!patchIsComplete && instruction.opcode == OpCodes.Ldloc_2)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Callvirt, typeof(Pawn).GetProperty("Map").GetGetMethod());
                    yield return new CodeInstruction(OpCodes.Ldloc_2);
                    yield return new CodeInstruction(OpCodes.Callvirt, typeof(Pawn).GetProperty("Map").GetGetMethod());
                    yield return new CodeInstruction(OpCodes.Ceq);
                    yield return new CodeInstruction(OpCodes.Brfalse_S, labelContinue);

                    patchIsComplete = true;
                }
                if (!labelInsertComplete && instruction.opcode == OpCodes.Ldloc_1)
                {
                    occurenceLdloc1++;

                    if (occurenceLdloc1 == 2)
                    {
                        instruction.labels.Add(labelContinue);
                        labelInsertComplete = true;
                    }
                }

                yield return instruction;
            }
        }
    }
    */
}
