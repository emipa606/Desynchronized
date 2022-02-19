using System;
using Verse;

namespace Desynchronized.TNDBS;

/// <summary>
///     A glorified key-value pair to express whether a pawn has heard of some certain TaleNews.
///     <para />
///     Also contains some functions to help with implementing and running the TNDBS.
/// </summary>
public class TaleNewsReference : IExposable
{
    /// <summary>
    ///     Default reference. Its UID is 0.
    /// </summary>
    public static readonly TaleNewsReference DefaultReference = new TaleNewsReference(null)
    {
        uidOfReferencedTaleNews = 0
    };

    private Pawn cachedSubject;

    // Pivate attributes as required by IExposable
    private bool hasBeenActivated;
    private NullableType<float> newsImportance;
    private bool newsIsShocking;
    private int tickReceived;
    private int uidOfReferencedTaleNews = -1;

    /// <summary>
    ///     DO NOT USE THIS CONSTRUCTOR EXPLICITLY.
    /// </summary>
    [Obsolete("Rewrite code to reconstruct the TaleNewsReference properly.", true)]
    public TaleNewsReference()
    {
    }

    /// <summary>
    ///     Used by RimWorld to reconstruct the TaleNewsReference when loading from save.
    /// </summary>
    /// <param name="subject">The owner of this TaleNewsReference</param>
    public TaleNewsReference(Pawn subject)
    {
        CachedSubject = subject;
    }

    public TaleNewsReference(TaleNews news, Pawn recipient) : this(recipient)
    {
        if (!news.IsRegistered)
        {
            throw new ArgumentException("Unregisterd TaleNews received.");
        }

        uidOfReferencedTaleNews = news.UniqueID;
    }

    /// <summary>
    ///     True if the Receipient has reacted to the Underlying TaleNews before.
    /// </summary>
    public bool HasEverActivated => hasBeenActivated;

    /// <summary>
    ///     Retrieves the TaleNews referenced by this TNRef object.
    /// </summary>
    public TaleNews ReferencedTaleNews => DesynchronizedMain.TaleNewsDatabaseSystem?[uidOfReferencedTaleNews];

    /// <summary>
    ///     The subject of this TaleNewsReference, and the recipient of the referenced TaleNews.
    /// </summary>
    public Pawn CachedSubject
    {
        get => cachedSubject;
        internal set
        {
            if (cachedSubject == null)
            {
                cachedSubject = value;
            }
        }
    }

    /// <summary>
    ///     The tick which this tale-news reference is first created/received.
    /// </summary>
    public int TickReceived => tickReceived;

    public bool ReferenceIsValid
    {
        get
        {
            var tempTaleNews = ReferencedTaleNews;
            return CachedSubject != null && tempTaleNews != null && !(tempTaleNews is DefaultTaleNews) &&
                   tempTaleNews.IsValid();
        }
    }

    /// <summary>
    ///     Returns the last calculated news importance score. If the score is null (i.e. news is forgotten), returns 0.
    /// </summary>
    public float NewsImportance => newsImportance.GetValueOrDefault();

    /// <summary>
    ///     Returns true if the pawn has forgotten about the news.
    /// </summary>
    public bool NewsIsLocallyForgotten => !newsImportance.HasValue;

    /// <summary>
    ///     Returns true if no pawn ever remembers about the news.
    /// </summary>
    public bool NewsIsGloballyForgotten => ReferencedTaleNews.PermanentlyForgotten;

    public bool IsShockingNews => newsIsShocking;

    public void ExposeData()
    {
        Scribe_Values.Look(ref uidOfReferencedTaleNews, "newsUID", -1);
        Scribe_Values.Look(ref hasBeenActivated, "activated");
        Scribe_Values.Look(ref tickReceived, "tickReceived", Find.TickManager.TicksGame);
        Scribe_Values.Look(ref newsIsShocking, "newsIsShocking");
        Scribe_Deep.Look(ref newsImportance, "newsImportance");
    }

    public override string ToString()
    {
        if (IsDefaultReference())
        {
            return "Default Reference";
        }

        return "Reference to tale-news with id = " + uidOfReferencedTaleNews;
    }

    /// <summary>
    /// </summary>
    /// <param name="one"></param>
    /// <param name="two"></param>
    /// <returns></returns>
    // We are able to make this method extremely fast by switching over to the index-pointing method.
    public static bool RefsAreEqual(TaleNewsReference one, TaleNewsReference two)
    {
        return one.uidOfReferencedTaleNews == two.uidOfReferencedTaleNews;
    }

    public bool RefIsEqualTo(TaleNewsReference other)
    {
        return RefsAreEqual(this, other);
    }

    public bool IsReferencingTaleNews(TaleNews news)
    {
        return uidOfReferencedTaleNews == news.UniqueID;
    }

    internal void ChangeReferencedUID(int newID)
    {
        uidOfReferencedTaleNews = newID;
    }

    /// <summary>
    ///     Returns true if this tale-news reference is a Default Reference.
    /// </summary>
    /// <returns></returns>
    public bool IsDefaultReference()
    {
        return ReferencedTaleNews is DefaultTaleNews;
    }

    /// <summary>
    ///     Attempts to activate news, and let the recipient react to the news.
    ///     <para />
    ///     Will do nothing if the news has already been activated before.
    /// </summary>
    public void ActivateNews()
    {
        if (hasBeenActivated)
        {
            return;
        }

        // First time activating.
        ReferencedTaleNews.ActivateForReceipient(CachedSubject);
        UpdateNewsImportance(true);
        tickReceived = Find.TickManager.TicksGame;
        hasBeenActivated = true;
    }

    /// <summary>
    ///     The subject forgets about the news. May be able to regain memory by being reminded, but that's another story.
    ///     <para />
    ///     May also be used by Alzheimers.
    /// </summary>
    public void Forget()
    {
        newsImportance = null;
    }

    public void FlagAsShockingNews()
    {
        newsIsShocking = true;
    }

    internal void SelfVerify()
    {
        // No need to initialize newsImportance; as a struct, when it exists, it is already initialized.
        if (!newsImportance.HasValue)
        {
            UpdateNewsImportance(true);
        }
    }

    /// <summary>
    ///     Calculates and updates the news importance. If the importance score is low enough, the news is forgotten.
    /// </summary>
    /// <param name="forceRecheck"></param>
    public void UpdateNewsImportance(bool forceRecheck = false)
    {
        if (!forceRecheck && NewsIsLocallyForgotten)
        {
            return;
        }

        if (ReferencedTaleNews.PermanentlyForgotten)
        {
            // It's pointless to calculate an importance of something that does not exist.
            newsImportance = null;
        }
        else
        {
            var result = ReferencedTaleNews.CalculateNewsImportanceForPawn(cachedSubject, this);

            if (result < 1)
            {
                // Forgets
                newsImportance = null;
            }
            else
            {
                newsImportance = result;
            }
        }
    }
}