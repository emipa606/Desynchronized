using Desynchronized.TNDBS;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Desynchronized.Compatibility.Psychology;

[HarmonyPatch(typeof(TaleNewsPawnBanished), "GiveThoughtsToReceipient", MethodType.Normal)]
public class TaleNewsPawnBanished_GiveThoughtsToReceipient
{
    public static bool Prepare()
    {
        return ModDetector.PsychologyIsLoaded;
    }

    public static void Postfix(TaleNewsPawnBanished __instance, Pawn recipient)
    {
        var banishmentVictim = __instance.BanishmentVictim;

        if (!banishmentVictim.RaceProps.Humanlike || recipient == banishmentVictim)
        {
            return;
        }

        ThoughtDef thoughtDefToGain = null;
        if (!banishmentVictim.IsPrisonerOfColony)
        {
            thoughtDefToGain = __instance.IsDeadly
                ? Psycho_ThoughtDefOf.ColonistAbandonedToDieBleedingHeart
                : Psycho_ThoughtDefOf.ColonistAbandonedBleedingHeart;
        }
        else
        {
            if (__instance.IsDeadly)
            {
                thoughtDefToGain = Psycho_ThoughtDefOf.PrisonerAbandonedToDieBleedingHeart;
            }
        }

        if (ThoughtUtility.CanGetThought(recipient, thoughtDefToGain, true))
        {
            recipient.needs.mood.thoughts.memories.TryGainMemory(thoughtDefToGain, banishmentVictim);
        }
    }
}