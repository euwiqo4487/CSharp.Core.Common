using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Class to write data to a CSV file
    /// </summary>
    public class CsvFileWriter : StreamWriter
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="stream">串流</param>
        public CsvFileWriter(Stream stream) : base(stream)
        {}
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="stream">串流</param>
        /// <param name="encoding">編碼</param>
        public CsvFileWriter(Stream stream, Encoding encoding) : base(stream, encoding)
        { }
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="filename">完整檔名</param>
        public CsvFileWriter(string filename) : base(filename)
        {}
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="filename">完整檔名</param>
        /// <param name="encoding">編碼</param>
        public CsvFileWriter(string filename, Encoding encoding) : base(filename,false, encoding)
        { }
        /// <summary>
        /// Writes a single row to a CSV file.
        /// </summary>
        /// <param name="row">The row to be written</param>
        public void WriteRow(CsvRow row)
        {
            StringBuilder builder = new StringBuilder();
            bool firstColumn = true;
            foreach (string value in row)
            {
                // Add separator if this isn't the first value
                if (!firstColumn) builder.Append(',');
                // Implement special handling for values that contain comma or quote
                // Enclose in quotes and double up any double quotes
                if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                    builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                else
                    builder.Append(value);
                firstColumn = false;
            }
            row.LineText = builder.ToString();
            WriteLine(row.LineText);
        }
    }
}
