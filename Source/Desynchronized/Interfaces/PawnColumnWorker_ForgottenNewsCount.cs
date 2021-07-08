using System.Linq;
using System.Text;
using Desynchronized.TNDBS.Extenders;
using RimWorld;
using Verse;

namespace Desynchronized.Interfaces
{
    public class PawnColumnWorker_ForgottenNewsCount : PawnColumnWorker_Text
    {
        private int GetForgottenNewsCount(Pawn pawn)
        {
            return (int) (pawn.GetNewsKnowledgeTracker() != null
                ? pawn.GetNewsKnowledgeTracker().GetAllForgottenNewsReferences().Count()
                : (int?) 0);
        }

        protected override string GetTextFor(Pawn pawn)
        {
            return GetForgottenNewsCount(pawn).ToString();
        }

        protected override string GetTip(Pawn pawn)
        {
            var builder = new StringBuilder("ForgottenNewsTip_01".Translate());
            builder.Append(GetForgottenNewsCount(pawn));
            builder.AppendLine("\n");
            builder.Append("ForgottenNewsTip_02".Translate());

            return builder.ToString();
        }
    }
}