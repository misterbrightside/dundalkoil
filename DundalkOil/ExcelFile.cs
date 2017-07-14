using System;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DundalkOil
{
    class ExcelFile
    {
        private Excel.Application application;
        private Excel.Worksheet worksheet;
        private Excel.Workbook workbook;
        private Dictionary<string, int> headers;
        private Excel.Range range;
        private int columnCount;
        private int rowCount;

        public ExcelFile(Excel.Application application, string filename)
        {
            this.application = application;
            this.worksheet = Open(filename);
            this.headers = GetHeaders();
            this.range = this.worksheet.UsedRange;
            this.columnCount = this.range.Columns.Count;
            this.rowCount = this.range.Rows.Count;
        }

        public string GetValue(string column, int rowId)
        {
            return this.GetCellValue(rowId, this.headers[column]);
        }

        private Excel.Worksheet Open(string filename)
        {
            this.workbook = this.application.Workbooks.Open(filename);
            return workbook.Sheets[1];
        }

        public string[] GetAllForColumn(string columnName)
        {
            int columnIndex = headers[columnName];
            Excel.Range columnRange = this.range.Columns[columnIndex];
            Array values = (Array)columnRange.Cells.Value2;
            return values.OfType<object>().Select(o => o.ToString()).ToArray();
        }

        private Dictionary<string, int> GetHeaders()
        {
            int columnCount = this.worksheet.UsedRange.Columns.Count;
            Excel.Range usedHeaders = this.worksheet.Rows[1];
            Dictionary<string, int> headers = new Dictionary<string, int>();
            for (int i = 1; i <= columnCount; i++)
            {
                string header = usedHeaders.Cells[1, i].Value2.ToString();
                headers.Add(header, i);
            }
            return headers;
        }

        private string GetCellValue(int rowId, int columnId)
        {
            return this.range.Cells[rowId, columnId].Value2.ToString();
        }

        private string HeadersToString()
        {
            return string.Join(";", this.headers.Select(x => x.Key + '=' + x.Value));
        }

        public int ColumnCount()
        {
            return this.columnCount;
        }

        public int RowCount()
        {
            return this.rowCount;
        }

        public Excel.Range Range()
        {
            return this.range;
        }

        public Excel.Worksheet sheet()
        {
            return worksheet;
        }

        public Dictionary<string, int> Headers()
        {
            return headers;
        }

        public void Close()
        {
            this.workbook.Close();
        }
    }
}
