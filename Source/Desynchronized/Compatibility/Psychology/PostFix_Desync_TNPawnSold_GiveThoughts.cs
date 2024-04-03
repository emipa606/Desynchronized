using Desynchronized.TNDBS;
using HarmonyLib;
using Verse;

namespace Desynchronized.Compatibility.Psychology;

[HarmonyPatch(typeof(TaleNewsPawnSold), "GiveThoughtsToReceipient", MethodType.Normal)]
public class PostFix_Desync_TNPawnSold_GiveThoughts
{
    public static bool Prepare()
    {
        return ModDetector.PsychologyIsLoaded;
    }

    public static void ApplyPsychologyThoughts(TaleNewsPawnSold __instance, Pawn recipient)
    {
        if (!recipient.IsCapableOfThought())
        {
            return;
        }

        var primaryVictim = __instance.PrimaryVictim;
        if (!primaryVictim.RaceProps.Humanlike)
        {
            return;
        }

        // Some prisoner was sold
        if (primaryVictim.IsPrisonerOfColony)
        {
            recipient.needs.mood.thoughts.memories.TryGainMemory(
                Psycho_ThoughtDefOf.KnowPrisonerSoldBleedingHeart, __instance.Instigator);
        }
    }
}