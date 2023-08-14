namespace SharpEngine.Steamworks.Steam;

/// <summary>
/// Class which represents Steam Achievement
/// </summary>
public class Achievement
{
    /// <summary>
    /// Id of Achievement
    /// </summary>
    public string AchievementId { get; }
    
    /// <summary>
    /// Name of Achievement
    /// </summary>
    public string DisplayName { get; set; }
    
    /// <summary>
    /// Description of Achievement
    /// </summary>
    public string DisplayDescription { get; set; }
    
    /// <summary>
    /// If Achievement is achieve
    /// </summary>
    public bool Achieved { get; set; }
    
    /// <summary>
    /// Icon of Achievement
    /// </summary>
    public int Icon { get; set; }

    /// <summary>
    /// Create Achievement
    /// </summary>
    /// <param name="achievementId">Achievement Id</param>
    /// <param name="displayName">Achievement Name</param>
    /// <param name="displayDescription">Achievement Description</param>
    /// <param name="achieved">Achievement Achieved</param>
    /// <param name="icon">Achievement Icon</param>
    public Achievement(string achievementId, string displayName = "", string displayDescription = "",
        bool achieved = false, int icon = 0)
    {
        AchievementId = achievementId;
        DisplayName = displayName;
        DisplayDescription = displayDescription;
        Achieved = achieved;
        Icon = icon;
    }
}