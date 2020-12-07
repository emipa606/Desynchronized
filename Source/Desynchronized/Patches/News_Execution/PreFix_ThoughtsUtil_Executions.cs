using Desynchronized.Handlers;
using Desynchronized.TNDBS.Datatypes;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Desynchronized.Patches.News_Execution
{
    [HarmonyPatch(typeof(ThoughtUtility))]
    [HarmonyPatch("GiveThoughtsForPawnExecuted", MethodType.Normal)]
    public class PreFix_ThoughtsUtil_Executions
    {
        [HarmonyPrefix]
        public static bool SignalRelevantHandlersAndPreventVanillaThoughts(Pawn victim, PawnExecutionKind kind)
        {
            if (victim.RaceProps.Humanlike)
            {
                DeathBrutality brutality = DeathBrutality.HUMANE;
                if (!victim.guilt.IsGuilty)
                {
                    switch (kind)
                    {
                        case PawnExecutionKind.GenericHumane:
                            brutality = DeathBrutality.HUMANE;
                            break;
                        case PawnExecutionKind.GenericBrutal:
                            brutality = DeathBrutality.BRUTAL;
                            break;
                        case PawnExecutionKind.OrganHarvesting:
                            brutality = DeathBrutality.UNETHICAL;
                            break;
                    }
                }
                else
                {
                    brutality = DeathBrutality.JUSTIFIED;
                }
                Handler_PawnExecuted.HandlePawnExecuted(victim, brutality);
            }

            // It's not like we have anything more to process or anything, baka.
            return false;
        }
    }
}
