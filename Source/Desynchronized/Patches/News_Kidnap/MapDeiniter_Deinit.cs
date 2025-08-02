using Desynchronized.Handlers;
using HarmonyLib;
using Verse;

namespace Desynchronized.Patches.News_Kidnap;

[HarmonyPatch(typeof(MapDeiniter), nameof(MapDeiniter.Deinit), MethodType.Normal)]
public class MapDeiniter_Deinit
{
    // Just in case we have pawns lost in offensive battles.
    public static bool Prefix(Map map)
    {
        //DesynchronizedMain.LogError("Map from patcher: " + map);
        Handler_PawnKidnapped.Signal_OffensiveBattle_BeginBlock(map);
        return true;
    }

    // Reset the signal for future usage.
    public static void Postfix()
    {
        Handler_PawnKidnapped.Signal_OffensiveBattle_EndBlock();
    }
}