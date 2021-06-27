using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTextHandler : MonoBehaviour
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
        string txt = string.Empty;

        Csv.CsvData data = new Csv.CsvData();

        if (Csv.CsvHandler.TryGetCsvDataFromPath(LocalizationHandler.CurrentLocFilePath(), in data))
        {
            List<string> row;

            if (data.TryGetRow(0, out row))
                txt = row[1];
            else
                Debug.LogError("TryGetRow");
        }
        else
        {
            Debug.LogError("TryGetCsvDataFromPath: " + LocalizationHandler.CurrentLocFilePath());
        }

        _textComp.text = txt;
    }
}
