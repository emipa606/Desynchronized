using HarmonyLib;
using RimWorld.Planet;

namespace Desynchronized.Patches.NewsTransmit
{
    [HarmonyPatch(typeof(Caravan))]
    [HarmonyPatch("Tick", MethodType.Normal)]
    public class PostFix_Caravan_Ticking
    {
        [HarmonyPostfix]
        public static void TickTheInteractionWorkers(Caravan __instance)
        {
            // Foreach pawn
            var cache = __instance.pawns;
            foreach (var unused in cache)
            {
            }
        }
    }
}