using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Handlers
{
    public class HandlerUtility
    {
        /// <summary>
        /// If subject is in an active map, returns all Colonists in the map. If subject is in a caravan, return all Colonist caravan members.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Pawn> GetNewsCandidatesForNewsInvolvingPawn_ColonistsOnly(Pawn subject)
        {
            if (subject.MapHeld != null)
            {
                // In map
                Map mapOfOccurence = subject.MapHeld;
                
                foreach (Pawn potential in PawnsFinder.AllMapsWorldAndTemporary_Alive)
                {
                    if (potential.IsColonist && potential.MapHeld == mapOfOccurence)
                    {
                        yield return potential;
                    }
                }
            }
            else if (subject.IsCaravanMember())
            {
                // In caravan
                Caravan caravan = subject.GetCaravan();
                
                foreach (Pawn potential in PawnsFinder.AllCaravansAndTravelingTransportPods_Alive)
                {
                    if (potential.IsColonist && potential.GetCaravan() == caravan)
                    {
                        yield return potential;
                    }
                }
            }
            else
            {
                // IDK now.
            }

            yield break;
        }

        public static IEnumerable<Pawn> GetNewsCandidatesForNewsInvolvingPawn_ColonistsAndPrisoners(Pawn subject)
        {
            foreach (Pawn pawn in GetNewsCandidatesForNewsInvolvingPawn_ColonistsOnly(subject))
            {
                yield return pawn;
            }

            if (subject.MapHeld != null)
            {
                // In map
                Map mapOfOccurence = subject.MapHeld;

                foreach (Pawn potential in PawnsFinder.AllMapsWorldAndTemporary_Alive)
                {
                    if (potential.IsPrisonerOfColony && potential.MapHeld == mapOfOccurence)
                    {
                        yield return potential;
                    }
                }
            }
            else if (subject.IsCaravanMember())
            {
                // In caravan
                Caravan caravan = subject.GetCaravan();

                foreach (Pawn potential in PawnsFinder.AllCaravansAndTravelingTransportPods_Alive)
                {
                    if (potential.IsPrisonerOfColony && potential.GetCaravan() == caravan)
                    {
                        yield return potential;
                    }
                }
            }
            else
            {
                // IDK now.
            }

            yield break;
        }
    }
}
