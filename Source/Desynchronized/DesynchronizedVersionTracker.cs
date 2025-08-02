using System;
using HugsLib.Utils;
using Verse;

namespace Desynchronized;

public class DesynchronizedVersionTracker : UtilityWorldObject
{
    // We will do "previous version" and "current version"

    private string versionOfMod;

    public Version VersionOfModWithinSave => versionOfMod == null ? new Version(0, 0, 0, 0) : new Version(versionOfMod);

    public static Version CurrentVersion => typeof(DesynchronizedMain).Assembly.GetName().Version;

    public string VersionOfMod => versionOfMod;

    public override void PostAdd()
    {
        base.PostAdd();
        versionOfMod = typeof(DesynchronizedMain).Assembly.GetName().Version.ToString();
    }

    public override void ExposeData()
    {
        // Actually IO-ing
        base.ExposeData();
        Scribe_Values.Look(ref versionOfMod, "versionOfMod");

        // The actual processing
        if (Scribe.mode != LoadSaveMode.LoadingVars)
        {
            return;
        }

        // For some reason this value did not get included in the save-file.
        // Just making sure the string is stored properly, so it could be saved properly too.
        versionOfMod ??= typeof(DesynchronizedMain).Assembly.GetName().Version.ToString();
    }
}