using System.Text;
using SharpEngine.Core;
using SharpEngine.Core.Manager;
using SharpEngine.Core.Utils;
using SharpEngine.Steamworks.Steam;
using Steamworks;

namespace SharpEngine.Steamworks;

/// <summary>
/// Static class to manager steam
/// </summary>
public static class SteamManager
{
    private static AppId_t _gameId;
    private static bool _requestedStats;
    private static bool _statsValid;
    private static bool _storeStats;
    private static readonly List<Achievement> Achievements = new();

    /// <summary>
    /// Is SteamManager is Running
    /// </summary>
    public static bool IsRunning { get; private set; } = false;

    /// <summary>
    /// Add Achievement to Manager
    /// </summary>
    /// <param name="achievement">Achievement</param>
    public static void AddAchievement(Achievement achievement) => Achievements.Add(achievement);
    
    /// <summary>
    /// Get All Achievements
    /// </summary>
    /// <returns>Achievements</returns>
    public static List<Achievement> GetAchievements() => Achievements;
    
    /// <summary>
    /// Get Steam User Name
    /// </summary>
    public static string UserName => SteamFriends.GetPersonaName();
    
    /// <summary>
    /// Init SteamManager
    /// </summary>
    /// <param name="window">Game Window</param>
    /// <param name="appId">App Steam Id</param>
    /// <param name="stopIfNotSteam">Stop Window if Steam is not running</param>
    public static void Init(Window window, uint appId, bool stopIfNotSteam = true)
    {
        if (SteamAPI.RestartAppIfNecessary((AppId_t)appId))
        {
            window.Stop();
            return;
        }
        
        DebugManager.Log(LogLevel.LogInfo, "STEAM: Attempting initialization...");
        try
        {
            IsRunning = SteamAPI.IsSteamRunning();
            DebugManager.Log(LogLevel.LogInfo, $"STEAM: Is Running : {SteamAPI.IsSteamRunning()}");
            if (SteamAPI.IsSteamRunning())
            {
                if (SteamAPI.Init())
                {
                    DebugManager.Log(LogLevel.LogInfo, "STEAM: Initialization succeeded !");
                    _gameId = SteamUtils.GetAppID();

                    SteamClient.SetWarningMessageHook(SteamApiDebugTextHook);
                    //Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
                    Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
                    Callback<UserStatsStored_t>.Create(OnUserStatsStored);
                    Callback<UserAchievementStored_t>.Create(OnAchievementStored);

                    DebugManager.Log(LogLevel.LogInfo, $"STEAM: Connected User : {UserName}");
                }
                else
                {
                    DebugManager.Log(LogLevel.LogError, $"STEAM: Initialization failed !");
                    window.Stop();
                }
            }
            else if (stopIfNotSteam)
                window.Stop();
        }
        catch (Exception e)
        {
            DebugManager.Log(LogLevel.LogError, $"STEAM: Exception during Initialization: \n{e.StackTrace}");
            window.Stop();
        }
    }

    /// <summary>
    /// Shutdown SteamManager
    /// </summary>
    public static void Shutdown()
    {
        DebugManager.Log(LogLevel.LogInfo, "STEAM: Shutting down...");
        try
        {
            SteamAPI.Shutdown();
            DebugManager.Log(LogLevel.LogInfo, "STEAM: Shutdown succeeded !");
        }
        catch (Exception e)
        {
            DebugManager.Log(LogLevel.LogError, $"STEAM: Exception during Shutdown: \n{e.StackTrace}");
        }
    }

    /// <summary>
    /// Unlock Achievement
    /// </summary>
    /// <param name="name">Achievement Id</param>
    public static void UnlockAchievement(string name)
    {
        foreach (var achievement in Achievements)
        {
            if (achievement.AchievementId == name)
            {
                achievement.Achieved = true;
                achievement.Icon = 0;
                SteamUserStats.SetAchievement(achievement.AchievementId);
                _storeStats = true;
                break;
            }
        }
    }

