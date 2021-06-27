using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurLocText : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _textComp = null;

    private void Awake()
    {
        _textComp = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Start()
    {
        try { Messenger.AddListener(Messages.LocSettingChanged, DisplayText); } catch { }
        DisplayText();
    }

    private void OnDestroy()
    {
        try { Messenger.RemoveListener(Messages.LocSettingChanged, DisplayText); } catch { }
    }

    private void DisplayText()
    {
        _textComp.text = LocalizationHandler.LocID;
    }
}
