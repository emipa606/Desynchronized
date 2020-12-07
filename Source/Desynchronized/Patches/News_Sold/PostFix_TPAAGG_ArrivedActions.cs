namespace Desynchronized.Patches.News_Sold
{
    /*
    /// <summary>
    /// Retrieves sender information of pods from center storage. Handles the case of player selling whatever pawns via drop-pods.
    /// </summary>
    [HarmonyPatch(typeof(TransportPodsArrivalAction_GiveGift))]
    [HarmonyPatch("Arrived", MethodType.Normal)]
    public class PostFix_TPAAGG_ArrivedActions
    {
        [HarmonyPostfix]
        public static void CheckAndSignalRelevantHandlers(TransportPodsArrivalAction_GiveGift __instance, List<ActiveDropPodInfo> pods)
        {
            Map mapOfSender = DesynchronizedMain.ArrivalActionAndSenderLinker.SafelyGetMapOfGivenAction(__instance);

            for (int i = 0; i < pods.Count; i++)
            {
                for (int j = 0; j < pods[i].innerContainer.Count; j++)
                {
                    Pawn victim = pods[i].innerContainer[j] as Pawn;
                    if (victim != null && victim.RaceProps.Humanlike)
                    {
                        Handler_PawnSold.HandlePawnSold_ByGiftingViaPods(victim, mapOfSender);
                    }
                }
            }
        }
    }
    */
}
