namespace SharpEngine.Steamworks.Steam;

/// <summary>
/// Class which represents Steam Achievement
/// </summary>
/// <param name="achievementId">Achievement Id</param>
/// <param name="displayName">Achievement Name</param>
/// <param name="displayDescription">Achievement Description</param>
/// <param name="achieved">Achievement Achieved</param>
/// <param name="icon">Achievement Icon</param>
public class Achievement(
    string achievementId,
    string displayName = "",
    string displayDescription = "",
    bool achieved = false,
    int icon = 0
    )
{
    /// <summary>
    /// Id of Achievement
    /// </summary>
    public string AchievementId { get; } = achievementId;

    /// <summary>
    /// Name of Achievement
    /// </summary>
    public string DisplayName { get; set; } = displayName;

    /// <summary>
    /// Description of Achievement
    /// </summary>
    public string DisplayDescription { get; set; } = displayDescription;

    /// <summary>
    /// If Achievement is achieve
    /// </summary>
    public bool Achieved { get; set; } = achieved;

    /// <summary>
    /// Icon of Achievement
    /// </summary>
    public int Icon { get; set; } = icon;
}