    /// <summary>
    /// Get All Subscribed Items
    /// </summary>
    /// <returns>Enumerable of Subscribed Items</returns>
    public static IEnumerable<PublishedFileId_t> GetSubscribedItems()
    {
        var nb = SteamUGC.GetNumSubscribedItems();
        var items = new PublishedFileId_t[nb];
        return SteamUGC.GetSubscribedItems(items, nb) != 0 ? items : Array.Empty<PublishedFileId_t>();
    }

    /// <summary>
    /// Get Item State
    /// </summary>
    /// <param name="id">Item</param>
    /// <returns>State</returns>
    public static uint GetItemState(PublishedFileId_t id) => SteamUGC.GetItemState(id);

    /// <summary>
    /// Get Item Install Info
    /// </summary>
    /// <param name="id">Item</param>
    /// <returns>Install Info</returns>
    public static ItemInstallInfo? GetItemInstallInfo(PublishedFileId_t id) =>
        SteamUGC.GetItemInstallInfo(id, out var punSizeOnDisk, out var pchFolder, 256, out var punTimeStamp)
            ? new ItemInstallInfo(punSizeOnDisk, pchFolder, punTimeStamp)
            : null;

    /// <summary>
    /// Run Steam Callbacks
    /// </summary>
    public static void RunCallbacks()
    {
        SteamAPI.RunCallbacks();

        if (!_requestedStats)
            _requestedStats = SteamUserStats.RequestCurrentStats();

        if (!_statsValid)
            return;

        if (_storeStats)
            _storeStats = !SteamUserStats.StoreStats();
    }

    private static void SteamApiDebugTextHook(int nSeverity, StringBuilder pchDebugText) =>
        DebugManager.Log(LogLevel.LogError, $"STEAM: Severity : {nSeverity} - Message : {pchDebugText}");

    /*private static void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
        if (pCallback.m_bActive != 0)
        {
        }
    }*/

    private static void OnUserStatsReceived(UserStatsReceived_t pCallback)
    {
        if(_gameId != (AppId_t)pCallback.m_nGameID) return;

        if (pCallback.m_eResult == EResult.k_EResultOK)
        {
            DebugManager.Log(LogLevel.LogInfo, "STEAM: ReceiveStats - Success");
            _statsValid = true;

            foreach (var achievement in Achievements)
            {
                SteamUserStats.GetAchievement(achievement.AchievementId, out var achievementAchieved);
                achievement.Achieved = achievementAchieved;
                achievement.DisplayName =
                    SteamUserStats.GetAchievementDisplayAttribute(achievement.AchievementId, "name");
                achievement.DisplayDescription =
                    SteamUserStats.GetAchievementDisplayAttribute(achievement.AchievementId, "desc");
            }
        }
        else
            DebugManager.Log(LogLevel.LogError, $"STEAM: ReceiveStats - Failed : {pCallback.m_eResult}");
    }

    private static void OnUserStatsStored(UserStatsStored_t pCallback)
    {
        if(_gameId != (AppId_t)pCallback.m_nGameID) return;
        
        if(pCallback.m_eResult == EResult.k_EResultOK)
            DebugManager.Log(LogLevel.LogInfo, "STEAM: StoreStats - Success");
        else if (pCallback.m_eResult == EResult.k_EResultInvalidParam)
        {
            DebugManager.Log(LogLevel.LogError, "STEAM: StoreStats - Failed to validate");
            var callback = new UserStatsReceived_t
            {
                m_eResult = EResult.k_EResultOK,
                m_nGameID = (uint)_gameId
            };
            OnUserStatsReceived(callback);
        }
        else
            DebugManager.Log(LogLevel.LogError, $"STEAM: StoreStats - Failed : {pCallback.m_eResult}");
    }
    
    private static void OnAchievementStored(UserAchievementStored_t pCallback)
    {
        if(_gameId != (AppId_t)pCallback.m_nGameID) return;

        DebugManager.Log(LogLevel.LogInfo, pCallback.m_nMaxProgress == 0
            ? $"STEAM: Achievement Unlocked : {pCallback.m_rgchAchievementName}"
            : $"STEAM: Achievement Progress : {pCallback.m_rgchAchievementName} ({pCallback.m_nCurProgress} / {pCallback.m_nCurProgress})");
    }
}