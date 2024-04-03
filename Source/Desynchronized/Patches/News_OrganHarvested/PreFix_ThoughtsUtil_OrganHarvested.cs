using Desynchronized.Handlers;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Desynchronized.Patches.News_OrganHarvested;

[HarmonyPatch(typeof(ThoughtUtility), nameof(ThoughtUtility.GiveThoughtsForPawnOrganHarvested), MethodType.Normal)]
public class PreFix_ThoughtsUtil_OrganHarvested
{
    [HarmonyPrefix]
    public static bool PreFix(Pawn victim)
    {
        // Log.Error("Not an error, but we are executing prefix of organ harvested.");
        Handler_PawnHarvested.HandlePawnHarvested(victim);

        // It's not we have anything left to process anymore or anything, baka.
        return false;
    }
}