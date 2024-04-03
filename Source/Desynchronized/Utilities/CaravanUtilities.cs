using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace Desynchronized.Utilities;

public class CaravanUtilities
{
    public static void ManipulateInteractionTargetsList(List<Pawn> original, Pawn self)
    {
        var caravan = self.GetCaravan();
        if (caravan == null)
        {
            // No caravan, no modification needed
            return;
        }

        DesynchronizedMain.LogWarning($"Hi hi! Processing {self.Name}");
        var possibleCandidates = caravan.PawnsListForReading;
        var selfFaction = self.Faction;
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