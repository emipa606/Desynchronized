using Desynchronized.TNDBS;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Desynchronized.Compatibility.Psychology
{
    [HarmonyPatch(typeof(TaleNewsPawnDied))]
    [HarmonyPatch("GiveOutGenericThoughts", MethodType.Normal)]
    public class PostFix_Desync_TNPawnDied_GenericThoughts
    {
        public static bool Prepare()
        {
            return ModDetector.PsychologyIsLoaded;
        }

        [HarmonyPostfix]
        public static void ApplyPsychologyThoughts(TaleNewsPawnDied __instance, Pawn recipient)
        {
            Pawn killer = __instance.Killer;
            Pawn victim = __instance.Victim;

            if (victim.Faction == Faction.OfPlayer && victim.Faction == recipient.Faction && victim.HostFaction != recipient.Faction)
            {
                recipient.needs.mood.thoughts.memories.TryGainMemory(Psycho_ThoughtDefOf.KnowColonistDiedBleedingHeart);
            }
            bool prisonerIsInnocent = victim.IsPrisonerOfColony && !victim.guilt.IsGuilty && !victim.InAggroMentalState;
            if (prisonerIsInnocent && recipient.Faction == Faction.OfPlayer && !recipient.IsPrisoner)
            {
                recipient.needs.mood.thoughts.memories.TryGainMemory(Psycho_ThoughtDefOf.KnowPrisonerDiedInnocentBleedingHeart);
            }
        }
    }
}
