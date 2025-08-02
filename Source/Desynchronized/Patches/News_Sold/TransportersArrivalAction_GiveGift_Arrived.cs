using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Desynchronized.Patches.News_Sold;

[HarmonyPatch(typeof(TransportersArrivalAction_GiveGift), nameof(TransportersArrivalAction_GiveGift.Arrived),
    MethodType.Normal)]
public class TransportersArrivalAction_GiveGift_Arrived
{
    public static bool Prefix(TransportersArrivalAction_GiveGift __instance,
        List<ActiveTransporterInfo> transporters)
    {
        var settlement = Traverse.Create(__instance).Field("settlement").GetValue<Settlement>();
        _ = DesynchronizedMain.ArrivalActionAndSenderLinker.SafelyGetMapOfGivenAction(__instance);

        foreach (var activeDropPodInfo in transporters)
        {
            foreach (var thing in activeDropPodInfo.innerContainer)
            {
                if (thing is not Pawn)
                {
                }
            }
        }

        FactionGiftUtility.GiveGift(transporters, settlement);

        // Original method need not be called.
        return false;
    }
}