using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Listens for Messages.DirectoryChosen 
/// </summary>
[DisallowMultipleComponent]
public class SelectedDirectoryDisplay : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _textComp = null;
    private void Awake()
    {
        _textComp = GetComponent<TMPro.TextMeshProUGUI>();
        _textComp.text = string.Empty;

        DirectoryChosen();

        try { Messenger.AddListener(Messages.LocSettingChanged, DirectoryChosen); } catch { }
    }

    private void OnDestroy()
    {
        try { Messenger.RemoveListener(Messages.LocSettingChanged, DirectoryChosen); } catch { }
    }

    private void DirectoryChosen()
    {
        _textComp.text = LocalizationHandler.LocFolderPath;
    }
}
