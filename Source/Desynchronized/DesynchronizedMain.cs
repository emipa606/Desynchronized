using System;
using Desynchronized.Handlers;
using Desynchronized.TNDBS;
using Desynchronized.Utilities;
using HugsLib;
using HugsLib.Settings;
using HugsLib.Utils;
using Verse;

namespace Desynchronized;

public class DesynchronizedMain : ModBase
{
    public static readonly string Desync_PawnIsReferencedString = "Desync_ReferencedByTaleNews";
    public static string MODID => "com.vectorial1024.rimworld.desynchronized";

    /// <summary>
    ///     Already includes a space character.
    /// </summary>
    public static string MODPREFIX => "[Desyncronized]: ";

    /// <summary>
    ///     A very convenient property for myself to limit features to DevMode only.
    /// </summary>
    public static bool WeAreInDevMode => Prefs.DevMode;

    public override string ModIdentifier => MODID;

    // public static InformationKnowledgeStorage InfoKnowStorage { get; private set; }
    public static TaleNewsDatabase TaleNewsDatabaseSystem { get; private set; }
    public static Linker_ArrivalActionAndSender ArrivalActionAndSenderLinker { get; private set; }
    public static DesynchronizedVersionTracker DesynchronizedVersionTracker { get; private set; }
    public static HallOfFigures TheHallOfFigures { get; private set; }
    public static SettingHandle<bool> SettingHandle_AutoPauseNewsInterface { get; private set; }
    public static SettingHandle<bool> SettingHandle_OnlySpreadInSameRoom { get; private set; }
    public static SettingHandle<bool> SettingHandle_VerboseLogging { get; private set; }
    public static bool NewsUI_ShouldAutoPause => SettingHandle_AutoPauseNewsInterface.Value;
    public static bool NewsBehaviour_OnlySpreadInSameRoom => SettingHandle_OnlySpreadInSameRoom.Value;
    public static bool ModBehaviour_VerboseLogging => SettingHandle_VerboseLogging.Value;

    public override void WorldLoaded()
    {
        TaleNewsDatabaseSystem = UtilityWorldObjectManager.GetUtilityWorldObject<TaleNewsDatabase>();
        ArrivalActionAndSenderLinker =
            UtilityWorldObjectManager.GetUtilityWorldObject<Linker_ArrivalActionAndSender>();
        DesynchronizedVersionTracker =
            UtilityWorldObjectManager.GetUtilityWorldObject<DesynchronizedVersionTracker>();
        TheHallOfFigures = UtilityWorldObjectManager.GetUtilityWorldObject<HallOfFigures>();

        if (DesynchronizedVersionTracker.VersionOfModWithinSave < new Version(1, 4, 5, 0))
        {
            TaleNewsDatabaseSystem.SelfPatching_NullVictims();
        }

        TaleNewsDatabaseSystem.SelfVerify();
    }

    public override void DefsLoaded()
    {
        PrepareModSettingHandles();
    }

    private void PrepareModSettingHandles()
    {
        SettingHandle_AutoPauseNewsInterface = Settings.GetHandle("toggleAutoPauseNewsInterface",
            "NewsUIAutoPause_title".Translate(), "NewsUIAutoPause_descr".Translate(), true);
        SettingHandle_OnlySpreadInSameRoom = Settings.GetHandle("toggleOnlySpreadInSameRoom",
            "OnlySpreadInSameRoom_title".Translate(), "OnlySpreadInSameRoom_descr".Translate(), false);
        SettingHandle_VerboseLogging = Settings.GetHandle("toggleVerboseLogging",
            "VerboseLogging_title".Translate(), "VerboseLogging_descr".Translate(), false);
    }

    public static void LogInfo(string message, bool force = false)
    {
        if (!ModBehaviour_VerboseLogging && !force)
        {
            return;
        }

        Log.Message($"{MODPREFIX}{message}");
    }

    public static void LogError(string message)
    {
        Log.Error($"{MODPREFIX}{message}");
    }

    public static void LogWarning(string message)
    {
        Log.Warning($"{MODPREFIX}{message}");
    }
}