using Desynchronized.Handlers;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Desynchronized.Patches.News_Sold;

[HarmonyPatch(typeof(Pawn), nameof(Pawn.PreTraded), MethodType.Normal)]
public class Pawn_PreTraded
{
    public static void Postfix(Pawn __instance, TradeAction action, Pawn playerNegotiator)
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