using HarmonyLib;
using RimWorld.Planet;
using Verse;

namespace Desynchronized.Patches.WorldPawns;

[HarmonyPatch(typeof(WorldPawnGC))]
[HarmonyPatch("GetCriticalPawnReason", MethodType.Normal)]
public class PostFix_WorldPawnGC_PawnImportanceReason
{
    [HarmonyPostfix]
    public static void AppendCriticalReasonDueToTaleNews(Pawn pawn, ref string __result)
    {
        // DesynchronizedMain.LogError("Requesting GC status for pawn " + pawn + "; game gave reason of " + __result);
        if (__result == null && pawn != null &&
            DesynchronizedMain.TaleNewsDatabaseSystem.PawnIsInvolvedInSomeTaleNews(pawn))
        {
            // DesynchronizedMain.LogError("This pawn should be reserved because TNDBS is referencing it.");
            __result = DesynchronizedMain.Desync_PawnIsReferencedString;
        }
    }
}