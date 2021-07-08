using Desynchronized.TNDBS;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Desynchronized.Compatibility.Psychology
{
    [HarmonyPatch(typeof(TaleNewsPawnDied))]
    [HarmonyPatch("TryProcessKillerHigh", MethodType.Normal)]
    public class PostFix_Desync_TNPawnDied_KillerHigh
    {
        public static bool Prepare()
        {
            return ModDetector.PsychologyIsLoaded;
        }

        [HarmonyPostfix]
        public static void AddPsychologyThoughts(TaleNewsPawnDied __instance, Pawn recipient)
        {
            var killer = __instance.Killer;
            var victim = __instance.Victim;

            // Killer != null => DamageInfo != null
            if (killer == null)
            {
                return;
            }

            // Currently you can't really kill yourself.
            if (recipient != killer)
            {
                return;
            }

            if (!__instance.KillingBlowDamageDef.ExternalViolenceFor(victim))
            {
                return;
            }

            if (killer.story == null)
            {
                return;
            }

            // Try to add "Killed Enemy Humanlike" thought
            // Check the conditions
            if (!victim.RaceProps.Humanlike)
            {
                return;
            }

            if (killer.HostileTo(victim) && killer.Faction != null &&
                killer.Faction.HostileTo(victim.Faction))
            {
                new IndividualThoughtToAdd(Psycho_ThoughtDefOf.KilledHumanlikeEnemy, killer, victim)
                    .Add();
            }
        }
    }
}