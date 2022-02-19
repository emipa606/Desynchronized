using RimWorld;

namespace Desynchronized.Utilities;

public static class FactionUtilities
{
    /// <summary>
    ///     Returns the goodwill between one faction and another. Guaranteed error-free.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static int ObtainFactionGoodwillSafely(Faction subject, Faction other)
    {
        if (subject == other)
        {
            if (subject == null)
            {
                // Both are null
                return 0;
            }

            // Both are non-null
            return 100;
        }

        if (subject == null || other == null)
        {
            // One of them is null
            return 0;
        }

        // Both of them are non-null
        return subject.RelationWith(other).baseGoodwill;
    }

    /// <summary>
    ///     Returns the goodwill between this faction and the given other faction. Guaranteed error-free.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static int GetGoodwillWith(this Faction subject, Faction other)
    {
        return ObtainFactionGoodwillSafely(subject, other);
    }
}