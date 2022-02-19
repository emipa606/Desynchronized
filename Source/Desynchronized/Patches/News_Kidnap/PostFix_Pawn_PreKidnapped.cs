using Desynchronized.Handlers;
using HarmonyLib;
using Verse;

namespace Desynchronized.Patches.News_Kidnap;

/// <summary>
///     Post-fixes to generate relevant kidnapped thoughts.
/// </summary>
[HarmonyPatch(typeof(Pawn))]
[HarmonyPatch("PreKidnapped", MethodType.Normal)]
public class PostFix_Pawn_PreKidnapped
{
    [HarmonyPostfix]
    public static void SignalRelevantHandlers(Pawn __instance, Pawn kidnapper)
    {
        Handler_PawnKidnapped.HandlePawnKidnapped(__instance, kidnapper);
    }
}