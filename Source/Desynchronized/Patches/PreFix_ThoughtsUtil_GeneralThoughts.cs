using Desynchronized.Handlers;
using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace Desynchronized.Patches
{
    /// <summary>
    /// Note: sooner or later, the features of this patch will be broken down by the respective handlers.
    /// </summary>
    [HarmonyPatch(typeof(PawnDiedOrDownedThoughtsUtility))]
    [HarmonyPatch("TryGiveThoughts", MethodType.Normal)]
    [HarmonyPatch(new Type[] { typeof(Pawn), typeof(DamageInfo), typeof(PawnDiedOrDownedThoughtsKind) })]
    public class PreFix_ThoughtsUtil_GeneralThoughts
    {
        [HarmonyPrefix]
        public static bool PreFix(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind)
        {
            try
            {
                if (!PawnGenerator.IsBeingGenerated(victim) && Current.ProgramState == ProgramState.Playing)
                {
                    // This is the main redirection for our patches
                    switch (thoughtsKind)
                    {
                        case PawnDiedOrDownedThoughtsKind.Downed:
                            // There is potential here, simply wasted.
                            return false;
                        case PawnDiedOrDownedThoughtsKind.Died:
                            // Let's see if this works; I hope it does.
                            // It works.
                            // Refer to PostFix_Pawn_Kill
                            // Handler_PawnDied.HandlePawnDied(victim, dinfo);
                            return false;
                        case PawnDiedOrDownedThoughtsKind.BanishedToDie:
                            Handler_PawnBanished.HandlePawnBanished(victim, true);
                            return false;
                        case PawnDiedOrDownedThoughtsKind.Banished:
                            Handler_PawnBanished.HandlePawnBanished(victim, false);
                            return false;
                        case PawnDiedOrDownedThoughtsKind.Lost:
                            // Refer to PostFix_Pawn_PreKidnapped
                            return false;
                        default:
                            // This shouldn't happen. You should check things again.
                            throw new InvalidOperationException();
                    }
                }
            }
            catch (Exception arg)
            {
                Log.Error("[V1024-DESYNC] Could not give thought, falling back to vanilla thought-giving procedures: " + arg, false);
            }

            return true;
        }
    }
}
