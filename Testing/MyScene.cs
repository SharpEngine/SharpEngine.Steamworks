using SharpEngine.Core;
using SharpEngine.Core.Input;
using SharpEngine.Core.Manager;
using SharpEngine.Core.Math;
using SharpEngine.Core.Utils;
using SharpEngine.Core.Widget;
using SharpEngine.Steamworks;

namespace Testing;

public class MyScene : Scene
{
    public MyScene()
    {
        AddWidget(
            new Label(
                new Vec2(640, 460),
                "Instructions :\nE : Print UserName\nR : Print Achievements",
                "RAYLIB_DEFAULT",
                centerAllLines: true,
                fontSize: 50
            )
        );
    }

    public override void Unload()
    {
        base.Unload();
        SteamManager.Shutdown();
    }

    public override void Update(float delta)
    {
        base.Update(delta);

        SteamManager.RunCallbacks();

        if (InputManager.IsKeyPressed(Key.E))
            DebugManager.Log(LogLevel.LogInfo, $"TESTING: Username : {SteamManager.UserName}");
        if (InputManager.IsKeyPressed(Key.R))
            foreach (var achievement in SteamManager.GetAchievements())
            {
                DebugManager.Log(
                    LogLevel.LogInfo,
                    $"TESTING: Achievement : {achievement.AchievementId}"
                );
                DebugManager.Log(LogLevel.LogInfo, $"TESTING:   Name : {achievement.DisplayName}");
                DebugManager.Log(
                    LogLevel.LogInfo,
                    $"TESTING:   Description : {achievement.DisplayDescription}"
                );
                DebugManager.Log(LogLevel.LogInfo, $"TESTING:   Achieved : {achievement.Achieved}");
                DebugManager.Log(LogLevel.LogInfo, $"TESTING:   Icon : {achievement.Icon}");
            }
    }
}
