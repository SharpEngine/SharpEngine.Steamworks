using SharpEngine.Core;
using SharpEngine.Core.Manager;
using SharpEngine.Core.Utils;
using SharpEngine.Steamworks;
using SharpEngine.Steamworks.Steam;

namespace Testing;

internal static class Program
{
    private static void Main()
    {
        SESteamworks.AddVersions();

        var window = new Window(
            1280,
            920,
            "SE Raylib",
            Color.CornflowerBlue,
            null,
            true,
            true,
            true
        )
        {
            RenderImGui = DebugManager.CreateSeImGuiWindow
        };

        SteamManager.AddAchievement(new Achievement("TUER_TIMMY", "Kill Timmy"));
        SteamManager.AddAchievement(new Achievement("NEW_ALLY", "New Ally"));
        SteamManager.AddAchievement(new Achievement("WAVE_1", "Timid Start"));
        SteamManager.AddAchievement(new Achievement("WAVE_5", "In Midst of Battle"));
        SteamManager.AddAchievement(new Achievement("WAVE_10", "That's a lot, isn't it?"));

        SteamManager.Init(window, APPID);

        window.AddScene(new MyScene());

        window.Run();
    }
}
