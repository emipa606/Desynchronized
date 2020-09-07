using Desynchronized.TNDBS;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Desynchronized.Compatibility.Psychology
{
    [HarmonyPatch(typeof(TaleNewsPawnHarvested))]
    [HarmonyPatch("GiveThoughtsToReceipient", MethodType.Normal)]
    public class PostFix_Desync_TNPawnHarvested_GiveThoughts
    {
        public static bool Prepare()
        {
            return ModDetector.PsychologyIsLoaded;
        }

        [HarmonyPostfix]
        public static void ApplyPsychologyThoughts(TaleNewsPawnHarvested __instance, Pawn recipient)
        {
            Pawn primaryVictim = __instance.PrimaryVictim;

            if (recipient != primaryVictim)
            {
                // Not the same guy
                // Determine the correct Bleeding Heart thought to be given out
                if (primaryVictim.IsColonist)
                {
                    recipient.needs.mood.thoughts.memories.TryGainMemory(Psycho_ThoughtDefOf.KnowColonistOrganHarvestedBleedingHeart);
                }
                else if (primaryVictim.HostFaction == Faction.OfPlayer)
                {
                    recipient.needs.mood.thoughts.memories.TryGainMemory(Psycho_ThoughtDefOf.KnowGuestOrganHarvestedBleedingHeart);
                }
            }
        }
    }
}
