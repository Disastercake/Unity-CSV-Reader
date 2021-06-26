using System.Collections;
using System.Collections.Generic;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Linq;

namespace Csv
{
    public static class CsvHandler
    {
        /// <summary>
        /// Caches the loaded files so they don't need to be loaded again.
        /// </summary>
        private static Dictionary<string, UnityEngine.TextAsset> _loadedScripts = new Dictionary<string, UnityEngine.TextAsset>();

        /// <summary>
        /// Loads the specified file from Resources.
        /// Gets every row from the CSV file.
        /// Usage:
        /// returnedArray[0] = get entire row
        /// returnArray[0][0] = get exact row/column cell
        /// </summary>
        public static List<List<string>> ReadCsv_AllRows(string fileName)
        {
            List<List<string>> rowValues = new List<List<string>>();

            // open the file "data.csv" which is a CSV file with headers
            using (CsvReader csv = new CsvReader(
                   new StreamReader(new MemoryStream(LoadTextFileFromResources(fileName).bytes)), true))
            {
                // missing fields will not throw an exception,
                // but will instead be treated as if there was a null value
                csv.MissingFieldAction = MissingFieldAction.ReplaceByNull;
                // to replace by "" instead, then use the following action:
                //csv.MissingFieldAction = MissingFieldAction.ReplaceByEmpty;
                int fieldCount = csv.FieldCount;

#pragma warning disable 219
                string[] headers = csv.GetFieldHeaders();
#pragma warning restore 219

                int row = 0;

                while (csv.ReadNextRecord()) // Each iteration is a new row.
                {
                    rowValues.Add(new List<string>());

                    if (fieldCount > 0)
                    {
                        if (csv[0] != null)
                        {
                            for (int i = 0; i < fieldCount; i++)
                            {
                                if (csv[i] != null)
                                    rowValues[row].Add(csv[i]);
                                else
                                    rowValues[row].Add(string.Empty);
                            }
                        }
                    }

                    row++;
                }
            }

            return rowValues;
        }

        /// <summary>
        /// Loads the specified file from Resources.
        /// Cycles through the file from beginning towards the end, then stops at and caches the first occurance of the matching first row.
        /// </summary>
        public static List<string> ReadCsv_SingleRow(string fileName, string firstColumnValue)
        {
            List<string> rowValues = new List<string>();

            // open the file "data.csv" which is a CSV file with headers
            using (CsvReader csv = new CsvReader(
                   new StreamReader(new MemoryStream(LoadTextFileFromResources(fileName).bytes)), true))
            {
                // missing fields will not throw an exception,
                // but will instead be treated as if there was a null value
                csv.MissingFieldAction = MissingFieldAction.ReplaceByNull;
                // to replace by "" instead, then use the following action:
                //csv.MissingFieldAction = MissingFieldAction.ReplaceByEmpty;
                int fieldCount = csv.FieldCount;

#pragma warning disable 219
                string[] headers = csv.GetFieldHeaders();
#pragma warning restore 219

                while (csv.ReadNextRecord())
                {
                    if (fieldCount > 0)
                    {
                        if (csv[0] != null)
                        {
                            if (csv[0] == firstColumnValue)
                            {
                                for (int i = 0; i < fieldCount; i++)
                                {
                                    if (csv[i] != null)
                                        rowValues.Add(csv[i]);
                                    else
                                        rowValues.Add(string.Empty);
                                }

                                break;
                            }
                        }
                    }
                }
            }

            return rowValues;
        }

        /// <summary>
        /// Does a Resources.Load(fileName) and converts the object to a TextAsset.
        /// </summary>
        public static UnityEngine.TextAsset LoadTextFileFromResources(string fileName)
        {
            UnityEngine.TextAsset asset;

            // Don't use the database in editor mode.
            //   This fixes an issue with the file not updating because the database didn't clear.
            if (UnityEngine.Application.isPlaying)
            {
                if (_loadedScripts.TryGetValue(fileName, out asset) == false)
                {
                    asset = UnityEngine.Resources.Load(fileName) as UnityEngine.TextAsset;

                    // Note: This will add null values, but might as well.
                    _loadedScripts.Add(fileName, asset);
                }
            }
            else
            {
                asset = UnityEngine.Resources.Load(fileName) as UnityEngine.TextAsset;
            }

            if (asset == null)
                UnityEngine.Debug.LogError("Could not load file as TextAsset (from Resources).  File name: " + fileName);

            return asset;
        }
    }
}
