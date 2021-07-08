using System;
using Verse;

namespace Desynchronized.TNDBS
{
    public class TaleNewsColonyEstablished : TaleNewsNeutralIndividual
    {
        public override float CalculateNewsImportanceForPawn(Pawn pawn, TaleNewsReference refernce)
        {
            throw new NotImplementedException();
        }

        public override string GetDetailsPrintout()
        {
            throw new NotImplementedException();
        }

        public override string GetNewsTypeName()
        {
            return "Colony Established";
        }

        public override bool IsValid()
        {
            return true;
        }

        public override bool PawnIsInvolved(Pawn pawn)
        {
            return false;
        }

        protected override void DiscardNewsDetails()
        {
            throw new NotImplementedException();
        }

        protected override void GiveThoughtsToReceipient(Pawn recipient)
        {
            throw new NotImplementedException();
        }

        internal override void SelfVerify()
        {
            throw new NotImplementedException();
        }
    }
}