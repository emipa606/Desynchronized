using System;
using System.Linq;
using Desynchronized.TNDBS.Datatypes;
using Desynchronized.TNDBS.Extenders;
using Desynchronized.Utilities;
using RimWorld;
using UnityEngine;
using Verse;

namespace Desynchronized.TNDBS;

public class TaleNewsPawnDied : TaleNewsNegativeIndividual
{
    public enum DeathMethod
    {
        INDETERMINATE = -1,
        NON_EXECUTION = 0,
        EXECUTION = 1
    }

    private DeathBrutality brutalityDegree;

    private Hediff culpritHediff;

    private DamageDef killingBlowDamageType;

    private DeathMethod methodOfDeath = DeathMethod.INDETERMINATE;

    public TaleNewsPawnDied()
    {
    }

    public TaleNewsPawnDied(Pawn victim, DamageInfo? dinfo) : base(victim, InstigationInfo.NoInstigator)
    {
        // DesynchronizedMain.LogError("Dumping stacktrace: " + Environment.StackTrace);

        methodOfDeath = DeathMethod.NON_EXECUTION;

        if (!dinfo.HasValue)
        {
            return;
        }

        var dinfoUnpacked = dinfo.Value;
        killingBlowDamageType = dinfoUnpacked.Def;
        if (dinfoUnpacked.Instigator is Pawn instigator)
        {
            InstigationDetails = (InstigationInfo)instigator;
        }
    }

    public TaleNewsPawnDied(Pawn victim, DamageInfo? dinfo, Hediff culprit) : this(victim, dinfo)
    {
        culpritHediff = culprit;
    }

    public TaleNewsPawnDied(Pawn victim, DeathBrutality brutality) : base(victim, InstigationInfo.NoInstigator)
    {
        methodOfDeath = DeathMethod.EXECUTION;
    }

    public Pawn Killer => Instigator;

    public Pawn Victim => PrimaryVictim;

    /// <summary>
    ///     Defaults to HUMANE if something went wrong.
    /// </summary>
    public DeathBrutality BrutalityDegree
    {
        get => brutalityDegree;
        private set => brutalityDegree = value;
    }

    public DamageDef KillingBlowDamageDef => killingBlowDamageType;

    public DeathMethod MethodOfDeath => methodOfDeath;

    public Hediff CulpritHediff => culpritHediff;

    [Obsolete("Use constructors instead.")]
    public static TaleNewsPawnDied GenerateAsExecution(Pawn victim, DeathBrutality brutality)
    {
        return new TaleNewsPawnDied(victim, brutality);
    }

    [Obsolete("Use constructors instead.")]
    public static TaleNewsPawnDied GenerateGenerally(Pawn victim, DamageInfo? dinfo, Hediff culprit)
    {
        return new TaleNewsPawnDied(victim, dinfo, culprit);
    }

    public override string GetNewsTypeName()
    {
        return "Pawn Died";
    }

    protected override void ConductSaveFileIO()
    {
        base.ConductSaveFileIO();
        Scribe_Values.Look(ref methodOfDeath, "methodOfDeath", DeathMethod.INDETERMINATE);
        Scribe_Values.Look(ref brutalityDegree, "brutalityDegree", DeathBrutality.HUMANE);
        Scribe_Defs.Look(ref killingBlowDamageType, "killingBlowDamageType");
        Scribe_Deep.Look(ref culpritHediff, "culpritHediff");
    }

    /// <summary>
    ///     Attempts to process this TaleNews as an execution event. Completes processing and returns true if successful;
    ///     aborts and returns false otherwise.
    /// </summary>
    /// <param name="recipient"></param>
    /// <returns></returns>
    private bool TryProcessAsExecutionEvent(Pawn recipient)
    {
        if (methodOfDeath != DeathMethod.EXECUTION)
        {
            return false;
        }

        // Rather simple. Copied from vanilla code.
        var forcedStage = (int)BrutalityDegree;
        var thoughtToGive =
            Victim.IsColonist ? ThoughtDefOf.KnowColonistExecuted : ThoughtDefOf.KnowGuestExecuted;
        recipient.needs.mood.thoughts.memories.TryGainMemory(
            ThoughtMaker.MakeThought(thoughtToGive, forcedStage));

        return true;
    }

