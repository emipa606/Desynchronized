using Desynchronized.TNDBS.Extenders;
using Desynchronized.TNDBS.Utilities;
using RimWorld;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Interfaces
{
    public class PawnColumnWorker_KnownNewsCount : PawnColumnWorker_Text
    {
        private int GetKnownNewsCount(Pawn pawn)
        {
            return pawn.GetNewsKnowledgeTracker().GetAllValidNonForgottenNewsReferences().Count();
        }

        protected override string GetTextFor(Pawn pawn)
        {
            return GetKnownNewsCount(pawn).ToString();
        }

        protected override string GetTip(Pawn pawn)
        {
            StringBuilder builder = new StringBuilder("KnownNewsTip_01".Translate());
            builder.Append(GetKnownNewsCount(pawn));
            builder.AppendLine("\n");
            builder.Append("KnownNewsTip_02".Translate());

            return builder.ToString();
        }
    }
}
