﻿using System;
using Desynchronized.TNDBS.Datatypes;
using RimWorld;
using Verse;

namespace Desynchronized.TNDBS;

[Obsolete("Use [] instead.")]
public class TaleNewsPawnKidnapped : TaleNewsNegativeIndividual
{
    private Faction kidnapperFaction;

    public TaleNewsPawnKidnapped()
    {
    }

    public TaleNewsPawnKidnapped(Pawn victim, Faction kidnappingFaction) : base(victim,
        InstigationInfo.NoInstigator)
    {
        if (kidnappingFaction == null)
        {
            DesynchronizedMain.LogError($"Kidnapping faction cannot be null! Fake news!\n{Environment.StackTrace}");
        }
        else
        {
            InstigationDetails = (InstigationInfo)kidnappingFaction;
            kidnapperFaction = kidnappingFaction;
        }
    }

    public TaleNewsPawnKidnapped(Pawn victim, Pawn kidnapper) : base(victim, InstigationInfo.NoInstigator)
    {
        if (kidnapper == null)
        {
            DesynchronizedMain.LogError($"Kidnapper cannot be null! Fake News!\n{Environment.StackTrace}");
        }
        else
        {
            InstigationDetails = (InstigationInfo)kidnapper;
            kidnapperFaction = kidnapper.Faction;
        }
    }

    public Pawn Kidnapper => Instigator;

    public Pawn KidnapVictim => PrimaryVictim;

    public Faction KidnapperFaction => kidnapperFaction;

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

        if (ThoughtUtility.CanGetThought(recipient, ThoughtDefOf.ColonistLost, true))
        {
            recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ColonistLost);
        }

        // Then give Friend/Rival Kidnapped thoughts
        if (recipient.RaceProps.IsFlesh && PawnUtility.ShouldGetThoughtAbout(KidnapVictim, recipient))
        {
            var opinion = recipient.relations.OpinionOf(KidnapVictim);
            if (opinion >= 20)
            {
                new IndividualThoughtToAdd(ThoughtDefOf.PawnWithGoodOpinionLost, recipient, KidnapVictim,
                    KidnapVictim.relations.GetFriendDiedThoughtPowerFactor(opinion)).Add();
            }
            else if (opinion <= -20)
            {
                new IndividualThoughtToAdd(ThoughtDefOf.PawnWithBadOpinionLost, recipient, KidnapVictim,
                    KidnapVictim.relations.GetRivalDiedThoughtPowerFactor(opinion)).Add();
            }
        }

        // Finally give Family Member Kidnapped thoughts
        var mostImportantRelation = recipient.GetMostImportantRelation(KidnapVictim);

        var genderSpecificLostThought = mostImportantRelation?.GetGenderSpecificLostThought(KidnapVictim);
        if (genderSpecificLostThought != null)
        {
            new IndividualThoughtToAdd(genderSpecificLostThought, recipient, KidnapVictim).Add();
            // outIndividualThoughts.Add(new IndividualThoughtToAdd(genderSpecificDiedThought, potentiallyRelatedPawn, victim, 1f, 1f));
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
        var basic = base.GetDetailsPrintout();
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