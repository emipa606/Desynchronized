using Desynchronized.Handlers;
using HarmonyLib;
using Verse;

namespace Desynchronized.Patches.News_Kidnap;

[HarmonyPatch(typeof(MapDeiniter))]
[HarmonyPatch("Deinit", MethodType.Normal)]
public class MultiFix_MapDeiniter
{
    // Just in case we have pawns lost in offensive battles.
    [HarmonyPrefix]
    public static bool Signal_BeginBlock(Map map)
    {
        //DesynchronizedMain.LogError("Map from patcher: " + map);
        Handler_PawnKidnapped.Signal_OffensiveBattle_BeginBlock(map);
        return true;
    }

    // Reset the signal for future usage.
    [HarmonyPostfix]
    public static void Signal_EndBlock()
    {
        Handler_PawnKidnapped.Signal_OffensiveBattle_EndBlock();
    }
}