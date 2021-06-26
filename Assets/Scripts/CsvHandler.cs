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
        /// Attempts to read a CSV file and cache the data in a CsvData object.
        /// Use this method to reuse CsvData objects so they don't need to be reallocated.
        /// </summary>
        public static bool TryGetCsvData(string fileName, in CsvData data)
        {
            List<List<string>> rawdata;

            if (TryGetCsvStrings(fileName, out rawdata))
            {
                if (rawdata.Count > 0)
                    data.SetRawData(rawdata);
            }

            return data != null;
        }

        /// <summary>
        /// Loads the specified file from Resources.
        /// Gets every row from the CSV file.
        /// 
        /// Usage:
        /// returnedArray[row] = get entire row
        /// returnedArray[row][column] = get exact cell
        /// </summary>
        public static bool TryGetCsvStrings(string fileName, out List<List<string>> data)
        {
            data = new List<List<string>>();

            UnityEngine.TextAsset textAsset;

            if (!TryLoadTextFileFromResources(fileName, out textAsset))
                return false;

            // open the file "data.csv" which is a CSV file with headers
            using (CsvReader csv = new CsvReader(
                   new StreamReader(new MemoryStream(textAsset.bytes)), true))
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
                    data.Add(new List<string>());

                    if (fieldCount > 0)
                    {
                        if (csv[0] != null)
                        {
                            for (int i = 0; i < fieldCount; i++)
                            {
                                if (csv[i] != null)
                                    data[row].Add(csv[i]);
                                else
                                    data[row].Add(string.Empty);
                            }
                        }
                    }

                    row++;
                }
            }

            return data.Count > 0;
        }

        /// <summary>
        /// Loads the specified file from Resources.
        /// Cycles through the file from beginning towards the end, then stops at and caches the first occurance of the matching value in the first column.
        /// </summary>
        public static bool TryGetCsvStrings(string fileName, string firstColumnValue, out List<string> rowValues)
        {
            rowValues = new List<string>();

            UnityEngine.TextAsset textAsset;

            if (!TryLoadTextFileFromResources(fileName, out textAsset))
                return false;

            // open the file "data.csv" which is a CSV file with headers
            using (CsvReader csv = new CsvReader(
                   new StreamReader(new MemoryStream(textAsset.bytes)), true))
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

            return rowValues.Count > 0;
        }

        /// <summary>
        /// Does a Resources.Load(fileName) and converts the object to a TextAsset.
        /// </summary>
        private static bool TryLoadTextFileFromResources(string fileName, out UnityEngine.TextAsset asset)
        {
            asset = null;

            // Don't use the database in editor mode.
            //   This fixes an issue with the file not updating because the database didn't clear.
            if (UnityEngine.Application.isPlaying)
            {
                if (_loadedScripts.TryGetValue(fileName, out asset) == false)
                {
                    var loadedObject = UnityEngine.Resources.Load(fileName);

                    if (loadedObject == null)
                    {
                        UnityEngine.Debug.LogError("File did not exist: " + fileName);
                        return false;
                    }

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

            return asset != null;
        }
    }
}
