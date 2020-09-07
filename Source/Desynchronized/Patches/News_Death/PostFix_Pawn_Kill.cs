using Desynchronized.Handlers;
using HarmonyLib;
using Verse;

namespace Desynchronized.Patches.News_Death
{
    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch("Kill", MethodType.Normal)]
    public class PostFix_Pawn_Kill
    {
        [HarmonyPostfix]
        public static void SignalRelevantHandlers(Pawn __instance, DamageInfo? dinfo, Hediff exactCulprit)
        {
            Handler_PawnDied.HandlePawnDied(__instance, dinfo, exactCulprit);
        }
    }
}
