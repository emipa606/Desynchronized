using Desynchronized.TNDBS;
using HarmonyLib;
using Verse;

namespace Desynchronized.Patches;

/// <summary>
///     Prefix to catch the corpse location
/// </summary>
[HarmonyPatch(typeof(Corpse))]
[HarmonyPatch("ButcherProducts", MethodType.Normal)]
[HarmonyPatch(new[] { typeof(Pawn), typeof(float) })]
public class PreFix_Corpse_ButcherProducts
{
    public static Corpse corpse;
    public static IntVec3 corpseLocation;
    public static Map corpseMap;
    public static Room corpseRoom;
    public static bool overrideValue;
    public static TaleNewsPawnButchered corpseNews;

    [HarmonyPrefix]
    public static void PreFix(Pawn butcher, float efficiency, ref Corpse __instance)
    {
        corpse = __instance;
        corpseLocation = __instance.PositionHeld;
        corpseMap = __instance.MapHeld;
        corpseRoom = __instance.GetRoom();
        overrideValue = false;
        corpseNews = new TaleNewsPawnButchered(corpse.InnerPawn);
    }
}