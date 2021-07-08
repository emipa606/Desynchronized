using System.Reflection;
using HarmonyLib;

namespace Desynchronized.Compatibility.Psychology
{
    public class PreFix_Psycho_PostFix_ThoughtUtility_HarvestThoughts
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method("Psychology.Harmony.ThoughtUtility_OrganHarvestedPatch:BleedingHeartThoughts");
        }

        public static bool Prepare()
        {
            return ModDetector.PsychologyIsLoaded;
        }

        [HarmonyPrefix]
        public static bool DenyDoubleOrganHarvestBug()
        {
            return false;
        }
    }
}