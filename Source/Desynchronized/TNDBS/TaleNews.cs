using System;
using Desynchronized.TNDBS.Datatypes;
using Verse;

namespace Desynchronized.TNDBS
{
    /// <summary>
    ///     A TaleNews object simply represents a Tale, but in a more permanent form.
    /// </summary>
    public abstract class TaleNews : IExposable
    {
        public static readonly TaleNews DefaultTaleNews = new DefaultTaleNews();

        private bool isPermanentlyForgotten;

        private LocationInfo locationInfo;
        private int uniqueID = -1;

        /// <summary>
        ///     DO NOT USE THIS CONSTRUCTOR
        /// </summary>
        [Obsolete("This constructor is reserved. Keep this empty.")]
        public TaleNews()
        {
        }

        public TaleNews(LocationInfo info)
        {
            locationInfo = info;
            if (DesynchronizedMain.TaleNewsDatabaseSystem == null)
            {
                DesynchronizedMain.LogError(
                    "The TNDBS is unexpectedly null, cancelling creation of TaleNews. This may happen because the map is still being set up (or, alternatively, you are using Prepare Carefully), but otherwise, this is a bug.");
                return;
            }

            DesynchronizedMain.TaleNewsDatabaseSystem.RegisterNewTaleNews(this);
        }

        public int UniqueID => uniqueID;

        public bool IsRegistered => UniqueID >= 0;

        public LocationInfo LocationOfOccurence
        {
            get => locationInfo;
            protected set => locationInfo = value;
        }

        public bool PermanentlyForgotten => isPermanentlyForgotten;

        public void ExposeData()
        {
            Scribe_Values.Look(ref uniqueID, "uniqueID", -1);
            Scribe_Deep.Look(ref locationInfo, "locationInfo");
            if (locationInfo == null)
            {
                locationInfo = LocationInfo.EmptyLocationInfo;
            }

            Scribe_Values.Look(ref isPermanentlyForgotten, "permaForgotten");

            ConductSaveFileIO();
        }

        [Obsolete("Experimental tech.", true)]
        public static TaleNews GenerateTaleNewsGenerally(TaleNewsTypeEnum typeEnum)
        {
            var newsInstance = Activator.CreateInstance(typeEnum.GetTypeForEnum()) as TaleNews;
            DesynchronizedMain.TaleNewsDatabaseSystem.RegisterNewTaleNews(newsInstance);
            return newsInstance;
        }

        public override string ToString()
        {
            return GetNewsTypeName() + (IsRegistered
                ? " (ID: " + UniqueID + ") "
                : " " + (LocationOfOccurence != null ? "(from " + LocationOfOccurence + ")" : ""));
        }

        internal void ReRegisterWithID(int ID)
        {
            uniqueID = ID;
        }

        internal void ReceiveRegistrationID(int ID)
        {
            if (!IsRegistered)
            {
                uniqueID = ID;
            }
        }

        internal abstract void SelfVerify();

        /// <summary>
        ///     Also handles the cleanup of its related TaleNews
        /// </summary>
        internal void Signal_NewsIsPermanentlyForgotten()
        {
            isPermanentlyForgotten = true;
            // private void PurgeDetails()
            locationInfo = null;
            DiscardNewsDetails();
        }

        /// <summary>
        ///     For when forgetting news.
        /// </summary>
        protected abstract void DiscardNewsDetails();

        /// <summary>
        ///     Wrapper method to let the recipient react to the news.
        /// </summary>
        /// <param name="recipient"></param>
        public void ActivateForReceipient(Pawn recipient)
        {
            if (isPermanentlyForgotten)
            {
                DesynchronizedMain.LogError(
                    "Illegal state. This tale-news should be permanently forgotten. Tale-news: " + ToString());
            }

            try
            {
                GiveThoughtsToReceipient(recipient);
            }
            catch (Exception ex)
            {
                DesynchronizedMain.LogError("Cannot give thought(s). Something went wrong.\n" + ex);
            }
        }

        public TaleNewsReference CreateReferenceForReceipient(Pawn receipient)
        {
            return new TaleNewsReference(this, receipient);
        }

        /// <summary>
        ///     Carry out Scribe IO operations here to recreate your instance.
        ///     <para />
        ///     That said, please also provide an empty constructor for the whole mechanic to work.
        /// </summary>
        protected abstract void ConductSaveFileIO();

        /// <summary>
        ///     Called to apply Thoughts to the receipient.
        ///     You can assume that the Receipient hears of this TaleNews at the correct timing.
        ///     <para />
        ///     The Restructuring has separated the recipient from this template class.
        /// </summary>
        /// <param name="recipient"></param>
        protected abstract void GiveThoughtsToReceipient(Pawn recipient);

        /// <summary>
        ///     Returns a string that shows the type of the tale news.
        ///     <para />
        ///     For example, TaleNewsPawnDied should return "Pawn died".
        /// </summary>
        /// <returns></returns>
        public abstract string GetNewsTypeName();

        /// <summary>
        ///     Determines if the given pawn is specifically involved in this TaleNews (e.g. the victim of the murder).
        ///     <para />
        ///     Generally-involved pawns (e.g. colony established) should not return true.
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns></returns>
        public abstract bool PawnIsInvolved(Pawn pawn);

        /// <summary>
        ///     Determines if the TaleNews is valid; the TaleNews should be able to convey its meaning with the variables it have.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsValid();

        public abstract float CalculateNewsImportanceForPawn(Pawn pawn, TaleNewsReference reference);

        /// <summary>
        ///     Returns a multi-line string that shows the details of the tale-news. The most important information should be
        ///     displayed in the first line.
        ///     <para />
        ///     For example, for TaleNewsPawnDied, the first line should read "Victim: Vector", followed by the next line
        ///     "Instigator: Matt"
        /// </summary>
        /// <returns></returns>
        public abstract string GetDetailsPrintout();
    }
}