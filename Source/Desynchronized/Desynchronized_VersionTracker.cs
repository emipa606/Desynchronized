using HugsLib.Utils;
using System;
using Verse;

namespace Desynchronized
{
    [Obsolete("We are using DesyncVerTracker.", true)]
    public class Desynchronized_VersionTracker: UtilityWorldObject
    {
        private string versionOfMod;

        public string VersionOfMod => versionOfMod;

        public override void PostAdd()
        {
            base.PostAdd();
            versionOfMod = typeof(DesynchronizedMain).Assembly.GetName().Version.ToString();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref versionOfMod, "versionOfMod");
            var versionWithinSaveFile = new Version(versionOfMod);

            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                if (versionOfMod == null)
                {
                    versionOfMod = typeof(DesynchronizedMain).Assembly.GetName().Version.ToString();
                }
            }

            // Sanity check; only do this after the vars are loaded.
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                if (versionWithinSaveFile < new Version(1, 4, 5, 0))
                {
                    DesynchronizedMain.TaleNewsDatabaseSystem.SelfPatching_NullVictims();
                }
            }
        }
    }
}