    /// <summary>
    ///     Attempts to give the killer their appropriate excitement (or disappointment). Aborts if there is no killer.
    /// </summary>
    /// <param name="recipient"></param>
    private void TryProcessKillerHigh(Pawn recipient)
    {
        // Killer != null => DamageInfo != null
        if (Killer == null)
        {
            return;
        }

        // Currently you can't really kill yourself.
        // That would be something interesting, but we don't do that here.
        if (recipient != Killer)
        {
            return;
        }

        // IDK, is this something we can consider checking?
        if (!KillingBlowDamageDef.ExternalViolenceFor(Victim))
        {
            return;
        }

        // Why this check tho
        if (recipient.story == null)
        {
            return;
        }

        // Bloodlust thoughts for Bloodlust guys, currently only for human victims
        // We can expand upon this, and add in witnessed death (animals) with bloodlust
        if (!Victim.RaceProps.Humanlike)
        {
            return;
        }

        // Try to add Bloodlust thoughts; will be auto-rejected if recipient does not have Bloodlust
        new IndividualThoughtToAdd(ThoughtDefOf.KilledHumanlikeBloodlust, recipient).Add();

        // Try to add Defeated Hostile Leader thoughts
        if (Victim.HostileTo(Killer) && Victim.Faction != null &&
            PawnUtility.IsFactionLeader(Victim) && Victim.Faction.HostileTo(Killer.Faction))
        {
            new IndividualThoughtToAdd(ThoughtDefOf.DefeatedHostileFactionLeader, Killer,
                Victim).Add();
        }
    }

    /// <summary>
    ///     Attempts to give the recipient eye-witness thoughts; general thoughts otherwise.
    /// </summary>
    /// <param name="recipient"></param>
    private void TryProcessEyeWitness(Pawn recipient)
    {
        // There is potential to expand upon this condition.
        if (!Victim.RaceProps.Humanlike)
        {
            return;
        }

        if (recipient.CanWitnessOtherPawn(Victim))
        {
            GiveOutEyewitnessThoughts(recipient);
        }
        else
        {
            GiveOutGenericThoughts(recipient);
        }
    }

    /// <summary>
    ///     Attempts to give the recipient thoughts that are derived from their relationship statuses, if there are any.
    /// </summary>
    /// <param name="recipient"></param>
    private void TryProcessRelationshipThoughts(Pawn recipient)
    {
        // Check if there is any relationship at all
        var tracker = Victim.relations;
        if (tracker != null && tracker.PotentiallyRelatedPawns.Contains(recipient))
        {
            GiveOutRelationshipBasedThoughts(recipient);
        }
    }

