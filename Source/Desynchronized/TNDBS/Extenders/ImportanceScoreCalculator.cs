using RimWorld;
using Verse;

namespace Desynchronized.TNDBS.Extenders;

public static class ImportanceScoreCalculator
{
    /// <summary>
    ///     Extension method. A Social Proxomity Score is given for the other pawn. The "socially closer" the other pawn is
    ///     w.r.t self (e.g. close family), the higher the score.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static int GetSocialProximityScoreForOther(this Pawn subject, Pawn other)
    {
        if (subject == null)
        {
            return 0;
        }

        var relation = subject.GetMostImportantRelation(other);

        // Score is given by the "differential pattern" of subject.
        if (relation == PawnRelationDefOf.Lover || relation == PawnRelationDefOf.Fiance ||
            relation == PawnRelationDefOf.Spouse)
        {
            return 5;
        }

        if (relation == PawnRelationDefOf.Parent || relation == PawnRelationDefOf.Child ||
            relation == PawnRelationDefOf.Bond)
        {
            return 4;
        }

        if (relation == PawnRelationDefOf.Sibling || relation == PawnRelationDefOf.HalfSibling ||
            relation == PawnRelationDefOf.Cousin)
        {
            return 4;
        }

        if (relation == PawnRelationDefOf.Grandparent || relation == PawnRelationDefOf.Grandchild)
        {
            return 3;
        }

        if (relation == PawnRelationDefOf.UncleOrAunt || relation == PawnRelationDefOf.NephewOrNiece)
        {
            return 3;
        }

        if (relation == PawnRelationDefOf.ExLover || relation == PawnRelationDefOf.ExSpouse)
        {
            return 3;
        }

        // Determine score by factions
        var subjectFaction = subject.Faction;
        var otherFaction = other.Faction;
        if (subjectFaction != null)
        {
            if (subjectFaction == otherFaction)
            {
                return 2;
            }

            return subjectFaction.AlliedTo(otherFaction) ? 1 : 0;
        }

        if (otherFaction == null)
        {
            return 0;
        }

        return otherFaction.AlliedTo(null) ? 1 : 0;
    }
}