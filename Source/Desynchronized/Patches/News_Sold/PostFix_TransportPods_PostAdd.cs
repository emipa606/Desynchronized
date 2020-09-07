using HarmonyLib;
using RimWorld.Planet;

namespace Desynchronized.Patches.News_Sold
{
    /// <summary>
    /// Marks down all the Gift Sender pods leaving their launching map.
    /// </summary>
    [HarmonyPatch(typeof(TravelingTransportPods))]
    [HarmonyPatch("PostAdd", MethodType.Normal)]
    public class PostFix_TransportPods_PostAdd
    {
        [HarmonyPostfix]
        public static void AddPodsToTracker(TravelingTransportPods __instance)
        {
            TransportPodsArrivalAction arrivalAction = __instance.arrivalAction;
            if (arrivalAction is TransportPodsArrivalAction_GiveGift arrivalActionGG)
            {
                DesynchronizedMain.ArrivalActionAndSenderLinker.EstablishRelationship(arrivalActionGG, __instance.Tile);
            }
        }
    }
}
