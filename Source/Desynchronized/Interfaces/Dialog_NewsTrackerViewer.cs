using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Desynchronized.TNDBS;
using Desynchronized.TNDBS.Extenders;
using UnityEngine;
using Verse;

namespace Desynchronized.Interfaces;

public class Dialog_NewsTrackerViewer : Window
{
    public readonly int EntryHeight = 30;

    public readonly int EntryWidth = 800;

    public readonly int HeaderLineHeight = 6;

    public readonly int ScrollerMargin = 16;

    public readonly int TopAreaHeight = 58;

    public List<TaleNewsReference> knownNews;

    public Vector2 scrollPosition = Vector2.zero;

    private bool shouldDisplayForgottenNews = true;

    public Pawn subjectPawn;

    /// <summary>
    ///     Instantiates a new Tale-News Knowledge Tracker dialog.
    /// </summary>
    /// <param name="subject">Optional. Causes the dialog to focus on this specific pawn if provided.</param>
    public Dialog_NewsTrackerViewer(Pawn subject = null)
    {
        subjectPawn = subject;
        var pawnName = subject != null ? subject.Name.ToStringFull : (string)"AllPawns".Translate();
        optionalTitle = "ViewingKnowledgeOf".Translate() + pawnName;
        resizeable = false;
        forcePause = DesynchronizedMain.NewsUI_ShouldAutoPause;
        doCloseX = true;
        doCloseButton = true;
        preventCameraMotion = false;
        closeOnClickedOutside = true;
        //absorbInputAroundWindow = true;
    }

    public override Vector2 InitialSize => new Vector2(800, UI.screenHeight - 100);

    public override void DoWindowContents(Rect inRect)
    {
        var mainAreaBegin = 0;
        DrawToggleDisplayForgottenNews(ref mainAreaBegin);

        // Draw first row
        var headerRect = new Rect(0, mainAreaBegin, EntryWidth, EntryHeight);
        DrawHeaderRow(headerRect);

        var mainRect = new Rect(0, mainAreaBegin + EntryHeight, inRect.width,
            inRect.height - TopAreaHeight - EntryHeight - mainAreaBegin);
        DrawRemainingRows(mainRect);

        GenUI.ResetLabelAlign();
        /*
        DesynchronizedMain.LogError("Rectangle is: " + inRect.ToString());
        throw new NotImplementedException();
        */
    }

    private void DrawToggleDisplayForgottenNews(ref int mainAreaBeginPos)
    {
        if (subjectPawn != null)
        {
            return;
        }

        // Button for overall view: filter forgotten news?
        var buttonRect = new Rect(0, 0 + 2, 400, EntryHeight - 4);
        // Rect rect4 = new Rect(x, rect.y + 2f, num2, rect.height - 4f);
        if (Widgets.ButtonText(buttonRect, "ForgottenNewsToggle".Translate()))
        {
            shouldDisplayForgottenNews = !shouldDisplayForgottenNews;
        }

        mainAreaBeginPos += 30;

        var boundingRect = new Rect(400, 0, 400, EntryHeight);
        var textRect = boundingRect;
        textRect.xMin += 10;
        textRect.xMax -= 10;
        string readout = shouldDisplayForgottenNews
            ? "ShowingForgottenNews".Translate()
            : "HidingForgottenNews".Translate();

        Text.Anchor = TextAnchor.MiddleLeft;
        Widgets.Label(textRect, readout);
    }

    [Obsolete("Consider patronizing NewsSelector.")]
    private IEnumerable<TaleNews> ObtainTaleNewsForPawn(Pawn subject = null)
    {
        if (subject != null)
        {
            var listOfAllKnownNews = subject.GetNewsKnowledgeTracker()?.ListOfAllKnownNews;
            if (listOfAllKnownNews == null)
            {
                yield break;
            }

            foreach (var reference in listOfAllKnownNews)
            {
                yield return reference.ReferencedTaleNews;
            }
        }
        else
        {
            foreach (var news in DesynchronizedMain.TaleNewsDatabaseSystem.TalesOfImportance_ReadOnly)
            {
                yield return news;
            }
        }
    }

