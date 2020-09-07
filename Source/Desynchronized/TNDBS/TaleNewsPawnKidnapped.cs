using Desynchronized.TNDBS.Datatypes;
using RimWorld;
using System;
using Verse;

namespace Desynchronized.TNDBS
{
    [Obsolete("Use [] instead.")]
    public class TaleNewsPawnKidnapped : TaleNewsNegativeIndividual
    {
        private Faction kidnapperFaction;

        public Pawn Kidnapper => Instigator;

        public Pawn KidnapVictim => PrimaryVictim;

        public Faction KidnapperFaction
        {
            get
            {
                return kidnapperFaction;
            }
        }

        public TaleNewsPawnKidnapped()
        {

        }

        public TaleNewsPawnKidnapped(Pawn victim, Faction kidnappingFaction): base(victim, InstigationInfo.NoInstigator)
        {
            if (kidnappingFaction == null)
            {
                DesynchronizedMain.LogError("Kidnapping faction cannot be null! Fake news!\n" + Environment.StackTrace);
            }
            else
            {
                InstigationDetails = (InstigationInfo) kidnappingFaction;
                kidnapperFaction = kidnappingFaction;
            }
        }

        public TaleNewsPawnKidnapped(Pawn victim, Pawn kidnapper): base(victim, InstigationInfo.NoInstigator)
        {
            if (kidnapper == null)
            {
                DesynchronizedMain.LogError("Kidnapper cannot be null! Fake News!\n" + Environment.StackTrace);
            }
            else
            {
                InstigationDetails = (InstigationInfo) kidnapper;
                kidnapperFaction = kidnapper.Faction;
            }
        }

        public override string GetNewsTypeName()
        {
            return "Pawn Kidnapped";
        }

        protected override void ConductSaveFileIO()
        {
            base.ConductSaveFileIO();
            Scribe_References.Look(ref kidnapperFaction, "kidnapperFaction");
        }

        protected override void GiveThoughtsToReceipient(Pawn recipient)
        {
            // Check if the receipient can receive any thoughts at all.
            // No need to check for victim's raceprops; only Colonists can be targetted to be kidnapped.
            if (!recipient.IsCapableOfThought())
            {
                return;
            }

            // Change to vanilla "pawn lost" thoughts

            // Give generic Colonist Kidnapped thoughts
            recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ColonistLost);

            // Then give Friend/Rival Kidnapped thoughts
            if (recipient.RaceProps.IsFlesh && PawnUtility.ShouldGetThoughtAbout(KidnapVictim, recipient))
            {
                int opinion = recipient.relations.OpinionOf(KidnapVictim);
                if (opinion >= 20)
                {
                    new IndividualThoughtToAdd(ThoughtDefOf.PawnWithGoodOpinionLost, recipient, KidnapVictim, KidnapVictim.relations.GetFriendDiedThoughtPowerFactor(opinion), 1f).Add();
                }
                else if (opinion <= -20)
                {
                    new IndividualThoughtToAdd(ThoughtDefOf.PawnWithBadOpinionLost, recipient, KidnapVictim, KidnapVictim.relations.GetRivalDiedThoughtPowerFactor(opinion), 1f).Add();
                }
            }

            // Finally give Family Member Kidnapped thoughts
            PawnRelationDef mostImportantRelation = recipient.GetMostImportantRelation(KidnapVictim);
            if (mostImportantRelation != null)
            {
                ThoughtDef genderSpecificLostThought = mostImportantRelation.GetGenderSpecificLostThought(KidnapVictim);
                if (genderSpecificLostThought != null)
                {
                    new IndividualThoughtToAdd(genderSpecificLostThought, recipient, KidnapVictim).Add();
                    // outIndividualThoughts.Add(new IndividualThoughtToAdd(genderSpecificDiedThought, potentiallyRelatedPawn, victim, 1f, 1f));
                }
            }
            // TODO
        }

        public override float CalculateNewsImportanceForPawn(Pawn pawn, TaleNewsReference reference)
        {
            // Placeholder
            return 3;
        }

        public override string GetDetailsPrintout()
        {
            string basic = base.GetDetailsPrintout();
            basic += "\nKidnapped by faction: ";
            if (kidnapperFaction != null)
            {
                basic += kidnapperFaction.Name;
            }
            else
            {
                basic += "unknown";
            }
            basic += "\nActual kidnapper: ";
            if (Kidnapper != null)
            {
                basic += Kidnapper.Name;
            }
            else
            {
                basic += "unknown";
            }

            return basic;
        }
    }
}
