using Desynchronized.Handlers;
using HarmonyLib;
using Verse;

namespace Desynchronized.Patches.News_Kidnap;

/// <summary>
///     Post-fixes to generate relevant kidnapped thoughts.
/// </summary>
[HarmonyPatch(typeof(Pawn), nameof(Pawn.PreKidnapped), MethodType.Normal)]
public class Pawn_PreKidnapped
{
    public static void Postfix(Pawn __instance, Pawn kidnapper)
    {
        Handler_PawnKidnapped.HandlePawnKidnapped(__instance, kidnapper);
    }
}