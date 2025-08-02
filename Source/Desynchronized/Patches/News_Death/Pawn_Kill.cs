using Desynchronized.Handlers;
using HarmonyLib;
using Verse;

namespace Desynchronized.Patches.News_Death;

[HarmonyPatch(typeof(Pawn), nameof(Pawn.Kill), MethodType.Normal)]
public class Pawn_Kill
{
    public static void Postfix(Pawn __instance, DamageInfo? dinfo, Hediff exactCulprit)
    {
        Handler_PawnDied.HandlePawnDied(__instance, dinfo, exactCulprit);
    }
}