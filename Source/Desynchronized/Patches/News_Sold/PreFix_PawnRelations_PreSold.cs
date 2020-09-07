using Desynchronized.Handlers;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Desynchronized.Patches.News_Sold
{
    /// <summary>
    /// Patches for the case when the player sells a Prisoner
    /// </summary>
    [HarmonyPatch(typeof(Pawn_RelationsTracker))]
    [HarmonyPatch("Notify_PawnSold", MethodType.Normal)]
    public class PreFix_PawnRelations_PreSold
    {
        [HarmonyPrefix]
        public static bool SignalRelevantHandlers(Pawn_RelationsTracker __instance, Pawn playerNegotiator)
        {
            foreach (Pawn potential in Find.WorldPawns.AllPawnsAlive)
            {
                /*
                 * Reversed method for finding the victim.
                 * If RelationsTracker -> pawn does not work, then let's do pawn -> RelationsTracker
                 */
                if (potential.relations == __instance)
                {
                    Handler_PawnSold.HandlePawnSold_ByTrade(potential, playerNegotiator);
                    return false;
                }
            }

            DesynchronizedMain.LogError("Failed to determine owner of Pawn_RelationsTracker. Falling back to vanilla behavior.");
            return true;
        }
    }
}
