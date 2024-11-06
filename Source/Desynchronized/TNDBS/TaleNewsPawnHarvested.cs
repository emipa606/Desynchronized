using Desynchronized.TNDBS.Datatypes;
using RimWorld;
using Verse;

namespace Desynchronized.TNDBS;

public class TaleNewsPawnHarvested : TaleNewsNegativeIndividual
{
    public TaleNewsPawnHarvested()
    {
    }

    public TaleNewsPawnHarvested(Pawn victim) : base(victim, InstigationInfo.NoInstigator)
    {
    }

    public override float CalculateNewsImportanceForPawn(Pawn pawn, TaleNewsReference reference)
    {
        // Placeholder
        return 3;
    }

    public override string GetDetailsPrintout()
    {
        // IDK what to do here for now.
        var basic = base.GetDetailsPrintout();
        return basic;
    }

    public override string GetNewsTypeName()
    {
        return "Pawn Organ-Harvested";
    }

    protected override void GiveThoughtsToReceipient(Pawn recipient)
    {
        if (recipient == PrimaryVictim)
        {
            if (!ThoughtUtility.CanGetThought(recipient, ThoughtDefOf.MyOrganHarvested, true))
            {
                return;
            }

            recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.MyOrganHarvested);
            return;
        }

        // Not the same guy
        // Determine the correct thought to be given out
        if (PrimaryVictim.IsColonist)
        {
            if (!ThoughtUtility.CanGetThought(recipient, Desynchronized_ThoughtDefOf.KnowColonistOrganHarvested, true))
            {
                return;
            }

            recipient.needs.mood.thoughts.memories.TryGainMemory(Desynchronized_ThoughtDefOf
                .KnowColonistOrganHarvested);
            return;
        }

        if (PrimaryVictim.HostFaction != Faction.OfPlayer)
        {
            return;
        }

        if (!ThoughtUtility.CanGetThought(recipient, Desynchronized_ThoughtDefOf.KnowGuestOrganHarvested, true))
        {
            return;
        }

        recipient.needs.mood.thoughts.memories.TryGainMemory(Desynchronized_ThoughtDefOf.KnowGuestOrganHarvested);
    }
}