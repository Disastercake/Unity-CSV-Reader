using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCsvGetter : MonoBehaviour
{
    // Start is called before the first frame update.
    void Start()
    {
        Csv.CsvData csv = new Csv.CsvData();

        if (Csv.CsvHandler.TryGetCsvData("ItemData_Fish", in csv))
        {
            var str = new System.Text.StringBuilder();

            for (int i1 = 0; i1 < csv.RowCount(); i1++)
            {
                List<string> row;

                if(csv.TryGetRow(i1, out row))
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
