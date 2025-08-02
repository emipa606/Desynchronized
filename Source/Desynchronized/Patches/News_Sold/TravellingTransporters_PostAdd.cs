using HarmonyLib;
using RimWorld.Planet;

namespace Desynchronized.Patches.News_Sold;

/// <summary>
///     Marks down all the Gift Sender pods leaving their launching map.
/// </summary>
[HarmonyPatch(typeof(TravellingTransporters), nameof(TravellingTransporters.PostAdd), MethodType.Normal)]
public class TravellingTransporters_PostAdd
{
    public static void Postfix(TravellingTransporters __instance)
    {
        var arrivalAction = __instance.arrivalAction;
        if (arrivalAction is TransportersArrivalAction_GiveGift arrivalActionGG)
        {
            DesynchronizedMain.ArrivalActionAndSenderLinker.EstablishRelationship(arrivalActionGG, __instance.Tile);
        }
    }
}