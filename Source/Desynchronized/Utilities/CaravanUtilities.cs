using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using Verse;

namespace Desynchronized.Utilities
{
    public class CaravanUtilities
    {
        public static void ManipulateInteractionTargetsList(List<Pawn> original, Pawn self)
        {
            Caravan caravan = self.GetCaravan();
            if (caravan == null)
            {
                // No caravan, no modification needed
                return;
            }

            DesynchronizedMain.LogWarning("Hi hi! Processing " + self.Name);
            List<Pawn> possibleCandidates = caravan.PawnsListForReading;
            Faction selfFaction = self.Faction;
            for (var i = possibleCandidates.Count; i >= 0; i++)
            {
                if (possibleCandidates[i].Faction != selfFaction)
                {
                    possibleCandidates.RemoveAt(i);
                }
            }
            original.Clear();
            original.AddRange(possibleCandidates);
        }
    }
}
