using Desynchronized.TNDBS;
using HarmonyLib;
using Verse;
using Verse.AI;

namespace Desynchronized.Patches.Hediffs_MentalBreaks
{
    /*
    /// <summary>
    /// v2.0.0 devnote: vanilla v1.1 removed the PostStart function, so I assume they also changed how the structures work.
    /// 
    /// Disabling this for now.
    /// </summary>
    [HarmonyPatch(typeof(MentalState_WanderConfused))]
    [HarmonyPatch("PostStart", MethodType.Normal)]
    public class PostFix_MentalBreakWanderConfused_Start
    {
        [HarmonyPostfix]
        public static void ForceForgetTaleNews(MentalState_WanderConfused __instance)
        {
            Pawn subjectPawn = __instance.pawn;
            // Minimum charge of 1 forgotten
            subjectPawn.GetNewsKnowledgeTracker().ForgetRandomly(1);
            Hediff alzheimers = subjectPawn.health?.hediffSet.hediffs.Find((Hediff temp) => (temp.def == Vanilla_HediffDefOf.Alzheimers));
            if (alzheimers != null)
            {
                // Currently, we have conversion table of .2/.4/.6/.8/1 => 1/1/2/3/4
                float severity = alzheimers.Severity;
                Pawn_NewsKnowledgeTracker tracker = subjectPawn.GetNewsKnowledgeTracker();
                int amount;
                if (severity < 0.4f)
                {
                    return;
                }
                else if (severity < 0.6f)
                {
                    amount = 1;
                }
                else if (severity < 0.8f)
                {
                    amount = 2;
                }
                else
                {
                    amount = 3;
                }
                tracker.ForgetRandomly(amount + 1);
            }
            //__instance.pawn.GetNewsKnowledgeTracker().ForgetRandom();
        }
    }
    */
}
