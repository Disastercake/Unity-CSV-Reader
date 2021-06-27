using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationDropDown : MonoBehaviour
{
    TMPro.TMP_Dropdown _dropdown = null;

    private const string LOC_FILE_PREFIX = "loc_";

    private void Awake()
    {
        _dropdown = GetComponent<TMPro.TMP_Dropdown>();
        DisplayText();
        _dropdown.onValueChanged.AddListener(ValueChanged);
        try { Messenger.AddListener(Messages.LocSettingChanged, DisplayText); } catch { }
    }

    private void ValueChanged(int v)
    {
        Debug.Log("ValueChanged: " + v);
        LocalizationHandler.SetLoc(_dropdown.options[v].text);
    }

    private void OnDestroy()
    {
        _dropdown.onValueChanged.RemoveListener(ValueChanged);
        try { Messenger.RemoveListener(Messages.LocSettingChanged, DisplayText); } catch { }
    }

    private void DisplayText()
    {
        var names = GetNames();

        List<TMPro.TMP_Dropdown.OptionData> options = new List<TMPro.TMP_Dropdown.OptionData>(names.Count);

        foreach (var n in names)
        {
            options.Add(new TMPro.TMP_Dropdown.OptionData(n));
        }

        _dropdown.options = options;

        _dropdown.value = names.IndexOf(LocalizationHandler.LocID);
    }

    private List<string> GetNames()
    {
        var info = new DirectoryInfo(LocalizationHandler.LocFolderPath);

        var fileInfo = info.GetFiles();

        List<string> locales = new List<string>();

        foreach (var file in fileInfo)
        {
            var filename = Path.GetFileNameWithoutExtension(file.Name);
            if (filename.Contains(LOC_FILE_PREFIX))
            {
                var id = filename.Substring(LOC_FILE_PREFIX.Length);
                locales.Add(id);
            }
        }

        locales.Sort();

        return locales;
    }
}
