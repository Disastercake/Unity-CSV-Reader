/// <summary>
/// Helper class to easily track messages used with the Messenger system.
/// </summary>
public static class Messages
{
    /// <summary>
    /// Broadcast when a directory for the CSV files has been chosen.
    /// Passes: string (The directory that was chosen).
    /// </summary>
    public const string DirectoryChosen = "DirectoryChosen";

    /// <summary>
    /// Broadcast when the localization setting is changed to a different locale.
    /// </summary>
    public const string LocSettingChanged = "LocSettingChanged";
}
