using Desynchronized.TNDBS;
using HarmonyLib;
using Verse;

namespace Desynchronized.Patches;

/// <summary>
///     Prefix to catch the corpse location
/// </summary>
[HarmonyPatch(typeof(Corpse), nameof(Corpse.ButcherProducts), typeof(Pawn), typeof(float))]
public class Corpse_ButcherProducts
{
    private static Corpse corpse;
    public static IntVec3 corpseLocation;
    public static Map corpseMap;
    public static Room corpseRoom;
    public static bool overrideValue;
    public static TaleNewsPawnButchered corpseNews;

    public static void Prefix(ref Corpse __instance)
    {
        corpse = __instance;
        corpseLocation = __instance.PositionHeld;
        corpseMap = __instance.MapHeld;
        corpseRoom = __instance.GetRoom();
        overrideValue = false;
        corpseNews = new TaleNewsPawnButchered(corpse.InnerPawn);
    }
}