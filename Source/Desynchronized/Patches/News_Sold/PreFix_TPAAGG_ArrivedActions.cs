using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using Verse;

namespace Desynchronized.Patches.News_Sold
{
    [HarmonyPatch(typeof(TransportPodsArrivalAction_GiveGift))]
    [HarmonyPatch("Arrived", MethodType.Normal)]
    public class PreFix_TPAAGG_ArrivedActions
    {
        [HarmonyPrefix]
        public static bool CheckAndSignalRelevantHandlers(TransportPodsArrivalAction_GiveGift __instance, List<ActiveDropPodInfo> pods)
        {
			Settlement settlement = Traverse.Create(__instance).Field("settlement").GetValue<Settlement>();
			Map mapOfSender = DesynchronizedMain.ArrivalActionAndSenderLinker.SafelyGetMapOfGivenAction(__instance);

			for (var i = 0; i < pods.Count; i++)
			{
				for (var j = 0; j < pods[i].innerContainer.Count; j++)
				{
                    if (!(pods[i].innerContainer[j] is Pawn pawn))
                    {
                        continue;
                    }
                }
			}
			FactionGiftUtility.GiveGift(pods, settlement);

			// Original method need not be called.
			return false;
        }
    }
}
