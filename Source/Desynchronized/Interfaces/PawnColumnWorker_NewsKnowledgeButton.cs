using RimWorld;
using UnityEngine;
using Verse;

namespace Desynchronized.Interfaces;

public class PawnColumnWorker_NewsKnowledgeButton : PawnColumnWorker
{
    public readonly int MinimumWidth = 175;

    public readonly int RecommendedWidth = 250;

    public readonly int TopAreaHeight = 65;

    public readonly int ViewAllTaleNews_Height = 32;
    public readonly int ViewAllTaleNews_Width = 0;

    public override int GetMinHeaderHeight(PawnTable table)
    {
        return Mathf.Max(base.GetMinHeaderHeight(table), TopAreaHeight);
    }

    public override int GetMinWidth(PawnTable table)
    {
        return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(MinimumWidth));
    }

    public override int GetOptimalWidth(PawnTable table)
    {
        return Mathf.Clamp(RecommendedWidth, GetMinWidth(table), GetMaxWidth(table));
    }

    public override void DoHeader(Rect rect, PawnTable table)
    {
        base.DoHeader(rect, table);

        var rect2 = new Rect(rect.x, rect.y + (rect.height - TopAreaHeight), Mathf.Min(rect.width, 360f),
            ViewAllTaleNews_Height);
        if (Widgets.ButtonText(rect2, "ViewAllTaleNews".Translate()))
        {
            Find.WindowStack.Add(new Dialog_NewsTrackerViewer());
        }
    }

    public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
    {
        var buttonRect = new Rect(rect.x, rect.y + 2, RecommendedWidth, rect.height - 4);
        // Rect rect4 = new Rect(x, rect.y + 2f, num2, rect.height - 4f);
        if (Widgets.ButtonText(buttonRect, "WhatDoesThisPawnKnow".Translate()))
        {
            Find.WindowStack.Add(new Dialog_NewsTrackerViewer(pawn));
        }
    }
}