using Desynchronized.TNDBS;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Desynchronized.Compatibility.Psychology
{
    [HarmonyPatch(typeof(TaleNewsPawnDied))]
    [HarmonyPatch("GiveOutEyewitnessThoughts", MethodType.Normal)]
    public class PostFix_Desync_TNPawnDied_EyeWitnesses
    {
        public static bool Prepare()
        {
            return ModDetector.PsychologyIsLoaded;
        }

        [HarmonyPostfix]
        public static void AppendPsychologyThoughts(TaleNewsPawnDied __instance, Pawn recipient)
        {
            Pawn killer = __instance.Killer;
            Pawn victim = __instance.Victim;

            // Psychology did a lot of work to exclude thoughts from Bleeding Heart.

            if (recipient.Faction == victim.Faction)
            {
                new IndividualThoughtToAdd(Psycho_ThoughtDefOf.WitnessedDeathAllyBleedingHeart, recipient).Add();
            }
            else if (victim.Faction == null || victim.Faction.HostileTo(recipient.Faction) || recipient.story.traits.HasTrait(Psycho_TraitDefOf.BleedingHeart))
            {
                new IndividualThoughtToAdd(Psycho_ThoughtDefOf.WitnessedDeathNonAllyBleedingHeart, recipient).Add();
            }
            bool traitsDisallowDesensitization = recipient.story.traits.HasTrait(Psycho_TraitDefOf.BleedingHeart) || recipient.story.traits.HasTrait(TraitDefOf.Psychopath) || recipient.story.traits.HasTrait(TraitDefOf.Bloodlust) || recipient.story.traits.HasTrait(Psycho_TraitDefOf.Desensitized);
            // ALL PRAISE GOD RANDY OUR ONE TRUE LORD
            bool randyAllowsDesensitization = (recipient.GetHashCode() ^ ((GenLocalDate.DayOfYear(recipient) + GenLocalDate.Year(recipient) + (int)(GenLocalDate.DayPercent(recipient) * 5f) * 60) * 391)) % 1000 == 0;
            // No Bleeding Heart + No Psychopath + No Bloodlust + No Desensitised + Random Genner
            if (!traitsDisallowDesensitization && randyAllowsDesensitization)
            {
                // Gain Desensitized
                recipient.story.traits.GainTrait(new Trait(Psycho_TraitDefOf.Desensitized));
                recipient.needs.mood.thoughts.memories.TryGainMemory(Psycho_ThoughtDefOf.RecentlyDesensitized);
            }
        }
    }
}
