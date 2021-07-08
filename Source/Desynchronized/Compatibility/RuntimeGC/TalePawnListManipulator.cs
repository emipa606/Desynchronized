using System.Collections.Generic;
using Verse;

namespace Desynchronized.Compatibility.RuntimeGC
{
    public class TalePawnListManipulator
    {
        public static void ManipulateListOfPawnsUsedByTales(List<Pawn> original)
        {
            // Note: currently, the list contains only the pawns that aer married, which is no where near optimal.

            var excludedByTaleNews = new List<Pawn>();
            foreach (var pawn in Find.WorldPawns.AllPawnsAliveOrDead)
            {
                foreach (var news in DesynchronizedMain.TaleNewsDatabaseSystem.TalesOfImportance_ReadOnly)
                {
                    if (news.PawnIsInvolved(pawn) && !excludedByTaleNews.Contains(pawn))
                    {
                        excludedByTaleNews.Add(pawn);
                    }
                }
            }

            original.AddRange(excludedByTaleNews);
        }
    }
}