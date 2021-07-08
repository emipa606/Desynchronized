namespace Desynchronized.Patches.News_Sold
{
    /*
    /// <summary>
    /// Objective: rather simple actually;
    /// eradicate the signature double-layer loop.
    /// 
    /// v2.0.0 devnote: rewrite this along with the postfix class into a prefix class so that the code is clearer to maintain
    /// </summary>
    [HarmonyPatch(typeof(TransportPodsArrivalAction_GiveGift))]
    [HarmonyPatch("Arrived", MethodType.Normal)]
    public class Transpiler_TPAAGG_Arrived
    {
        /// <summary>
        /// This is very straight-forward; the double-layer loop is at the beginning of the method block.
        /// <para/>
        /// Ignore the first 41 assembly instructions.
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns></returns>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RemoveDoubleLoop(IEnumerable<CodeInstruction> instructions)
        {
            int ignoreCount = 41;

            foreach (CodeInstruction instruction in instructions)
            {
                if (ignoreCount > 0)
                {
                    instruction.opcode = OpCodes.Nop;
                    ignoreCount--;
                }

                yield return instruction;
            }
        }
    }
    */
}