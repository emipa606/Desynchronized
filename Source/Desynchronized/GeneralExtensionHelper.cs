using Desynchronized.TNDBS;
using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Desynchronized
{
    public static class GeneralExtensionHelper
    {
        /// <summary>
        /// Extension method. Returns true if can have Thoughts. Is NullReference-safe.
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
                    if (kidnapVictim.gender == Gender.Female)
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyDaughterWasKidnapped;
                    }
                    else
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MySonWasKidnapped;
                    }
                    break;
                case "Spouse":
                    if (kidnapVictim.gender == Gender.Female)
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyWifeWasKidnapped;
                    }
                    else
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyHusbandWasKidnapped;
                    }
                    break;
                case "Fiance":
                    if (kidnapVictim.gender == Gender.Female)
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyWifeWasKidnapped;
                    }
                    else
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyHusbandWasKidnapped;
                    }
                    break;
                case "Lover":
                    resultingDef = Desynchronized_ThoughtDefOf.MyLoverWasKidnapped;
                    break;
                case "Sibling":
                    if (kidnapVictim.gender == Gender.Female)
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MySisterWasKidnapped;
                    }
                    else
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyBrotherWasKidnapped;
                    }
                    break;
                case "Grandchild":
                    resultingDef = Desynchronized_ThoughtDefOf.MyGrandchildWasKidnapped;
                    break;
                case "Parent":
                    if (kidnapVictim.gender == Gender.Female)
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyMotherWasKidnapped;
                    }
                    else
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyFatherWasKidnapped;
                    }
                    break;
                case "NephewOrNiece":
                    if (kidnapVictim.gender == Gender.Female)
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyNieceWasKidnapped;
                    }
                    else
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyNephewWasKidnapped;
                    }
                    break;
                case "HalfSibling":
                    resultingDef = Desynchronized_ThoughtDefOf.MyHalfSiblingWasKidnapped;
                    break;
                case "UncleOrAunt":
                    if (kidnapVictim.gender == Gender.Female)
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyAuntWasKidnapped;
                    }
                    else
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyUncleWasKidnapped;
                    }
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
                default:
                    break;
            }
            return resultingDef;
        }

        /// <summary>
        /// Extension method. Copied from vanilla code because vanilla code does not allow usage of this method.
        /// <para/>
        /// Already includes checking whether both pawns are in the same map.
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
            if (!subject.Position.InHorDistOf(other.Position, 12f))
            {
                return false;
            }
            if (!GenSight.LineOfSight(other.Position, subject.Position, other.Map))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Extension method. Determines and returns the chance of the provided pawn to spread TaleNews.
        /// <para/>
        /// Invalid pawns (e.g. animals) will return 0.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static float GetBaseNewsSpreadChance(this Pawn instance)
        {
            return instance.GetStatValue(Desynchronized_StatDefOf.NewsSpreadTendency);
        }

        /// <summary>
        /// Extension method. This method calculates the cumulative chance
        /// for news-spreading as if the news-spreading check is done multiple times
        /// <para/>
        /// As an analogy, it is as if you rolled n dices, and you are looking for the
        /// probability that any one of them has a 3 facing up.
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
            List<Pawn_NewsKnowledgeTracker> masterList = DesynchronizedMain.TaleNewsDatabaseSystem.KnowledgeTrackerMasterList;
            foreach (Pawn_NewsKnowledgeTracker tracker in masterList)
            {
                if (tracker.Pawn == instance)
                {
                    return tracker;
                }
            }

            Pawn_NewsKnowledgeTracker newTracker = Pawn_NewsKnowledgeTracker.GenerateNewTrackerForPawn(instance);
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

        public static bool IsInSameMapOrCaravan(this Pawn subject, Pawn other)
        {
            if (subject.MapHeld != null)
            {
                return other.MapHeld == subject.MapHeld;
            }
            if (subject.GetCaravan() != null)
            {
                return other.GetCaravan() == subject.GetCaravan();
            }
            return false;
        }
    }
}
