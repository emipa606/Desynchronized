using Desynchronized.TNDBS;
using RimWorld;
using Verse;

namespace Desynchronized.Handlers;

/// <summary>
///     You must signal this class before instructing it to handle the kidnap event so that the correct response can be
///     given.
/// </summary>
public class Handler_PawnKidnapped
{
    private static Map mapOfOccurence;
    private static Faction kidnappingFaction;
    private static bool flagIsDuringOffensiveBattle;

    public static void Signal_OffensiveBattle_BeginBlock(Map map)
    {
        // The game has already implemented the "Caravan lost" letter, so we don't have to re-invent the wheel.
        mapOfOccurence = map;
        flagIsDuringOffensiveBattle = true;
    }

    public static void Signal_OffensiveBattle_EndBlock()
    {
        flagIsDuringOffensiveBattle = false;
    }

    public static void HandlePawnKidnapped(Pawn victim, Pawn kidnapper)
    {
        // mapOfOccurence should already have been determined before calling this.
        DetermineVariables(kidnapper);

        // Vanilla v1.1. already provides a letter for pawns kidnapped in player's map
        // so no need to reinvent the wheel.
        // This results in no need to send out any custom letters.

        GenerateAndProcessNews(victim, kidnapper);
    }

    private static void DetermineVariables(Pawn kidnapper)
    {
        // Confirm the faction that is responsible for this kidnap
        if (flagIsDuringOffensiveBattle)
        {
            // This is during an offensive battle, the map owner is responsible.
            // mapOfOccurence should have been set in the Signal_BeginBlock function
            kidnappingFaction = mapOfOccurence.ParentFaction;
        }
        else
        {
            // This is during a defensive battle, the kidnapper's faction is responsible.
            mapOfOccurence = kidnapper.MapHeld;
            kidnappingFaction = kidnapper.Faction;
        }
    }

    private static void SendOutNotificationLetter(Pawn victim)
    {
        DesynchronizedMain.LogError(
            "Called SendOutNotificationLetter(Pawn) to give out \"Pawn Kidnapped\" letters, which should not happen due to v1.1's own letters.");

        string letterLabel = "Kidnapped".Translate() + ": " + victim.LabelShortCap;
        var letterContent = string.Empty;
        letterContent += "PawnKidnapped".Translate(victim.LabelShort.CapitalizeFirst(),
            kidnappingFaction.def.pawnsPlural, kidnappingFaction.Name, victim.Named("PAWN"));

        Find.LetterStack.ReceiveLetter(letterLabel, letterContent, LetterDefOf.NegativeEvent, LookTargets.Invalid);
    }

    /// <summary>
    ///     Generate a TaleNews for everybody.
    ///     I have no better idea right now, so this would have to suffice.
    /// </summary>
    /// <param name="victim"></param>
    /// <param name="kidnapper"></param>
    private static void GenerateAndProcessNews(Pawn victim, Pawn kidnapper)
    {
        // Confirm instigator, hence the TaleNews to be transmitted
        var newsPawnKidnapped = kidnapper == null
            ? new TaleNewsPawnKidnapped(victim, kidnappingFaction)
            : new TaleNewsPawnKidnapped(victim, kidnapper);

        // Distribute news; quite standard procedure
        foreach (var other in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
        {
            // DesynchronizedMain.LogError("Parsing " + other.Name + "; he is in map " + other.MapHeld);

            // No special thoughts for victims for now
            if (other == victim)
            {
                continue;
            }

            if (other.IsNearEnough(victim))
            {
                other.GetNewsKnowledgeTracker()?.KnowNews(newsPawnKidnapped);
            }
        }
    }
}