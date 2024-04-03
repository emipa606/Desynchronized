using Desynchronized.Patches;
using Desynchronized.TNDBS;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace Desynchronized;

public static class GeneralExtensionHelper
{
    private static readonly float maximumRange = 30f;

    /// <summary>
    ///     Extension method. Returns true if you can have Thoughts. Is NullReference-safe.
    /// </summary>
    /// <param name="pawn"></param>
    /// <returns></returns>
    public static bool IsCapableOfThought(this Pawn pawn)
    {
        // Simplified from the glamorous usage of a try-catch block.
        return pawn?.needs?.mood?.thoughts != null;
    }

    public static ThoughtDef GetGenderSpecificKidnappedThought(this PawnRelationDef relation, Pawn kidnapVictim)
    {
        ThoughtDef resultingDef = null;
        switch (relation.defName)
        {
            case "Child":
                resultingDef = kidnapVictim.gender == Gender.Female
                    ? Desynchronized_ThoughtDefOf.MyDaughterWasKidnapped
                    : Desynchronized_ThoughtDefOf.MySonWasKidnapped;

                break;
            case "Spouse":
                resultingDef = kidnapVictim.gender == Gender.Female
                    ? Desynchronized_ThoughtDefOf.MyWifeWasKidnapped
                    : Desynchronized_ThoughtDefOf.MyHusbandWasKidnapped;

                break;
            case "Fiance":
                resultingDef = kidnapVictim.gender == Gender.Female
                    ? Desynchronized_ThoughtDefOf.MyWifeWasKidnapped
                    : Desynchronized_ThoughtDefOf.MyHusbandWasKidnapped;

                break;
            case "Lover":
                resultingDef = Desynchronized_ThoughtDefOf.MyLoverWasKidnapped;
                break;
            case "Sibling":
                resultingDef = kidnapVictim.gender == Gender.Female
                    ? Desynchronized_ThoughtDefOf.MySisterWasKidnapped
                    : Desynchronized_ThoughtDefOf.MyBrotherWasKidnapped;

                break;
            case "Grandchild":
                resultingDef = Desynchronized_ThoughtDefOf.MyGrandchildWasKidnapped;
                break;
            case "Parent":
                resultingDef = kidnapVictim.gender == Gender.Female
                    ? Desynchronized_ThoughtDefOf.MyMotherWasKidnapped
                    : Desynchronized_ThoughtDefOf.MyFatherWasKidnapped;

                break;
            case "NephewOrNiece":
                resultingDef = kidnapVictim.gender == Gender.Female
                    ? Desynchronized_ThoughtDefOf.MyNieceWasKidnapped
                    : Desynchronized_ThoughtDefOf.MyNephewWasKidnapped;

                break;
            case "HalfSibling":
                resultingDef = Desynchronized_ThoughtDefOf.MyHalfSiblingWasKidnapped;
                break;
            case "UncleOrAunt":
                resultingDef = kidnapVictim.gender == Gender.Female
                    ? Desynchronized_ThoughtDefOf.MyAuntWasKidnapped
                    : Desynchronized_ThoughtDefOf.MyUncleWasKidnapped;

                break;
            case "Grandparent":
                resultingDef = Desynchronized_ThoughtDefOf.MyGrandparentWasKidnapped;
                break;
            case "Cousin":
                resultingDef = Desynchronized_ThoughtDefOf.MyCousinWasKidnapped;
                break;
            case "Kin":
                resultingDef = Desynchronized_ThoughtDefOf.MyKinWasKidnapped;
                break;
        }

        return resultingDef;
    }

