using System;
using Desynchronized.Handlers;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Desynchronized.Patches;

/// <summary>
///     General prefix since it happens inside the butcher function
/// </summary>
[HarmonyPatch(typeof(MemoryThoughtHandler), nameof(MemoryThoughtHandler.TryGainMemory),
    [typeof(ThoughtDef), typeof(Pawn), typeof(Precept)])]
public class PreFix_MemoryThoughtHandler_TryGainMemory
{
    [HarmonyPrefix]
    public static bool PreFix(ThoughtDef def, ref MemoryThoughtHandler __instance)
    {
        if (PreFix_Corpse_ButcherProducts.overrideValue)
        {
            return true;
        }

        try
        {
            if (def == null)
            {
                return true;
            }

            if (def != Desynchronized_ThoughtDefOf.KnowButcheredHumanlikeCorpse &&
                def != Desynchronized_ThoughtDefOf.ButcheredHumanlikeCorpse)
            {
                return true;
            }

            Handler_PawnButchered.HandlePawnButchered(__instance.pawn);
            return false;
        }
        catch (Exception arg)
        {
            Log.Error(
                $"[V1024-DESYNC] Could not give thought, falling back to vanilla thought-giving procedures: {arg}");
        }

        return true;
    }
}