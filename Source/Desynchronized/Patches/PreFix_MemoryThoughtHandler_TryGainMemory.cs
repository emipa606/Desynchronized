using Desynchronized.Handlers;
using Desynchronized.TNDBS;
using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace Desynchronized.Patches
{

    /// <summary>
    /// Prefix to catch the corpse location
    /// </summary>
    [HarmonyPatch(typeof(Corpse))]
    [HarmonyPatch("ButcherProducts", MethodType.Normal)]
    [HarmonyPatch(new Type[] { typeof(Pawn), typeof(float) })]
    public class PreFix_Corpse_ButcherProducts
    {
        public static Corpse corpse;
        public static IntVec3 corpseLocation;
        public static Map corpseMap;
        public static Room corpseRoom;
        public static bool overrideValue;
        public static TaleNewsPawnButchered corpseNews;

        [HarmonyPrefix]
        public static void PreFix(Pawn butcher, float efficiency, ref Corpse __instance)
        {
            corpse = __instance;
            corpseLocation = __instance.PositionHeld;
            corpseMap = __instance.MapHeld;
            corpseRoom = __instance.GetRoom(); 
            overrideValue = false;
            corpseNews = new TaleNewsPawnButchered(corpse.InnerPawn);
        }
    }

    /// <summary>
    /// General prefix since it happens inside the butcher function
    /// </summary>
    [HarmonyPatch(typeof(MemoryThoughtHandler))]
    [HarmonyPatch("TryGainMemory", MethodType.Normal)]
    [HarmonyPatch(new Type[] { typeof(ThoughtDef), typeof(Pawn) })]
    public class PreFix_MemoryThoughtHandler_TryGainMemory
    {
        [HarmonyPrefix]
        public static bool PreFix(ThoughtDef def, Pawn otherPawn, ref MemoryThoughtHandler __instance)
        {
            if(PreFix_Corpse_ButcherProducts.overrideValue)
            {
                return true;
            }
            try
            {
                if (def == null)
                {
                    return true;
                }
                if (def != ThoughtDefOf.KnowButcheredHumanlikeCorpse && def != ThoughtDefOf.ButcheredHumanlikeCorpse)
                {
                    return true;
                }
                Handler_PawnButchered.HandlePawnButchered(__instance.pawn);
                return false;
            }
            catch (Exception arg)
            {
                Log.Error("[V1024-DESYNC] Could not give thought, falling back to vanilla thought-giving procedures: " + arg, false);
            }

            return true;
        }
    }
}
