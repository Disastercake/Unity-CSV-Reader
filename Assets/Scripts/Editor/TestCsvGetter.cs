using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class TestCsvGetter : EditorWindow
{
    string fileName = "ItemData_Fish";

    // Add menu item named "CSV" to the Assets menu
    [MenuItem("Assets/CSV/Test CSV Reader")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(TestCsvGetter));
    }

    void OnGUI()
    {
        GUILayout.Label("Test CSV Reader", EditorStyles.boldLabel);
        fileName = EditorGUILayout.TextField("CSV file name", fileName);

        if (GUILayout.Button("Try to read CSV"))
        {
            Test(fileName);
        }
    }

    void Test(string fileName)
    {
        Csv.CsvData csv = new Csv.CsvData();

        if (Csv.CsvHandler.TryGetCsvData(fileName, in csv))
        {
            var str = new System.Text.StringBuilder();

            for (int i1 = 0; i1 < csv.RowCount(); i1++)
            {
                List<string> row;

                if (csv.TryGetRow(i1, out row))
                {
                    str.AppendLine();
                    str.Append(string.Format("Row {0}: ", i1));

                    for (int i2 = 0; i2 < row.Count; i2++)
                    {
                        str.Append(row[i2]);
                        if (i2 < row.Count - 1)
                            str.Append(", ");
                    }
                }
            }

            Debug.Log(str);
        }
        else
        {
            Debug.LogError("Failed");
        }
    }
}