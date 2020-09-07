using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Desynchronized.Patches.News_Sold
{
    [HarmonyPatch(typeof(GenGuest))]
    [HarmonyPatch("AddPrisonerSoldThoughts", MethodType.Normal)]
    public class PreFix_GenGuest_PrisonerSoldThoughts
    {
        private static List<string> reportedNamespaces = new List<string>();

        [HarmonyPrefix]
        public static bool PreventVanillaThoughts()
        {
            StackFrame investigateFrame = new StackFrame(2);
            string namespaceString = investigateFrame.GetMethod().ReflectedType.Namespace;

            if (namespaceString == "RimWorld" || namespaceString == "Verse")
            {
                return false;
            }
            if (!reportedNamespaces.Contains(namespaceString))
            {
                reportedNamespaces.Add(namespaceString);
                DesynchronizedMain.LogError("Mod incompatibility detected. " +
                    "There are some other mods calling the vanilla GenGuest.AddPrisonerSoldThoughts() function.\n" +
                    "This detected mod comes from " + namespaceString + ".\n" + Environment.StackTrace);
            }
            return true;
        }
    }
}
