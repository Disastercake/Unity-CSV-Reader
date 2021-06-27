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

        DirectoryChosen(LocalizationHandler.LocFolderPath);

        try { Messenger<string>.AddListener(Messages.DirectoryChosen, DirectoryChosen); } catch { }
    }

    private void OnDestroy()
    {
        try { Messenger<string>.RemoveListener(Messages.DirectoryChosen, DirectoryChosen); } catch { }
    }

    private void DirectoryChosen(string directory)
    {
        _textComp.text = directory;
    }
}
