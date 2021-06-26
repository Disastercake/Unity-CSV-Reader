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
        /// </summary>
        public List<List<string>> RawData { get; private set; }

        public void SetRawData(List<List<string>> rawData)
        {
            RawData = rawData;
        }
    }
}
