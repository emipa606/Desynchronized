using Desynchronized.TNDBS.Datatypes;
using RimWorld;
using Verse;

namespace Desynchronized.TNDBS;

public class TaleNewsPawnBanished : TaleNewsNegativeIndividual
{
    private bool isDeadly;

    public TaleNewsPawnBanished()
    {
    }

    public TaleNewsPawnBanished(Pawn victim, bool isBanishedToDie) : base(victim, InstigationInfo.NoInstigator)
    {
        isDeadly = isBanishedToDie;
    }

    public Pawn BanishmentVictim => PrimaryVictim;

    public bool IsDeadly => isDeadly;

    public override string GetNewsTypeName()
    {
        return "Pawn Banished";
    }

    protected override void ConductSaveFileIO()
    {
        base.ConductSaveFileIO();
        Scribe_Values.Look(ref isDeadly, "isDeadly");
    }

    protected override void GiveThoughtsToReceipient(Pawn recipient)
    {
        if (!recipient.IsCapableOfThought())
        {
            return;
        }

        // Switch for handling Bonded Animal Banished
        if (BanishmentVictim.RaceProps.Animal)
        {
            if (recipient.relations.GetDirectRelation(PawnRelationDefOf.Bond, BanishmentVictim) != null)
            {
                new IndividualThoughtToAdd(ThoughtDefOf.BondedAnimalBanished, recipient, BanishmentVictim).Add();
            }

            return;
        }

        if (recipient == BanishmentVictim)
        {
            // We have potential here. Next version, perhaps.
            return;
        }

        ThoughtDef thoughtDefToGain;
        if (!BanishmentVictim.IsPrisonerOfColony)
        {
            thoughtDefToGain = IsDeadly ? ThoughtDefOf.ColonistBanishedToDie : ThoughtDefOf.ColonistBanished;
        }
        else
        {
            if (IsDeadly)
            {
                thoughtDefToGain = ThoughtDefOf.PrisonerBanishedToDie;
            }
            else
            {
                // Adjust for traits concerning prisoner released dangerously.
                // Bloodlust trait has higher priority.
                if (recipient.story.traits.HasTrait(TraitDefOf.Bloodlust))
                {
                    thoughtDefToGain = Desynchronized_ThoughtDefOf.PrisonerReleasedDangerously_Bloodlust;
                }
                else if (recipient.story.traits.HasTrait(TraitDefOf.Psychopath))
                {
                    thoughtDefToGain = Desynchronized_ThoughtDefOf.PrisonerReleasedDangerously_Psychopath;
                }
                else
                {
                    thoughtDefToGain = Desynchronized_ThoughtDefOf.PrisonerReleasedDangerously;
                }
            }
        }

        if (ThoughtUtility.CanGetThought(recipient, thoughtDefToGain, true))
        {
            recipient.needs.mood.thoughts.memories.TryGainMemory(thoughtDefToGain, BanishmentVictim);
        }
    }

    public override float CalculateNewsImportanceForPawn(Pawn pawn, TaleNewsReference reference)
    {
        // Placeholder
        return 3;
    }

    public override string GetDetailsPrintout()
    {
        var basic = base.GetDetailsPrintout();
        basic += $"\nDeadly? {isDeadly}";
        return basic;
    }
}