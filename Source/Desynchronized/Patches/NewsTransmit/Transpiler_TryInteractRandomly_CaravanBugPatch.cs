using System.Collections.Generic;
using System.Reflection.Emit;
using Desynchronized.Utilities;
using HarmonyLib;
using RimWorld;

namespace Desynchronized.Patches.NewsTransmit;

[HarmonyPatch(typeof(Pawn_InteractionsTracker))]
[HarmonyPatch("TryInteractRandomly", MethodType.Normal)]
public class Transpiler_TryInteractRandomly_CaravanBugPatch
{
    /// <summary>
    ///     v2.0.0 devnote: consider patching CanInteractNowWith instead, and use a postfix instead.
    ///     Or, see if Hospitality has any updated code to use.
    /// </summary>
    /// <param name="instructions"></param>
    /// <param name="generator"></param>
    /// <returns></returns>
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions,
        ILGenerator generator)
    {
        var patchIsComplete = false;

        foreach (var instr in instructions)
        {
            yield return instr;

            if (patchIsComplete || instr.opcode != OpCodes.Stloc_0)
            {
                continue;
            }

            yield return new CodeInstruction(OpCodes.Ldloc_0);
            yield return new CodeInstruction(OpCodes.Ldarg_0);
            yield return new CodeInstruction(OpCodes.Ldfld,
                AccessTools.Field(typeof(Pawn_InteractionsTracker), "pawn"));
            yield return new CodeInstruction(OpCodes.Call,
                typeof(CaravanUtilities).GetMethod("ManipulateInteractionTargetsList"));

            patchIsComplete = true;
        }
    }
}