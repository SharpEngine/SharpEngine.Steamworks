using SharpEngine.Core.Manager;

namespace SharpEngine.Steamworks;

/// <summary>
/// Static class with extensions and add version functions
/// </summary>
public static class SESteamworks
{
    /// <summary>
    /// Add versions to DebugManager
    /// </summary>
    public static void AddVersions()
    {
        DebugManager.Versions.Add("Steamworks.NET", "20.1.0");
        DebugManager.Versions.Add("SharpEngine.Steamworks", "1.1.0");
    }
}
