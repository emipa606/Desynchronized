using Desynchronized.TNDBS;
using Desynchronized.TNDBS.Utilities;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Desynchronized.Handlers
{
    public class Handler_PawnHarvested
    {
        public static void HandlePawnHarvested(Pawn victim)
        {
            // No need to send out letters, the player has full control of the entire operation.
            GenerateAndProcessNews(victim);
        }

        private static void GenerateAndProcessNews(Pawn victim)
        {
            // Definitely has potential here.
            if (!victim.RaceProps.Humanlike)
            {
                return;
            }

            TaleNewsPawnHarvested harvestNews = new TaleNewsPawnHarvested(victim);

            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
            {
                if (other.IsInSameMapOrCaravan(victim))
                {
                    other.GetNewsKnowledgeTracker().KnowNews(harvestNews);
                }
            }
        }
    }
}
