using HarmonyLib;
using RimWorld.Planet;
using Verse;

namespace Desynchronized.Patches.WorldPawns;

[HarmonyPatch(typeof(WorldPawnGC), "GetCriticalPawnReason", MethodType.Normal)]
public class WorldPawnGC_GetCriticalPawnReason
{
    public static void Postfix(Pawn pawn, ref string __result)
    {
        if (__result == null && pawn != null &&
            DesynchronizedMain.TaleNewsDatabaseSystem.PawnIsInvolvedInSomeTaleNews(pawn))
        {
            __result = DesynchronizedMain.Desync_PawnIsReferencedString;
        }
    }
}