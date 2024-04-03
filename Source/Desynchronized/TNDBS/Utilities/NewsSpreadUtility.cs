using System.Collections.Generic;
using System.Linq;
using Desynchronized.TNDBS.Extenders;
using Verse;

namespace Desynchronized.TNDBS.Utilities;

public class NewsSpreadUtility
{
    public enum SpreadMode
    {
        RANDOM,
        DISTINCT
    }

    public static void SpreadNews(Pawn initiator, Pawn receiver, SpreadMode mode = SpreadMode.RANDOM)
    {
        var newsToSend = DetermineTaleNewsToTransmit(initiator, receiver, mode);
        AttemptToTransmitNews(receiver, newsToSend);
    }

    private static TaleNewsReference DetermineTaleNewsToTransmit(Pawn initiator, Pawn receiver, SpreadMode mode)
    {
        TaleNewsReference result;

        switch (mode)
        {
            case SpreadMode.DISTINCT:
                SelectNewsDistinctly(initiator, receiver, out result);
                break;
            case SpreadMode.RANDOM:
                SelectNewsRandomly(initiator, out result);
                break;
            default:
                result = TaleNewsReference.DefaultReference;
                break;
        }

        return result;
    }

    private static void SelectNewsRandomly(Pawn initiator, out TaleNewsReference result)
    {
        // Is now weighted random.
        var listInitiator = initiator.GetNewsKnowledgeTracker()?.GetAllValidNonForgottenNewsReferences().ToList();

        if (listInitiator is { Count: 0 })
        {
            result = TaleNewsReference.DefaultReference;
        }
        else
        {
            // Collect weights
            var weights = new List<float>();
            float weightSum = 0;
            if (listInitiator == null)
            {
                result = null;
                return;
            }

            foreach (var reference in listInitiator)
            {
                var importanceScore = reference.NewsImportance;
                weights.Add(importanceScore);
                weightSum += importanceScore;
            }

            // Normalize weights
            for (var i = 0; i < weights.Count; i++)
            {
                weights[i] /= weightSum;
            }

            // Select index
            var randomChoice = Rand.Value;
            var selectedIndex = -1;
            weightSum = 0;
            for (var i = 0; i < weights.Count; i++)
            {
                var temp = weights[i];
                if (temp == 0)
                {
                    continue;
                }

                weightSum += temp;
                if (!(weightSum >= randomChoice))
                {
                    continue;
                }

                selectedIndex = i;
                break;
            }

            result = listInitiator[selectedIndex];
        }
    }

    private static void SelectNewsDistinctly(Pawn initiator, Pawn receiver, out TaleNewsReference result)
    {
        var listInitiator = initiator.GetNewsKnowledgeTracker()?.AllNewsReferences_ReadOnlyList;
        // DesynchronizedMain.TaleNewsDatabaseSystem.ListAllAwarenessOfPawn(initiator);
        var listReceiver = receiver.GetNewsKnowledgeTracker()?.AllNewsReferences_ReadOnlyList;
        // DesynchronizedMain.TaleNewsDatabaseSystem.ListAllAwarenessOfPawn(receiver);

        // Distinct List
        var listDistinct = new List<TaleNewsReference>();

        // Find out the contents of the distinct list
        if (listInitiator != null)
        {
            foreach (var reference in listInitiator)
            {
                if (listReceiver != null && !listReceiver.Contains(reference))
                {
                    listDistinct.Add(reference);
                }
            }
        }

        // Select one random entry from the distinct list
        result = listDistinct.Count == 0
            ? TaleNewsReference.DefaultReference
            : listDistinct[(int)((uint)Rand.Int % listDistinct.Count)];
    }

    private static void AttemptToTransmitNews(Pawn receiver, TaleNewsReference news)
    {
        // DesynchronizedMain.LogError("Attempting to transmit " + news.ToString());
        if (receiver == null)
        {
            return;
        }

        if (news == null || news.IsDefaultReference())
        {
            // DesynchronizedMain.LogError("It was a null news. Nothing was done.");
            return;
        }

        receiver.GetNewsKnowledgeTracker()?.KnowNews(news.ReferencedTaleNews);
    }
}