namespace Desynchronized.Patches.News_Sold;


/*
/// <summary>
/// Objective: remove segment of code that calls GenGuest.AddPrisonerSoldThoughts(this);
/// 
/// v2.0.0 dev note: disabling GenGuest.AddPrisonerSoldThoughts(this) instead for better compatibility.
/// </summary>
[HarmonyPatch(typeof(Pawn))]
[HarmonyPatch("PreTraded", MethodType.Normal)]
public class Transpiler_Pawn_PreTraded
{
    /// <summary>
    /// Find the 15th ldarg.0 and neutralize, including that statement, 6 assembly statements.
    /// 
    /// v2.0.0 devnote: how about we patch on GenGuest.AddPrisonerSoldThoughts , that seems more practical.
    /// We could catch sold thoughts there, and generate them by ourselves
    /// </summary>
    /// <param name="instructions"></param>
    /// <param name="generator"></param>
    /// <returns></returns>
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        int ignoreCount = 0;
        bool patchIsComplete = false;
        int occurenceOfLdarg0 = 0;

        foreach (CodeInstruction instruction in instructions)
        {
            if (!patchIsComplete && instruction.opcode == OpCodes.Ldarg_0)
            {
                occurenceOfLdarg0++;
                if (occurenceOfLdarg0 == 15)
                {
                    ignoreCount = 6;
                    patchIsComplete = true;
                }
            }

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