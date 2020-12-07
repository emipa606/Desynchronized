using Desynchronized.TNDBS;
using RimWorld;
using Verse;

namespace Desynchronized.Handlers
{
    public class Handler_PawnBanished
    {
        /// <summary>
        /// Handles a pawn banishment event.
        /// </summary>
        /// <param name="victim"></param>
        /// <param name="banishmentIsDeadly"></param>
        public static void HandlePawnBanished(Pawn victim, bool banishmentIsDeadly)
        {
            SendOutNotificationLetter(victim);
            GenerateAndProcessNews(victim, banishmentIsDeadly);
        }

        private static void SendOutNotificationLetter(Pawn victim)
        {
            Find.LetterStack.ReceiveLetter("Colonist banished", "Colonist banished. Name of Colonist: " + victim.Name, LetterDefOf.NegativeEvent, victim, null, null);
        }

        private static void GenerateAndProcessNews(Pawn victim, bool banishmentIsDeadly)
        {
            var banishmentNews = new TaleNewsPawnBanished(victim, banishmentIsDeadly);

            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
            {
                if (other.IsNearEnough(victim))
                {
                    other.GetNewsKnowledgeTracker().KnowNews(banishmentNews);
                }
            }
        }
    }
}
