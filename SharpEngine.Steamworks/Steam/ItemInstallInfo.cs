namespace SharpEngine.Steamworks.Steam;

/// <summary>
/// Class which represents install info of steam item
/// </summary>
public class ItemInstallInfo
{
    /// <summary>
    /// Size of Item
    /// </summary>
    public ulong SizeOnDisk { get; }

    /// <summary>
    /// Folder of Item
    /// </summary>
    public string Folder { get; }

    /// <summary>
    /// Install timestamp of Item
    /// </summary>
    public uint Timestamp { get; }

    /// <summary>
    /// Create Item Install Info
    /// </summary>
    /// <param name="sizeOnDisk">Item Size</param>
    /// <param name="folder">Item Folder</param>
    /// <param name="timestamp">Item Install Timestamp</param>
    public ItemInstallInfo(ulong sizeOnDisk, string folder, uint timestamp)
    {
        SizeOnDisk = sizeOnDisk;
        Folder = folder;
        Timestamp = timestamp;
    }
}
