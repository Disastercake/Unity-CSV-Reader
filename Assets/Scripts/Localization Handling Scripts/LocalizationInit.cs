using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationInit : MonoBehaviour
{
    private const string DEFAULT_LOC_FOLDER_NAME = "Localization Files";
    private readonly string[] DEFAULT_FILES = new string[] { "loc_en", "loc_es", "loc_jp" };

    private void Awake()
    {
        // Do this in start
        var path = Path.Combine(ReplaceBackslashWithSeparator(Application.persistentDataPath), DEFAULT_LOC_FOLDER_NAME);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        _CopyDefaultLocFiles(path);

        LocalizationHandler.SetLocFilePath(path);

        Done();
    }

    /// <summary>
    /// For some reason the persistent data path does not have the correct path separator.
    /// </summary>
    /// <returns></returns>
    private string ReplaceBackslashWithSeparator(string path)
    {
        path = path.Replace('/', Path.DirectorySeparatorChar);
        return path;
    }

    private void Done()
    {
        try { Messenger.Broadcast(Messages.LocSettingChanged); } catch { }
    }

    /// <summary>
    /// Copy the default loc files to the specified path.
    /// </summary>
    private void _CopyDefaultLocFiles(string path)
    {
        TextAsset ta;

        foreach (var fileName in DEFAULT_FILES)
        {
            if (Csv.CsvHandler.TryLoadTextFileFromResources(fileName, out ta))
            {
                StreamWriter writer = new StreamWriter(Path.Combine(path, fileName + ".csv"), false);
                writer.WriteLine(ta.text);
                writer.Close();
            }
        }
    }
}
