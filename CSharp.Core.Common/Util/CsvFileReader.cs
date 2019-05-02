using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Class to read data from a CSV file
    /// </summary>
    public class CsvFileReader : StreamReader
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="stream">串流</param>
        /// <param name="encoding">編碼</param>
        public CsvFileReader(Stream stream, Encoding encoding) : base(stream, encoding)
        {}
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="stream">串流</param>
        public CsvFileReader(Stream stream) : base(stream)
        { }
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="filename">完整檔名</param>
        public CsvFileReader(string filename) : base(filename)
        {}
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="filename">完整檔名</param>
        /// <param name="encoding">編碼</param>
        public CsvFileReader(string filename, Encoding encoding) : base(filename,encoding)
        { }
        /// <summary>
        /// Reads a row of data from a CSV file
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool ReadRow(CsvRow row)
        {
            row.LineText = ReadLine();//讀出一列資料
            if (String.IsNullOrEmpty(row.LineText)) return false;
            int pos = 0;//讀出資料列起始位置
            int rows = 0;//CsvRow集合位置
            while (pos < row.LineText.Length)
            {
                string value;
                // Special handling for quoted field
                if (row.LineText[pos] == '"')
                {   // Skip initial quote
                    pos++;
                    // Parse quoted value
                    int start = pos;
                    while (pos < row.LineText.Length)
                    {
                        // Test for quote character
                        if (row.LineText[pos] == '"')
                        {   // Found one
                            pos++;
                            // If two quotes together, keep one Otherwise, indicates end of value
                            if (pos >= row.LineText.Length || row.LineText[pos] != '"')
                            {
                                pos--;
                                break;
                            }
                        }
                        pos++;
                    }
                    value = row.LineText.Substring(start, pos - start);
                    value = value.Replace("\"\"", "\"");
                }
                else
                {   // Parse unquoted value
                    int start = pos;
                    while (pos < row.LineText.Length && row.LineText[pos] != ',') pos++;
                    value = row.LineText.Substring(start, pos - start);
                }
                // Add field to list
                if (rows < row.Count) row[rows] = value;
                else row.Add(value);
                rows++;
                // Eat up to and including next comma
                while (pos < row.LineText.Length && row.LineText[pos] != ',') pos++;
                if (pos < row.LineText.Length) pos++;
            }
            // Delete any unused items
            while (row.Count > rows) row.RemoveAt(rows);
            // Return true if any columns read
            return (row.Count > 0);
        }
    }
}
