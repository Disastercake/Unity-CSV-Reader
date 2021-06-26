using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csv
{
    /// <summary>
    /// The parsed data read from a CSV file through CsvHandler.
    /// </summary>
    public class CsvData
    {
        public CsvData() { }
        public CsvData(List<List<string>> rawData)
        {
            RawData = rawData;
        }

        /// <summary>
        /// The raw data parsed from the CSV file.
        /// NOTE: This is not a copy.  Altering will affect this CsvData object.
        /// </summary>
        public List<List<string>> RawData { get; private set; }

        /// <summary>
        /// Returns the amount of rows in the RawData.
        /// </summary>
        public int RowCount()
        {
            if (RawData == null) return 0;
            else return RawData.Count;
        }

        /// <summary>
        /// Returns the count of the first list.
        /// </summary>
        public int ColCount()
        {
            if (RowCount() > 0)
                return RawData[0].Count;
            else
                return 0;
        }

        public void SetRawData(List<List<string>> rawData)
        {
            RawData = rawData;
        }

        /// <summary>
        /// Rows start with an index of 0.
        /// Example: What would normall be row 1 in a spreadsheet viewer would be 0 in this method.
        /// 2 in a spreadsheet would be 1 in this method.
        /// </summary>
        public bool TryGetRow(int index, out List<string> data)
        {
            data = null;

            if (RawData == null) return false;
            if (RawData.Count-1 < index) return false;

            data = RawData[index];

            return data != null;
        }

        /// <summary>
        /// Creates a new list with every value in the specified column.
        /// </summary>
        public bool TryGetColumn(int index, bool includeHeader, out List<string> data)
        {
            data = null;

            if (RawData == null) return false;
            if (RawData.Count <= 0) return false;
            if (RawData[0].Count - 1 < index) return false;

            data = new List<string>(RawData[0].Count);

            for (int i = 0; i < RawData[0].Count-1; i++)
            {
                if (i == 0 && !includeHeader)
                    continue;

                if (RawData[i] != null)
                    if (RawData[i].Count > index)
                        data.Add(RawData[i][index]);
            }

            data = RawData[index];

            return data.Count > 0;
        }
    }
}
