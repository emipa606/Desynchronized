using RimWorld;
using Verse;

namespace Desynchronized.TNDBS.Datatypes
{
    public class InstigationInfo: IExposable
    {
        /// <summary>
        /// Our patch-mod should override this with Psychology mayor. Value can be null.
        /// </summary>
        /// <returns></returns>
        public static Pawn GetRepresentativeOfPlayerInGame()
        {
            return Faction.OfPlayer.leader;
        }

        public static Pawn RepresentativeOfPlayer => GetRepresentativeOfPlayerInGame();

        public static InstigationInfo NoInstigator => new InstigationInfo();

        private bool playerIsInstigator;
        private Pawn instigator;
        private Faction instigatingFaction;

        public bool PlayerIsInstigator
        {
            get => playerIsInstigator;
            private set => playerIsInstigator = value;
        }

        /// <summary>
        /// The instigator. Can be null.
        /// </summary>
        public Pawn InstigatingPawn
        {
            get => instigator;
            internal set => instigator = value;
        }

        public Faction InstigatingFaction
        {
            get => instigatingFaction;
            internal set => instigatingFaction = value;
        }

        /// <summary>
        /// Do not use this constructor explicitly.
        /// </summary>
        public InstigationInfo()
        {

        }

        public InstigationInfo(Faction instigFaction = null, Pawn instigPawn = null)
        {
            instigatingFaction = instigFaction;
            instigator = instigPawn;
            playerIsInstigator = false;
        }

        public static InstigationInfo GenerateAsPlayerInstigated()
        {
            return new InstigationInfo()
            {
                PlayerIsInstigator = true,
                InstigatingPawn = null
            };
        }

        public void ExposeData()
        {
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                // Do some init here
                if (instigatingFaction == null)
                {
                    instigatingFaction = instigator?.Faction ?? null;
                }
            }

            Scribe_Values.Look(ref playerIsInstigator, "playerIsInstigator", false);
            Scribe_References.Look(ref instigator, "instigator");
            Scribe_References.Look(ref instigatingFaction, "instigatingFaction");

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                // Do some init here
                if (instigatingFaction == null)
                {
                    instigatingFaction = instigator?.Faction ?? null;
                }
            }
        }

        public static explicit operator InstigationInfo(Faction faction)
        {
            return new InstigationInfo(faction);
        }

        public static explicit operator InstigationInfo(Pawn pawn)
        {
            return new InstigationInfo(pawn?.Faction, pawn);
        }
    }
}
