using System.Collections.Generic;

/// <summary>
/// Holds all information relevant for the localization scripts to function.
/// </summary>
public static class LocalizationHandler
{
    public static string LocID { get; private set; } = "en";
    public static void SetLoc(string id)
    {
        if (LocID == id) return;

        LocID = id;
        try { Messenger.Broadcast(Messages.LocSettingChanged); } catch { }
    }

    public static string LocFolderPath { get; private set; } = string.Empty;

    public static void SetLocFilePath(string path)
    {
        LocFolderPath = path;
        try { Messenger.Broadcast(Messages.LocSettingChanged); } catch { }
    }

    public static string CurrentLocFileName()
    {
        return string.Format("loc_{0}.csv", LocalizationHandler.LocID);
    }

    public static string CurrentLocFilePath()
    {
        return System.IO.Path.Combine(LocFolderPath, CurrentLocFileName());
    }
}