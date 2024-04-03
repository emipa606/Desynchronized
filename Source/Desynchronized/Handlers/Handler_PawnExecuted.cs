using Desynchronized.TNDBS;
using Desynchronized.TNDBS.Datatypes;
using RimWorld;
using Verse;

namespace Desynchronized.Handlers;

public class Handler_PawnExecuted
{
    /// <summary>
    ///     Handles a pawn execution event.
    /// </summary>
    /// <param name="victim"></param>
    /// <param name="brutality"></param>
    public static void HandlePawnExecuted(Pawn victim, DeathBrutality brutality)
    {
        SendOutNotificationLetter(victim);
        GenerateAndProcessNews(victim, brutality);
    }

    private static void SendOutNotificationLetter(Pawn victim)
    {
        // string letterLabel = "Kidnapped".Translate() + ": " + victim.LabelShortCap;
        // string letterContent = string.Empty;
        // letterContent += "PawnKidnapped".Translate(victim.LabelShort.CapitalizeFirst(), kidnapper.Faction.def.pawnsPlural, kidnapper.Faction.Name, victim.Named("PAWN"));

        Find.LetterStack.ReceiveLetter("Colonist/Guest executed",
            $"Colonist/Guest was executed. Name of Pawn: {victim.Name}", LetterDefOf.NegativeEvent, victim);
    }

    private static void GenerateAndProcessNews(Pawn victim, DeathBrutality brutality)
    {
        //TaleNewsPawnDied executionNews = TaleNewsPawnDied.GenerateAsExecution(victim, brutality);
        var executionNews = new TaleNewsPawnDied(victim, brutality);

        foreach (var other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
        {
            if (other.IsNearEnough(victim))
            {
                other.GetNewsKnowledgeTracker()?.KnowNews(executionNews);
            }
        }
    }
}