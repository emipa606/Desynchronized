using System;
using Verse;

namespace Desynchronized.TNDBS
{
    public abstract class TaleNewsNeutralIndividual: TaleNews
    {
        private Pawn receiver;

        public Pawn Receiver => receiver;

        public TaleNewsNeutralIndividual()
        {

        }

        public TaleNewsNeutralIndividual(Pawn receiver): base (null)
        {
            this.receiver = receiver;
        }

        protected override void ConductSaveFileIO()
        {
            Scribe_References.Look(ref receiver, "receiver");
            throw new NotImplementedException();
        }
    }
}
