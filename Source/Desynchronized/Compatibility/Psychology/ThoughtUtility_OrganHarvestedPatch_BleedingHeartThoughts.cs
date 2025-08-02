using System.Reflection;
using HarmonyLib;

namespace Desynchronized.Compatibility.Psychology;

[HarmonyPatch]
public class ThoughtUtility_OrganHarvestedPatch_BleedingHeartThoughts
{
    public static MethodBase TargetMethod()
    {
        return AccessTools.Method("Psychology.Harmony.ThoughtUtility_OrganHarvestedPatch:BleedingHeartThoughts");
    }

    public static bool Prepare()
    {
        return ModDetector.PsychologyIsLoaded;
    }

    public static bool Prefix()
    {
        return false;
    }
}