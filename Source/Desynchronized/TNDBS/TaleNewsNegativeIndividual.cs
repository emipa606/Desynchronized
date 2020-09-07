using Desynchronized.TNDBS.Datatypes;
using System;
using Verse;

namespace Desynchronized.TNDBS
{
    public abstract class TaleNewsNegativeIndividual : TaleNews
    {
        private Pawn primaryVictim;
        protected InstigationInfo instigatorInfo;

        public Pawn PrimaryVictim
        {
            get
            {
                return primaryVictim;
            }
        }

        public InstigationInfo InstigationDetails
        {
            get
            {
                return instigatorInfo;
            }
            protected set
            {
                instigatorInfo = value;
            }
        }

        /// <summary>
        /// The (primary) instigator of this negative tale-news, if there exists one.
        /// <para/>
        /// Null-safe.
        /// </summary>
        public Pawn Instigator => InstigationDetails?.InstigatingPawn ?? null;

        public TaleNewsNegativeIndividual()
        {

        }

        public TaleNewsNegativeIndividual(Pawn victim, InstigationInfo instigInfo): base(new LocationInfo(victim.MapHeld, victim.PositionHeld))
        {
            primaryVictim = victim;
            InstigationDetails = instigInfo;
        }

        [Obsolete("Experimental tech.", true)]
        public static TaleNewsNegativeIndividual GenerateTaleNewsNegativeIndividual(TaleNewsTypeEnum typeEnum, Pawn primaryVictim, InstigationInfo instigatorInfo)
        {
            TaleNewsNegativeIndividual taleNews = GenerateTaleNewsGenerally(typeEnum) as TaleNewsNegativeIndividual;
            taleNews.primaryVictim = primaryVictim;
            taleNews.instigatorInfo = instigatorInfo;
            return taleNews;
        }

        protected override void ConductSaveFileIO()
        {
            Scribe_References.Look(ref primaryVictim, "primaryVictim", true);
            Scribe_Deep.Look(ref instigatorInfo, "instigatorInfo");
        }

        public override bool PawnIsInvolved(Pawn pawn)
        {
            if (pawn == null)
            {
                return false;
            }
            if (pawn == PrimaryVictim || pawn == Instigator)
            {
                return true;
            }

            return false;
        }

        public override bool IsValid()
        {
            return (PrimaryVictim != null);
        }

        internal override void SelfVerify()
        {
            if (LocationOfOccurence == null)
            {
                LocationOfOccurence = LocationInfo.EmptyLocationInfo;
            }
        }

        public override string GetDetailsPrintout()
        {
            string result = "Victim: ";
            if (primaryVictim.Name != null)
            {
                result += primaryVictim.Name;
            }
            else
            {
                result += primaryVictim.ToString();
            }

            return result;
        }

        protected override void DiscardNewsDetails()
        {
            // Discard the victims, etc.
            instigatorInfo = null;
            primaryVictim = null;
        }
    }
}
