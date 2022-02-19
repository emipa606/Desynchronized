using Desynchronized.TNDBS;
using RimWorld;
using Verse;

namespace Desynchronized.Handlers;

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

        var harvestNews = new TaleNewsPawnHarvested(victim);

        foreach (var other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
        {
            if (other.IsNearEnough(victim))
            {
                other.GetNewsKnowledgeTracker()?.KnowNews(harvestNews);
            }
        }
    }
}