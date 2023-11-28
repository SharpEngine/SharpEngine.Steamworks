namespace SharpEngine.Steamworks.Steam;

/// <summary>
/// Class which represents install info of steam item
/// </summary>
/// <param name="sizeOnDisk">Item Size</param>
/// <param name="folder">Item Folder</param>
/// <param name="timestamp">Item Install Timestamp</param>
public class ItemInstallInfo(ulong sizeOnDisk, string folder, uint timestamp)
{
    /// <summary>
    /// Size of Item
    /// </summary>
    public ulong SizeOnDisk { get; } = sizeOnDisk;

    /// <summary>
    /// Folder of Item
    /// </summary>
    public string Folder { get; } = folder;

    /// <summary>
    /// Install timestamp of Item
    /// </summary>
    public uint Timestamp { get; } = timestamp;
}
