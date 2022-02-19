using System;
using Desynchronized.TNDBS.Datatypes;
using Verse;

namespace Desynchronized.TNDBS;

public abstract class TaleNewsNegativeIndividual : TaleNews
{
    protected InstigationInfo instigatorInfo;
    private Pawn primaryVictim;

    public TaleNewsNegativeIndividual()
    {
    }

    public TaleNewsNegativeIndividual(Pawn victim, InstigationInfo instigInfo) : base(
        new LocationInfo(victim.MapHeld, victim.PositionHeld))
    {
        primaryVictim = victim;
        InstigationDetails = instigInfo;
    }

    public Pawn PrimaryVictim => primaryVictim;

    public InstigationInfo InstigationDetails
    {
        get => instigatorInfo;
        protected set => instigatorInfo = value;
    }

    /// <summary>
    ///     The (primary) instigator of this negative tale-news, if there exists one.
    ///     <para />
    ///     Null-safe.
    /// </summary>
    public Pawn Instigator => InstigationDetails?.InstigatingPawn;

    [Obsolete("Experimental tech.", true)]
    public static TaleNewsNegativeIndividual GenerateTaleNewsNegativeIndividual(TaleNewsTypeEnum typeEnum,
        Pawn primaryVictim, InstigationInfo instigatorInfo)
    {
        if (GenerateTaleNewsGenerally(typeEnum) is not TaleNewsNegativeIndividual taleNews)
        {
            return null;
        }

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
        return PrimaryVictim != null;
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
        var result = "Victim: ";
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