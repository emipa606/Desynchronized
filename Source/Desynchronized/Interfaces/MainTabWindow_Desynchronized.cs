using RimWorld;

namespace Desynchronized.Interfaces
{
    public class MainTabWindow_Desynchronized : MainTabWindow_PawnTable
    {
        protected override PawnTableDef PawnTableDef => Desynchronized_PawnTableDefOf.NewsKnowledge;
    }
}