    private void DrawRemainingRows(Rect givenArea)
    {
        // Draw remaining rows

        // Step 1: Determine What to draw
        List<TaleNews> taleNewsList;
        if (subjectPawn == null)
        {
            // Viewing all pawns
            taleNewsList = shouldDisplayForgottenNews
                ? DesynchronizedMain.TaleNewsDatabaseSystem.ListOfAllTaleNews
                : DesynchronizedMain.TaleNewsDatabaseSystem.GetAllValidNonPermForgottenNews().ToList();
        }
        else
        {
            // Viewing individual pawns
            taleNewsList =
                new List<TaleNews>(subjectPawn.GetNewsKnowledgeTracker()?.GetAllValidNonForgottenNews()!);
        }

        var newsCount = taleNewsList.Count;

        // Step 2: Setup the area
        float scrollableHeight = HeaderLineHeight + (newsCount * EntryHeight);
        var viewingRect = new Rect(0, 0, givenArea.width - ScrollerMargin, scrollableHeight);
        Widgets.BeginScrollView(givenArea, ref scrollPosition, viewingRect);
        var currentHeight = HeaderLineHeight;
        var upperPosition = scrollPosition.y - EntryHeight;
        var lowerPosition = scrollPosition.y + givenArea.height;
        // Iterate through

        for (var i = 0; i < newsCount; i++)
        {
            // > or >= ?
            if (currentHeight > upperPosition && currentHeight < lowerPosition)
            {
                var rowRect = new Rect(0, currentHeight, viewingRect.width, EntryHeight);
                DrawRow(rowRect, i, taleNewsList[i]);
            }

            currentHeight += EntryHeight;
        }

        Widgets.EndScrollView();
    }

    /// <summary>
    ///     Draws the header row, along with its white separation line.
    /// </summary>
    /// <param name="givenArea"></param>
    private void DrawHeaderRow(Rect givenArea)
    {
        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.MiddleLeft;
        Widgets.DrawLightHighlight(givenArea);
        GUI.BeginGroup(givenArea);
        DrawNewsID_Header();
        DrawNewsType_Header();
        // reserved 80px for future use
        DrawNewsImportance_Header();
        DrawNewsDetails_Header();
        GUI.EndGroup();

        GUI.color = Color.gray;
        Widgets.DrawLineHorizontal(0, EntryHeight + givenArea.yMin, EntryWidth);
        GUI.color = Color.white;
    }

    private void DrawRow(Rect givenArea, int index, TaleNews news)
    {
        // Determine if highlight is needed
        if (index % 2 == 1)
        {
            Widgets.DrawLightHighlight(givenArea);
        }

        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.MiddleLeft;
        GUI.BeginGroup(givenArea);
        DrawNewsID(news);
        DrawNewsType(news);
        // reserved 80px for future use
        DrawNewsImportanceSpace(news);
        DrawNewsDetails(news);
        GUI.EndGroup();
    }

    private void DrawNewsID_Header()
    {
        var boundingRect = new Rect(0, 0, 80, EntryHeight);
        var textRect = boundingRect;
        textRect.xMin += 10;
        textRect.xMax -= 10;
        Widgets.Label(textRect, "IDNumber".Translate());
    }

    private void DrawNewsID(TaleNews news)
    {
        var boundingRect = new Rect(0, 0, 80, EntryHeight);
        Widgets.DrawHighlightIfMouseover(boundingRect);
        var textRect = boundingRect;
        textRect.xMin += 10;
        textRect.xMax -= 10;
        Widgets.Label(textRect, news.UniqueID.ToString());
        TooltipHandler.TipRegion(boundingRect, "IDNumber_Explanation".Translate());
    }

    private void DrawNewsType_Header()
    {
        var boundingRect = new Rect(80, 0, 240, EntryHeight);
        var textRect = boundingRect;
        textRect.xMin += 10;
        textRect.xMax -= 10;
        Widgets.Label(textRect, "TaleNewsType".Translate());
    }

