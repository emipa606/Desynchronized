using Desynchronized.TNDBS.Extenders;
using Desynchronized.TNDBS.Utilities;
using RimWorld;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Interfaces
{
    public class PawnColumnWorker_ForgottenNewsCount : PawnColumnWorker_Text
    {
        private int GetForgottenNewsCount(Pawn pawn) => pawn.GetNewsKnowledgeTracker().GetAllForgottenNewsReferences().Count();

        protected override string GetTextFor(Pawn pawn)
        {
            return GetForgottenNewsCount(pawn).ToString();
        }

        protected override string GetTip(Pawn pawn)
        {
            StringBuilder builder = new StringBuilder("ForgottenNewsTip_01".Translate());
            builder.Append(GetForgottenNewsCount(pawn));
            builder.AppendLine("\n");
            builder.Append("ForgottenNewsTip_02".Translate());

            return builder.ToString();
        }
    }
}
