using System;
using Verse;

namespace Desynchronized.TNDBS;

public class DefaultTaleNews : TaleNews
{
    public override float CalculateNewsImportanceForPawn(Pawn pawn, TaleNewsReference reference)
    {
        throw new NotImplementedException();
    }

    public override string GetNewsTypeName()
    {
        return "Default TaleNews";
    }

    public override bool IsValid()
    {
        return true;
    }

    internal override void SelfVerify()
    {
        // No need to self-verify; this is always valid.
    }

    public override bool PawnIsInvolved(Pawn pawn)
    {
        return false;
    }

    protected override void ConductSaveFileIO()
    {
    }

    protected override void GiveThoughtsToReceipient(Pawn recipient)
    {
        DesynchronizedMain.LogError(
            "Somebody tried to trigger a thought-giving process using a default TaleNews. Nothing was done.\n" +
            Environment.StackTrace);
    }

    public override string GetDetailsPrintout()
    {
        return "No details";
    }

    protected override void DiscardNewsDetails()
    {
        // We have nothing to discard.
    }
}