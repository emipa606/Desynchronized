using System;
using System.Collections.Generic;
using HugsLib.Utils;
using Verse;

namespace Desynchronized.Utilities;

[Obsolete("Not sure if feature will be used again.")]
public class HallOfFigures : UtilityWorldObject
{
    private HashSet<Pawn> pawnsRetained = new HashSet<Pawn>();

    public IEnumerable<Pawn> PawnsRetainedByTaleNews => pawnsRetained;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref pawnsRetained, true, "pawnsRetained", LookMode.Deep);
    }

    public void RetainPawn(Pawn deceased)
    {
        pawnsRetained.Add(deceased);
    }

    public bool ContainsPawn(Pawn target)
    {
        return pawnsRetained.Contains(target);
    }
}