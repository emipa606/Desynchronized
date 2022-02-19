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
            recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.MyOrganHarvested);
        }
        else
        {
            // Not the same guy
            // Determine the correct thought to be given out
            if (PrimaryVictim.IsColonist)
            {
                recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowColonistOrganHarvested);
            }
            else if (PrimaryVictim.HostFaction == Faction.OfPlayer)
            {
                recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowGuestOrganHarvested);
            }
        }
    }
}