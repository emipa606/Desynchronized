using RimWorld;
using Verse;

namespace Desynchronized.Compatibility.Psychology
{
    public class Psycho_ThoughtDefOf
    {
        // Using DefDatabase instead of the DefOf annotation to allow for silent errors.
        public static ThoughtDef KnowColonistDiedBleedingHeart = DefDatabase<ThoughtDef>.GetNamed("KnowColonistDiedBleedingHeart");
        public static ThoughtDef KnowPrisonerDiedInnocentBleedingHeart = DefDatabase<ThoughtDef>.GetNamed("KnowPrisonerDiedInnocentBleedingHeart");

        public static ThoughtDef ColonistAbandonedBleedingHeart = DefDatabase<ThoughtDef>.GetNamed("ColonistAbandonedBleedingHeart");
        public static ThoughtDef ColonistAbandonedToDieBleedingHeart = DefDatabase<ThoughtDef>.GetNamed("ColonistAbandonedToDieBleedingHeart");
        public static ThoughtDef PrisonerAbandonedToDieBleedingHeart = DefDatabase<ThoughtDef>.GetNamed("PrisonerAbandonedToDieBleedingHeart");

        public static ThoughtDef KnowColonistExecutedBleedingHeart = DefDatabase<ThoughtDef>.GetNamed("KnowColonistExecutedBleedingHeart");
        public static ThoughtDef KnowGuestExecutedBleedingHeart = DefDatabase<ThoughtDef>.GetNamed("KnowGuestExecutedBleedingHeart");

        public static ThoughtDef WitnessedDeathAllyBleedingHeart = DefDatabase<ThoughtDef>.GetNamed("WitnessedDeathAllyBleedingHeart");
        public static ThoughtDef WitnessedDeathNonAllyBleedingHeart = DefDatabase<ThoughtDef>.GetNamed("WitnessedDeathNonAllyBleedingHeart");
        public static ThoughtDef RecentlyDesensitized = DefDatabase<ThoughtDef>.GetNamed("RecentlyDesensitized");

        public static ThoughtDef KnowColonistOrganHarvestedBleedingHeart = DefDatabase<ThoughtDef>.GetNamed("KnowColonistOrganHarvestedBleedingHeart");
        public static ThoughtDef KnowGuestOrganHarvestedBleedingHeart = DefDatabase<ThoughtDef>.GetNamed("KnowGuestOrganHarvestedBleedingHeart");

        public static ThoughtDef KilledHumanlikeEnemy = DefDatabase<ThoughtDef>.GetNamed("KilledHumanlikeEnemy");

        public static ThoughtDef KnowPrisonerSoldBleedingHeart = DefDatabase<ThoughtDef>.GetNamed("KnowPrisonerSoldBleedingHeart");
    }
}