    /// <summary>
    ///     Gives out eyewitness Thoughts.
    ///     <para />
    ///     Only for eyewitnesses, that is, those Pawns whose .CanWitnessOtherPawn(Victim) returns true
    /// </summary>
    private void GiveOutEyewitnessThoughts(Pawn recipient)
    {
        // News is shocking because witnessed by naked eye
        recipient.GetNewsKnowledgeTracker()?.AttemptToObtainExistingReference(this).FlagAsShockingNews();

        if (recipient.Faction == Victim.Faction)
        {
            new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathAlly, recipient).Add();
        }
        else if (Victim.Faction == null || !Victim.Faction.HostileTo(recipient.Faction))
        {
            new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathNonAlly, recipient).Add();
        }

        if (recipient.relations.FamilyByBlood.Contains(Victim))
        {
            new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathFamily, recipient).Add();
        }

        new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathBloodlust, recipient).Add();
    }

    /// <summary>
    ///     Gives out generic thoughts such as Colonist Died or Prisoner Died, etc.
    ///     <para />
    ///     For everyone, though eyewitnesses should preferrably use GiveOutEyewitnessThoughts()
    /// </summary>
    private void GiveOutGenericThoughts(Pawn recipient)
    {
        if (Victim.Faction == Faction.OfPlayer && Victim.Faction == recipient.Faction &&
            Victim.HostFaction != recipient.Faction)
        {
            recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowColonistDied, Victim);
        }

        var prisonerIsInnocent = Victim.IsPrisonerOfColony && !Victim.guilt.IsGuilty && !Victim.InAggroMentalState;
        if (prisonerIsInnocent && recipient.Faction == Faction.OfPlayer && !recipient.IsPrisoner)
        {
            recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowPrisonerDiedInnocent, Victim);
        }
    }

    /// <summary>
    ///     Gives out relationship-based Thoughts, such as My Relative Died, and Killer Killed My Relative/Friend/Rival.
    ///     <para />
    ///     Most thoughts given out here are all social thoughts. The very few excptions to this are the Bonded Animal Died
    ///     thought
    /// </summary>
    private void GiveOutRelationshipBasedThoughts(Pawn recipient)
    {
        var mostImportantRelation = recipient.GetMostImportantRelation(Victim);
        if (mostImportantRelation == null)
        {
            return;
        }

        // Step 2.0: News is shocking because relative died.
        recipient.GetNewsKnowledgeTracker()?.AttemptToObtainExistingReference(this).FlagAsShockingNews();

        // Step 2.1: My relative died
        var genderSpecificDiedThought = mostImportantRelation.GetGenderSpecificDiedThought(Victim);
        if (genderSpecificDiedThought != null)
        {
            new IndividualThoughtToAdd(genderSpecificDiedThought, recipient, Victim).Add();
        }

        // Step 2.2: Hatred towards killer; does not allow self-hating
        if (Killer == null || Killer == Victim || Killer == recipient)
        {
            return;
        }

        // This killer killed my relatives
        GiveOutHatredTowardsKiller_ForRelatives(mostImportantRelation, recipient);

        // This killer killed my friend/rival
        if (recipient.RaceProps.IsFlesh)
        {
            GiveOutHatredTowardsKiller_ForFriendsOrRivals(recipient);
        }
    }

    private void GiveOutHatredTowardsKiller_ForRelatives(PawnRelationDef mostImportantRelation, Pawn recipient)
    {
        var genderSpecificKilledThought = mostImportantRelation.GetGenderSpecificKilledThought(Victim);
        if (genderSpecificKilledThought != null)
        {
            new IndividualThoughtToAdd(genderSpecificKilledThought, recipient, Killer).Add();
        }
    }

    private void GiveOutHatredTowardsKiller_ForFriendsOrRivals(Pawn recipient)
    {
        var opinion = recipient.relations.OpinionOf(Victim);
        IndividualThoughtToAdd? tempThought = null;

        // Woah we got our asses handed with a bad coding style...
        // The IndividualThoughtToAdd struct does not allow null ThoughtDef
        // So here we are using another way to write concise code
        if (opinion >= 20)
        {
            tempThought = new IndividualThoughtToAdd(ThoughtDefOf.KilledMyFriend, recipient, Killer, 1f,
                Victim.relations.GetFriendDiedThoughtPowerFactor(opinion));
        }
        else if (opinion <= -20)
        {
            tempThought = new IndividualThoughtToAdd(ThoughtDefOf.KilledMyRival, recipient, Killer, 1f,
                Victim.relations.GetRivalDiedThoughtPowerFactor(opinion));
        }

        tempThought?.Add();
    }

    /// <summary>
    ///     Gives My Friend/Rival Died mood thoughts
    /// </summary>
    private void GiveOutFriendOrRivalDiedThoughts(Pawn recipient)
    {
        var opinion = recipient.relations.OpinionOf(Victim);
        if (opinion >= 20)
        {
            new IndividualThoughtToAdd(ThoughtDefOf.PawnWithGoodOpinionDied, recipient, Victim,
                Victim.relations.GetFriendDiedThoughtPowerFactor(opinion)).Add();
        }
        else if (opinion <= -20)
        {
            new IndividualThoughtToAdd(ThoughtDefOf.PawnWithBadOpinionDied, recipient, Victim,
                Victim.relations.GetRivalDiedThoughtPowerFactor(opinion)).Add();
        }
    }

    protected override void GiveThoughtsToReceipient(Pawn recipient)
    {
        if (!recipient.IsCapableOfThought())
        {
            return;
        }

        if (!TryProcessAsExecutionEvent(recipient))
        {
            // Someone killed another one.
            TryProcessKillerHigh(recipient);
            TryProcessEyeWitness(recipient);
        }

        // These are general thoughts.
        TryProcessRelationshipThoughts(recipient);
        GiveOutFriendOrRivalDiedThoughts(recipient);
    }

    public override float CalculateNewsImportanceForPawn(Pawn pawn, TaleNewsReference reference)
    {
        // Again, this code is also a placeholder.

        float result = pawn.GetSocialProximityScoreForOther(Victim);
        // We have 2500 tick for 1 RW hour; hence, 60000 tick for 1 RW day
        // Shock grade scrapped in favour of simple boolean "shocking news" flag
        // result += (int)reference.ShockGrade * Mathf.Pow(0.5f, (1.0f / (60000 * 15)) * (Find.TickManager.TicksGame - reference.TickReceived));
        //float victimKindScore = Victim.RaceProps.Animal ? 1 : 10;
        // First determine thought by relations
        var potentialGivenThought =
            pawn.GetMostImportantRelation(Victim)?.GetGenderSpecificDiedThought(Victim);
        // If there exists none, then determine by factions
        if (potentialGivenThought == null)
        {
            if (pawn.Faction != null && pawn.Faction == Victim.Faction)
            {
                potentialGivenThought = ThoughtDefOf.KnowColonistDied;
            }
        }

        float relationalDeathImpact;
        if (potentialGivenThought != null)
        {
            var tempThought = ThoughtMaker.MakeThought(potentialGivenThought);
            tempThought.pawn = pawn;
            relationalDeathImpact = Mathf.Abs(tempThought.MoodOffset());
        }
        else
        {
            relationalDeathImpact = 0;
        }

        // Calculate main impact score
        // Base score is 2
        float mainScore = 2;
        // Accumulate relational impact
        mainScore += relationalDeathImpact;
        // Humanlike bonus: killing a man should be more significant than killing an animal
        if (PrimaryVictim.RaceProps.Humanlike)
        {
            mainScore += 5;
        }

        // Body size scaling: killing XL animals should be more significant than killing S animals
        mainScore *= PrimaryVictim.BodySize;
        // Pawn relations scaling: pawns with deeper bonds or deeper toothmarks should be more significant. Scales up to factor of 3.
        mainScore *= 1 + Mathf.Abs((float)pawn.relations.OpinionOf(Victim) / 50);
        // Faction relations scaling: factions with stronger relations should be more significant. Scales up to factor of 3.
        var interFactionGoodwill = pawn.Faction.GetGoodwillWith(Victim.Faction);
        mainScore *= 1 + Mathf.Abs((float)interFactionGoodwill / 50);

        // News importance decays over time. Normal rate is halving per year.
        // Decay factor increased if news is shocking
        var decayFactor = 0.5f;
        if (reference.IsShockingNews)
        {
            decayFactor = 0.75f;
        }

        // There are 60000 ticks per day, 15 days per Quadrum, and 4 Quadrums per year.
        result += mainScore * Mathf.Pow(decayFactor,
            1.0f / (60000 * 15 * 4) * (Find.TickManager.TicksGame - reference.TickReceived));

        // Memories are faulty.
        // They can be stronger or weaker, depending on how the brain is functioning at that moment.
        // Goes from -2 to +2.
        result += (Rand.Value * 4) - 2;

        // Check that the result is valid; value should not drop below 0.
        if (result < 0)
        {
            return 0;
        }

        return result;
    }

    public override string GetDetailsPrintout()
    {
        // Victim = [bleh]
        var basic = base.GetDetailsPrintout();

        // Determine death message.
        var deathMessage = "";
        if (KillingBlowDamageDef != null)
        {
            // Died due to damage.
            deathMessage =
                KillingBlowDamageDef.deathMessage.Formatted(Victim.LabelShort.CapitalizeFirst(),
                    Victim.Named("PAWN"));
        }
        else if (culpritHediff != null)
        {
            // Died due to health conditions.
            deathMessage = "PawnDiedBecauseOf".Translate(Victim.LabelShort.CapitalizeFirst(),
                culpritHediff.def.LabelCap, Victim.Named("PAWN"));
        }
        else
        {
            // Generally died.
            "PawnDied".Translate(Victim.LabelShort.CapitalizeFirst(), Victim.Named("PAWN"));
        }

        basic += "\n" + deathMessage;

        // Determine killer id
        if (Killer == null)
        {
            return basic;
        }

        basic += "\nKilled by: ";
        if (Killer.Name != null)
        {
            basic += Killer.Name;
        }
        else
        {
            basic += Killer.ToString();
        }

        return basic;
    }

    protected override void DiscardNewsDetails()
    {
        base.DiscardNewsDetails();
        methodOfDeath = DeathMethod.INDETERMINATE;
        killingBlowDamageType = null;
    }
}