    /// <summary>
    ///     Extension method. Copied from vanilla code because vanilla code does not allow usage of this method.
    ///     <para />
    ///     Already includes checking whether both pawns are in the same map.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool CanWitnessOtherPawn(this Pawn subject, Pawn other)
    {
        if (!subject.Awake() || !subject.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
        {
            return false;
        }

        if (other.IsCaravanMember())
        {
            return other.GetCaravan() == subject.GetCaravan();
        }

        if (!other.Spawned || !subject.Spawned)
        {
            return false;
        }

        return subject.Position.InHorDistOf(other.Position, 12f) &&
               GenSight.LineOfSight(other.Position, subject.Position, other.Map);
    }

    /// <summary>
    ///     Extension method. Determines and returns the chance of the provided pawn to spread TaleNews.
    ///     <para />
    ///     Invalid pawns (e.g. animals) will return 0.
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static float GetBaseNewsSpreadChance(this Pawn instance)
    {
        return instance.GetStatValue(Desynchronized_StatDefOf.NewsSpreadTendency);
    }

    /// <summary>
    ///     Extension method. This method calculates the cumulative chance
    ///     for news-spreading as if the news-spreading check is done multiple times
    ///     <para />
    ///     As an analogy, it is as if you rolled n dices, and you are looking for the
    ///     probability that any one of them has a 3 facing up.
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="iterations"></param>
    /// <returns></returns>
    public static float GetActualNewsSpreadChance(this Pawn instance, uint iterations = 1)
    {
        /*
         * The geometric sequence reduction formula is used to optimize performance when
         * given a sufficiently-high iterations parameter.
         * And suprisingly, the resulting formula is quite simple.
         */
        return 1 - Mathf.Pow(1 - GetBaseNewsSpreadChance(instance), iterations);
    }

    public static Pawn_NewsKnowledgeTracker GetNewsKnowledgeTracker(this Pawn instance)
    {
        if (instance == null)
        {
            return null;
        }

        var masterList = DesynchronizedMain.TaleNewsDatabaseSystem.KnowledgeTrackerMasterList;
        foreach (var tracker in masterList)
        {
            if (tracker.Pawn == instance)
            {
                return tracker;
            }
        }

        var newTracker = Pawn_NewsKnowledgeTracker.GenerateNewTrackerForPawn(instance);
        DesynchronizedMain.TaleNewsDatabaseSystem.KnowledgeTrackerMasterList.Add(newTracker);
        return newTracker;
    }

    public static bool AlliedTo(this Faction self, Faction other)
    {
        if (self == null || other == null || other == self)
        {
            // Same faction is more than "allied", hence not "allied"
            return false;
        }

        return self.RelationKindWith(other) == FactionRelationKind.Ally;
    }

    public static bool IsNearEnough(this Pawn subject, Pawn other = null)
    {
        var subjectMap = subject.MapHeld;
        Map targetMap;
        var subjectRoom = subject.GetRoom();
        Room targetRoom;
        var subjectPosition = subject.PositionHeld;
        IntVec3 targetPosition;
        if (other != null)
        {
            // Same caravan
            if (subject.GetCaravan() != null && subject.GetCaravan() == other.GetCaravan())
            {
                DesynchronizedMain.LogInfo("Is in the same caravan");
                return true;
            }

            targetMap = other.MapHeld;
            targetRoom = other.GetRoom();
            targetPosition = other.PositionHeld;
        }
        else
        {
            targetMap = PreFix_Corpse_ButcherProducts.corpseMap;
            targetRoom = PreFix_Corpse_ButcherProducts.corpseRoom;
            targetPosition = PreFix_Corpse_ButcherProducts.corpseLocation;
        }

        // Different maps
        if (subjectMap == null || subjectMap != targetMap)
        {
            DesynchronizedMain.LogInfo("Is in different maps");
            return false;
        }

        // Return true if not checking for room and range
        if (!DesynchronizedMain.NewsBehaviour_OnlySpreadInSameRoom)
        {
            return true;
        }

        // Different rooms
        if (subjectRoom != targetRoom)
        {
            DesynchronizedMain.LogInfo("Is in different rooms");
            return false;
        }

        // Outside but too far apart
        if (subjectRoom.PsychologicallyOutdoors && (subjectPosition.DistanceTo(targetPosition) > maximumRange ||
                                                    !subject.Awake() || !GenSight.LineOfSight(subjectPosition,
                                                        targetPosition, subject.Map, true)))
        {
            DesynchronizedMain.LogInfo(
                $"{subjectPosition.DistanceTo(targetPosition)} is larger than max: {maximumRange} or {subject.NameShortColored} cannot see corpse");
            return false;
        }

        DesynchronizedMain.LogInfo(
            $"{subject.NameShortColored} is close enough to hear about news. Same room/caravan or {subjectPosition.DistanceTo(targetPosition)} is less than {maximumRange}");
        return true;
    }
}