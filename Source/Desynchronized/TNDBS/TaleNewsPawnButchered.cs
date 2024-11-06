using Desynchronized.Patches;
using Desynchronized.TNDBS.Datatypes;
using RimWorld;
using Verse;

namespace Desynchronized.TNDBS;

public class TaleNewsPawnButchered : TaleNewsNegativeIndividual
{
    public TaleNewsPawnButchered()
    {
    }

    public TaleNewsPawnButchered(Pawn victim) : base(victim, InstigationInfo.NoInstigator)
    {
    }

    public override float CalculateNewsImportanceForPawn(Pawn pawn, TaleNewsReference reference)
    {
        // Placeholder
        return 6;
    }

    public override string GetDetailsPrintout()
    {
        // IDK what to do here for now.
        var basic = base.GetDetailsPrintout();
        return basic;
    }

    public override string GetNewsTypeName()
    {
        return "Pawn Butchered";
    }

    protected override void GiveThoughtsToReceipient(Pawn recipient)
    {
        if (recipient == null || recipient.Dead)
        {
            return;
        }

        if (!ThoughtUtility.CanGetThought(recipient, Desynchronized_ThoughtDefOf.KnowButcheredHumanlikeCorpse, true))
        {
            return;
        }

        PreFix_Corpse_ButcherProducts.overrideValue = true;
        recipient.needs.mood.thoughts.memories.TryGainMemory(Desynchronized_ThoughtDefOf.KnowButcheredHumanlikeCorpse);
        PreFix_Corpse_ButcherProducts.overrideValue = false;
    }
}