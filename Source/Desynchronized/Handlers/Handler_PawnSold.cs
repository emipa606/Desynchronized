using Desynchronized.TNDBS;
using Desynchronized.TNDBS.Datatypes;
using RimWorld;
using Verse;

namespace Desynchronized.Handlers
{
    public class Handler_PawnSold
    {
        public static void HandlePawnSold_ByTrade(Pawn victim, Pawn negotiator)
        {
            var news = new TaleNewsPawnSold(victim, (InstigationInfo)negotiator);
            SendOutNotificationLetter(victim);
            DistributeNews(victim, news, negotiator.Map);
        }

        public static void HandlePawnSold_ByGiftingViaPods(Pawn victim, Map mapOfSender)
        {
            var news = new TaleNewsPawnSold(victim);
            SendOutNotificationLetter(victim);
            DistributeNews(victim, news, mapOfSender);
        }

        private static void SendOutNotificationLetter(Pawn victim)
        {
            /*
            string letterLabel = "Kidnapped".Translate() + ": " + victim.LabelShortCap;
            string letterContent = string.Empty;
            letterContent += "PawnKidnapped".Translate(victim.LabelShort.CapitalizeFirst(), kidnapper.Faction.def.pawnsPlural, kidnapper.Faction.Name, victim.Named("PAWN"));
            */

            // It is a bit difficult; we need to determine to what extent our letters should cover.
            // Right now, I have thought that Bonded Sold and Prisoner Sold would work well.

            Find.LetterStack.ReceiveLetter("Pawn Sold", "A Pawn of your own has been sold away. Pawn name: " + victim.Name, LetterDefOf.NeutralEvent, LookTargets.Invalid, null, null);
        }

        /// <summary>
        /// A change in code structure results in this slight variation from the usual.
        /// </summary>
        /// <param name="salesNews">The TaleNews object for this sales.</param>
        /// <param name="mapOfOccurence">The Map where the sales occured/was initiated.</param>
        private static void DistributeNews(Pawn victim, TaleNewsPawnSold salesNews, Map mapOfOccurence)
        {
            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
            {
                if (other.IsNearEnough(victim))
                {
                    other.GetNewsKnowledgeTracker().KnowNews(salesNews);
                }
            }
        }
    }
}
