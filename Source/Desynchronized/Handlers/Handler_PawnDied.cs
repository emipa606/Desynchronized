using Desynchronized.TNDBS;
using RimWorld;
using Verse;

namespace Desynchronized.Handlers
{
    public class Handler_PawnDied
    {
        /// <summary>
        ///     Handles a pawn death situation.
        /// </summary>
        public static void HandlePawnDied(Pawn victim, DamageInfo? killingBlow, Hediff culpritHediff)
        {
            GenerateAndProcessNews(victim, killingBlow, culpritHediff);
        }

        /// <summary>
        ///     Protocol updated in v1.6.3. Now also reports the hediff that is causing the death.
        /// </summary>
        /// <param name="victim"></param>
        /// <param name="dinfo"></param>
        /// <param name="culpritHediff"></param>
        private static void GenerateAndProcessNews(Pawn victim, DamageInfo? dinfo, Hediff culpritHediff)
        {
            // Generate one.
            //TaleNewsPawnDied taleNews = TaleNewsPawnDied.GenerateGenerally(victim, dinfo, culpritHediff);
            var taleNews = new TaleNewsPawnDied(victim, dinfo, culpritHediff);

            // Distribute news.
            foreach (var other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
            {
                if (other.IsNearEnough(victim))
                {
                    other.GetNewsKnowledgeTracker()?.KnowNews(taleNews);
                }
            }
        }
    }
}