    private void DrawNewsType(TaleNews news)
    {
        var boundingRect = new Rect(80, 0, 240, EntryHeight);
        Widgets.DrawHighlightIfMouseover(boundingRect);
        var textRect = boundingRect;
        textRect.xMin += 10;
        textRect.xMax -= 10;
        Widgets.Label(textRect, news.GetNewsTypeName());
        TooltipHandler.TipRegion(boundingRect, "TaleNewsType_Explanation".Translate());
    }

    private void DrawNewsImportance_Header()
    {
        // Placeholder
        // Probably don't need a header, quite self-explanatory.
    }

    private void DrawNewsImportanceSpace(TaleNews news)
    {
        var boundingRect = new Rect(320, 0, 80, EntryHeight);
        Widgets.DrawHighlightIfMouseover(boundingRect);
        if (subjectPawn != null)
        {
            // Individual pawn
            var textRect = boundingRect;
            textRect.xMin += 10;
            textRect.xMax -= 10;
            var newsImportance = subjectPawn.GetNewsKnowledgeTracker()?.AttemptToObtainExistingReference(news)
                .NewsImportance;
            if (newsImportance == null)
            {
                return;
            }

            var importance = (float)newsImportance;
            Widgets.Label(textRect, Math.Round(importance, 2).ToString());
            var builder = new StringBuilder("ImportanceScore".Translate());
            builder.Append(importance);
            builder.AppendLine();
            builder.Append("ImportanceScore_Explanation".Translate());
            TooltipHandler.TipRegion(boundingRect, builder.ToString());
        }
        else
        {
            // All pawns
            TooltipHandler.TipRegion(boundingRect, "(Reserved.)");
        }
    }

    private void DrawNewsDetails_Header()
    {
        var boundingRect = new Rect(400, 0, 400 - ScrollerMargin, EntryHeight);
        var textRect = boundingRect;
        textRect.xMin += 10;
        textRect.xMax -= 10;
        Widgets.Label(textRect, "TaleNewsDetails".Translate());
    }

    private void DrawNewsDetails(TaleNews news)
    {
        var boundingRect = new Rect(400, 0, 400 - ScrollerMargin, EntryHeight);
        Widgets.DrawHighlightIfMouseover(boundingRect);
        var textRect = boundingRect;
        textRect.xMin += 10;
        textRect.xMax -= 10;
        // Only the first row is displayed; others are viewed in the tip region.
        string labelString;
        string readoutString;
        // Check if the stuff is forgotten
        if (subjectPawn == null && news.PermanentlyForgotten)
        {
            // All pawns list, all pawns forgot the news.
            labelString = "NewsIsPermForgot".Translate();
            readoutString = "NewsIsPermForgot_Explanation".Translate();
        }
        else
        {
            var newsIsLocallyForgotten = subjectPawn.GetNewsKnowledgeTracker()
                ?.AttemptToObtainExistingReference(news).NewsIsLocallyForgotten;
            if (newsIsLocallyForgotten != null && subjectPawn != null && (bool)newsIsLocallyForgotten)
            {
                // Individual pawns list, individual pawn forgot the news.
                labelString = "NewsIsLocalForgot".Translate();
                readoutString = "NewsIsLocalForgot_Explanation".Translate();
            }
            else
            {
                // In any case, someone remembers; print the details now.
                string originalString;
                try
                {
                    originalString = news.GetDetailsPrintout();
                }
                catch (Exception ex)
                {
                    originalString = DesynchronizedMain.MODPREFIX + "Error: " + ex.Message + "\n" + ex.StackTrace;
                }

                // At this stage, originalString guaranteed to be non-zero.
                var splitStrings = originalString.Split('\n');
                labelString = splitStrings[0];
                readoutString = originalString;
            }
        }

        Widgets.Label(textRect, labelString);
        TooltipHandler.TipRegion(boundingRect, readoutString);
    }
}