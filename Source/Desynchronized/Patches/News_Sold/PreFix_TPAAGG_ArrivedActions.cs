using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Desynchronized.Patches.News_Sold;

[HarmonyPatch(typeof(TransportPodsArrivalAction_GiveGift))]
[HarmonyPatch("Arrived", MethodType.Normal)]
public class PreFix_TPAAGG_ArrivedActions
{
    [HarmonyPrefix]
    public static bool CheckAndSignalRelevantHandlers(TransportPodsArrivalAction_GiveGift __instance,
        List<ActiveDropPodInfo> pods)
    {
        var settlement = Traverse.Create(__instance).Field("settlement").GetValue<Settlement>();
        var unused = DesynchronizedMain.ArrivalActionAndSenderLinker.SafelyGetMapOfGivenAction(__instance);

        foreach (var activeDropPodInfo in pods)
        {
            foreach (var thing in activeDropPodInfo.innerContainer)
            {
                if (!(thing is Pawn))
                {
                }
            }
        }

        FactionGiftUtility.GiveGift(pods, settlement);

        // Original method need not be called.
        return false;
    }
}