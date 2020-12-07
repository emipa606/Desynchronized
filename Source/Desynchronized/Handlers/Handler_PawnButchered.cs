using Desynchronized.TNDBS;
using Verse;

namespace Desynchronized.Handlers
{
    public class Handler_PawnButchered
    {
        /// <summary>
        /// Handles a pawn butchered event.
        /// </summary>
        /// <param name="listener"></param>
        public static void HandlePawnButchered(Pawn listener)
        {
            GenerateAndProcessNews(listener);
        }

        private static void GenerateAndProcessNews(Pawn listener)
        {
            if (listener.IsNearEnough())
            {
                listener.GetNewsKnowledgeTracker().KnowNews(Patches.PreFix_Corpse_ButcherProducts.corpseNews);
            }
        }
    }
}
