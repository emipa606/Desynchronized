using Desynchronized.TNDBS.Datatypes;
using RimWorld;
using Verse;

namespace Desynchronized.TNDBS
{
    public class TaleNewsPawnSold : TaleNewsNegativeIndividual
    {
        public Faction tradeDeal_OtherParty;

        public TaleNewsPawnSold()
        {

        }

        public TaleNewsPawnSold(Pawn victim): this(victim, InstigationInfo.NoInstigator)
        {

        }

        public TaleNewsPawnSold(Pawn victim, InstigationInfo info): base (victim, info)
        {

        }

        public override float CalculateNewsImportanceForPawn(Pawn pawn, TaleNewsReference reference)
        {
            // Placeholder
            return 3;
        }

        public override string GetDetailsPrintout()
        {
            var basic = base.GetDetailsPrintout();
            basic += "\nSold to: ";
            if (tradeDeal_OtherParty != null)
            {
                basic += tradeDeal_OtherParty.Name;
            }
            else
            {
                basic += "unknown group";
            }
            return basic;
        }

        public override string GetNewsTypeName()
        {
            return "Pawn Sold";
        }

        protected override void ConductSaveFileIO()
        {
            base.ConductSaveFileIO();

            Scribe_References.Look(ref tradeDeal_OtherParty, "tradeDeal_OtherParty");
            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                if (tradeDeal_OtherParty == null)
                {
                    tradeDeal_OtherParty = PrimaryVictim?.Faction ?? PrimaryVictim?.HostFaction ?? null;
                }
            }
        }

        protected override void GiveThoughtsToReceipient(Pawn recipient)
        {
            if (!recipient.IsCapableOfThought())
            {
                return;
            }

            if (recipient == PrimaryVictim)
            {
                // I was sold
            }
            else
            {
                // Animal or Prisoner sold
                if (PrimaryVictim.RaceProps.Animal)
                {
                    // Vanilla v1.1, there are new features here
                    Pawn firstDirectRelationPawn = PrimaryVictim.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond);
                    if (firstDirectRelationPawn != null && firstDirectRelationPawn == recipient && firstDirectRelationPawn.needs.mood != null)
                    {
                        PrimaryVictim.relations.RemoveDirectRelation(PawnRelationDefOf.Bond, recipient);
                        firstDirectRelationPawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SoldMyBondedAnimalMood);
                    }
                }
                else if (PrimaryVictim.RaceProps.Humanlike)
                {
                    // Some prisoner was sold
                    if (PrimaryVictim.IsPrisonerOfColony)
                    {
                        recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowPrisonerSold, Instigator);
                    }
                }

                // Relationship was sold, if there is any
                PawnRelationDef relation = recipient.GetMostImportantRelation(PrimaryVictim);
                if (relation != null)
                {
                    foreach (ThoughtDef soldThought in relation.soldThoughts)
                    {
                        recipient.needs.mood.thoughts.memories.TryGainMemory(soldThought, Instigator);
                    }
                }

                // Remove marriage-related memories, etc.
                Pawn spouse = PrimaryVictim.GetSpouse();
                if (spouse != null && recipient == spouse && !recipient.Dead)
                {
                    MemoryThoughtHandler memories = recipient.needs.mood.thoughts.memories;
                    memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
                    memories.RemoveMemoriesOfDef(ThoughtDefOf.HoneymoonPhase);
                }
            }
        }
    }
}
