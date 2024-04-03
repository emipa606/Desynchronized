namespace Desynchronized.Compatibility;

public class ModDetector
{
    /// <summary>
    ///     Detects if Psychology is loaded.
    ///     Currently, always returns false since Psychology official is kind of dead in v1.1.
    /// </summary>
    //public static bool PsychologyIsLoaded => LoadedModManager.RunningMods.Any((ModContentPack pack) => pack.Name.Contains("Psychology"));
    public static bool PsychologyIsLoaded => false;
}