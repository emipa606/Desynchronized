using RimWorld;
using Verse;

namespace Desynchronized.Compatibility.Psychology;

public class Psycho_ThoughtDefOf
{
    // Using DefDatabase instead of the DefOf annotation to allow for silent errors.
    public static readonly ThoughtDef KnowColonistDiedBleedingHeart =
        DefDatabase<ThoughtDef>.GetNamed("KnowColonistDiedBleedingHeart");

    public static readonly ThoughtDef KnowPrisonerDiedInnocentBleedingHeart =
        DefDatabase<ThoughtDef>.GetNamed("KnowPrisonerDiedInnocentBleedingHeart");

    public static readonly ThoughtDef ColonistAbandonedBleedingHeart =
        DefDatabase<ThoughtDef>.GetNamed("ColonistAbandonedBleedingHeart");

    public static readonly ThoughtDef ColonistAbandonedToDieBleedingHeart =
        DefDatabase<ThoughtDef>.GetNamed("ColonistAbandonedToDieBleedingHeart");

    public static readonly ThoughtDef PrisonerAbandonedToDieBleedingHeart =
        DefDatabase<ThoughtDef>.GetNamed("PrisonerAbandonedToDieBleedingHeart");

    public static readonly ThoughtDef KnowColonistExecutedBleedingHeart =
        DefDatabase<ThoughtDef>.GetNamed("KnowColonistExecutedBleedingHeart");

    public static readonly ThoughtDef KnowGuestExecutedBleedingHeart =
        DefDatabase<ThoughtDef>.GetNamed("KnowGuestExecutedBleedingHeart");

    public static readonly ThoughtDef WitnessedDeathAllyBleedingHeart =
        DefDatabase<ThoughtDef>.GetNamed("WitnessedDeathAllyBleedingHeart");

    public static readonly ThoughtDef WitnessedDeathNonAllyBleedingHeart =
        DefDatabase<ThoughtDef>.GetNamed("WitnessedDeathNonAllyBleedingHeart");

    public static readonly ThoughtDef RecentlyDesensitized = DefDatabase<ThoughtDef>.GetNamed("RecentlyDesensitized");

    public static readonly ThoughtDef KnowColonistOrganHarvestedBleedingHeart =
        DefDatabase<ThoughtDef>.GetNamed("KnowColonistOrganHarvestedBleedingHeart");

    public static readonly ThoughtDef KnowGuestOrganHarvestedBleedingHeart =
        DefDatabase<ThoughtDef>.GetNamed("KnowGuestOrganHarvestedBleedingHeart");

    public static readonly ThoughtDef KilledHumanlikeEnemy = DefDatabase<ThoughtDef>.GetNamed("KilledHumanlikeEnemy");

    public static readonly ThoughtDef KnowPrisonerSoldBleedingHeart =
        DefDatabase<ThoughtDef>.GetNamed("KnowPrisonerSoldBleedingHeart");
}