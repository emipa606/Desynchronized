﻿namespace Desynchronized.Patches.News_Sold;


//[HarmonyPatch(typeof(GenGuest))]
//[HarmonyPatch("AddPrisonerSoldThoughts", MethodType.Normal)]
//public class PreFix_GenGuest_PrisonerSoldThoughts
//{
//    private static readonly List<string> reportedNamespaces = new List<string>();

//    [HarmonyPrefix]
//    public static bool PreventVanillaThoughts()
//    {
//        var investigateFrame = new StackFrame(2);
//        var namespaceString = investigateFrame.GetMethod().ReflectedType?.Namespace;

//        if (namespaceString == "RimWorld" || namespaceString == "Verse")
//        {
//            return false;
//        }

//        if (reportedNamespaces.Contains(namespaceString))
//        {
//            return true;
//        }

//        reportedNamespaces.Add(namespaceString);
//        DesynchronizedMain.LogError("Mod incompatibility detected. " +
//                                    "There are some other mods calling the vanilla GenGuest.AddPrisonerSoldThoughts() function.\n" +
//                                    "This detected mod comes from " + namespaceString + ".\n" +
//                                    Environment.StackTrace);

//        return true;
//    }
//}