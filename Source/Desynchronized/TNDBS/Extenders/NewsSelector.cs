using System.Collections.Generic;
using System.Linq;

namespace Desynchronized.TNDBS.Extenders
{
    public static class NewsSelector
    {
        /// <summary>
        ///     Selects all news of this pawn that this pawn remembers. Returns a List[TaleNewsReference].
        ///     <para />
        ///     Performance advisory: This should be slightly faster than the TaleNews variant.
        /// </summary>
        /// <param name="tracker"></param>
        /// <returns></returns>
        public static IEnumerable<TaleNewsReference> GetAllValidNonForgottenNewsReferences(
            this Pawn_NewsKnowledgeTracker tracker)
        {
            if (tracker == null)
            {
                return Enumerable.Empty<TaleNewsReference>();
            }

            // EMBRACE THE POWER OF LINQ; LINQ PROTECTS
            return new List<TaleNewsReference>(tracker.AllValidNewsReferences).FindAll(reference =>
                !reference.NewsIsLocallyForgotten);
        }

        /// <summary>
        ///     Selects all news that this pawn remembers. Returns a List[TaleNews].
        ///     <para />
        ///     Performance advisory: This should be slightly slower than the TaleNewsReference variant.
        /// </summary>
        /// <param name="tracker"></param>
        /// <returns></returns>
        public static IEnumerable<TaleNews> GetAllValidNonForgottenNews(this Pawn_NewsKnowledgeTracker tracker)
        {
            if (tracker == null)
            {
                return Enumerable.Empty<TaleNews>();
            }

            // EMBRACE THE POWER OF LINQ; LINQ PROTECTS
            return GetAllValidNonForgottenNewsReferences(tracker).Select(reference => reference.ReferencedTaleNews);
        }

        /// <summary>
        ///     Selects all news that this pawn has forgotten. Returns a List[TaleNewsReference].
        /// </summary>
        /// <param name="tracker"></param>
        /// <returns></returns>
        public static IEnumerable<TaleNewsReference> GetAllForgottenNewsReferences(
            this Pawn_NewsKnowledgeTracker tracker)
        {
            if (tracker == null)
            {
                return Enumerable.Empty<TaleNewsReference>();
            }

            // EMBRACE THE POWER OF LINQ; LINQ PROTECTS
            return tracker.AllNewsReferences_ReadOnlyList.FindAll(reference => reference.NewsIsLocallyForgotten);
        }

        /// <summary>
        ///     Selects all news that are remembered by at least one pawn. Returns a List[TaleNews].
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public static IEnumerable<TaleNews> GetAllValidNonPermForgottenNews(this TaleNewsDatabase database)
        {
            if (database == null)
            {
                // This is highly unlikely, but be prepared.
                return Enumerable.Empty<TaleNews>();
            }

            return database.ListOfAllTaleNews.FindAll(news => news.IsValid() && !news.PermanentlyForgotten);
        }
    }
}