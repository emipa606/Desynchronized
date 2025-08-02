using Desynchronized.TNDBS;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Desynchronized.Compatibility.Psychology;

[HarmonyPatch(typeof(TaleNewsPawnHarvested), "GiveThoughtsToReceipient", MethodType.Normal)]
public class TaleNewsPawnHarvested_GiveThoughtsToReceipient
{
    public static bool Prepare()
    {
        return ModDetector.PsychologyIsLoaded;
    }

    public static void Postfix(TaleNewsPawnHarvested __instance, Pawn recipient)
    {
        var primaryVictim = __instance.PrimaryVictim;

        if (recipient == primaryVictim)
        {
            return;
        }

        // Not the same guy
        // Determine the correct Bleeding Heart thought to be given out
        if (primaryVictim.IsColonist)
        {
            if (ThoughtUtility.CanGetThought(recipient, Psycho_ThoughtDefOf.KnowColonistOrganHarvestedBleedingHeart,
                    true))
            {
                recipient.needs.mood.thoughts.memories.TryGainMemory(Psycho_ThoughtDefOf
                    .KnowColonistOrganHarvestedBleedingHeart);
            }

            return;
        }

        if (primaryVictim.HostFaction != Faction.OfPlayer)
        {
            return;
        }

        if (ThoughtUtility.CanGetThought(recipient, Psycho_ThoughtDefOf.KnowGuestOrganHarvestedBleedingHeart, true))
        {
            recipient.needs.mood.thoughts.memories.TryGainMemory(Psycho_ThoughtDefOf
                .KnowGuestOrganHarvestedBleedingHeart);
        }
    }
}