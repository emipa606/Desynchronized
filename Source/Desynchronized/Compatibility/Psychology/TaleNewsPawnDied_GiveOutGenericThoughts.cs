using Desynchronized.TNDBS;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Desynchronized.Compatibility.Psychology;

[HarmonyPatch(typeof(TaleNewsPawnDied), "GiveOutGenericThoughts", MethodType.Normal)]
public class TaleNewsPawnDied_GiveOutGenericThoughts
{
    public static bool Prepare()
    {
        return ModDetector.PsychologyIsLoaded;
    }

    public static void Postfix(TaleNewsPawnDied __instance, Pawn recipient)
    {
        var victim = __instance.Victim;

        if (victim.Faction == Faction.OfPlayer && victim.Faction == recipient.Faction &&
            victim.HostFaction != recipient.Faction &&
            ThoughtUtility.CanGetThought(recipient, Psycho_ThoughtDefOf.KnowColonistDiedBleedingHeart, true))
        {
            recipient.needs.mood.thoughts.memories.TryGainMemory(Psycho_ThoughtDefOf.KnowColonistDiedBleedingHeart);
        }

        var prisonerIsInnocent = victim.IsPrisonerOfColony && !victim.guilt.IsGuilty && !victim.InAggroMentalState;
        if (prisonerIsInnocent && recipient.Faction == Faction.OfPlayer && !recipient.IsPrisoner &&
            ThoughtUtility.CanGetThought(recipient, Psycho_ThoughtDefOf.KnowPrisonerDiedInnocentBleedingHeart, true))
        {
            recipient.needs.mood.thoughts.memories.TryGainMemory(Psycho_ThoughtDefOf
                .KnowPrisonerDiedInnocentBleedingHeart);
        }
    }
}