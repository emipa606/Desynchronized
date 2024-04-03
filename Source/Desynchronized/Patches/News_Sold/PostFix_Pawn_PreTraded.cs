using Desynchronized.Handlers;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Desynchronized.Patches.News_Sold;

[HarmonyPatch(typeof(Pawn), nameof(Pawn.PreTraded), MethodType.Normal)]
public class PostFix_Pawn_PreTraded
{
    [HarmonyPostfix]
    public static void SignalRelevantHandlers(Pawn __instance, TradeAction action, Pawn playerNegotiator)
    {
        if (action != TradeAction.PlayerSells)
        {
            return;
        }

        if (__instance.RaceProps.Humanlike)
        {
            Handler_PawnSold.HandlePawnSold_ByTrade(__instance, playerNegotiator);
        }
    }
}