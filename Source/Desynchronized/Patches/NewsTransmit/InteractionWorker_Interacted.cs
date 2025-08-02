using Desynchronized.TNDBS.Utilities;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Desynchronized.Patches.NewsTransmit;

[HarmonyPatch(typeof(InteractionWorker), nameof(InteractionWorker.Interacted), MethodType.Normal)]
public class InteractionWorker_Interacted
{
    public static void Postfix(InteractionWorker __instance, Pawn initiator, Pawn recipient)
    {
        switch (__instance)
        {
            case InteractionWorker_Chitchat:
            {
                if (Rand.Value <= initiator.GetActualNewsSpreadChance())
                {
                    NewsSpreadUtility.SpreadNews(initiator, recipient);
                }

                break;
            }
            case InteractionWorker_DeepTalk:
            {
                if (Rand.Value <= initiator.GetActualNewsSpreadChance(5))
                {
                    NewsSpreadUtility.SpreadNews(initiator, recipient, NewsSpreadUtility.SpreadMode.DISTINCT);
                }

                break;
            }
        }
    }
}