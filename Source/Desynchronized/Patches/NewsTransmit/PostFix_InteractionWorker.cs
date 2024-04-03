using Desynchronized.TNDBS.Utilities;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Desynchronized.Patches.NewsTransmit;

[HarmonyPatch(typeof(InteractionWorker), nameof(InteractionWorker.Interacted), MethodType.Normal)]
public class PostFix_InteractionWorker
{
    [HarmonyPostfix]
    public static void ExecuteNewsTarnsmission(InteractionWorker __instance, Pawn initiator, Pawn recipient)
    {
        if (__instance is InteractionWorker_Chitchat)
        {
            if (Rand.Value <= initiator.GetActualNewsSpreadChance())
            {
                NewsSpreadUtility.SpreadNews(initiator, recipient);
            }
        }
        else if (__instance is InteractionWorker_DeepTalk)
        {
            if (Rand.Value <= initiator.GetActualNewsSpreadChance(5))
            {
                NewsSpreadUtility.SpreadNews(initiator, recipient, NewsSpreadUtility.SpreadMode.DISTINCT);
            }
        }
    }
}