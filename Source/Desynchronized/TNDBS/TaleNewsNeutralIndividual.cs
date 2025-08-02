using System;
using Verse;

namespace Desynchronized.TNDBS;

public abstract class TaleNewsNeutralIndividual : TaleNews
{
    private Pawn receiver;

    public TaleNewsNeutralIndividual()
    {
    }

    protected TaleNewsNeutralIndividual(Pawn receiver) : base(null)
    {
        this.receiver = receiver;
    }

    public Pawn Receiver => receiver;

    protected override void ConductSaveFileIO()
    {
        Scribe_References.Look(ref receiver, "receiver");
        throw new NotImplementedException();
    }
}