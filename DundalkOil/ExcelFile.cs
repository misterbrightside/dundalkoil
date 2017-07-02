using System;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DundalkOil
{
    class ExcelFile
    {
        private Excel.Application application;
        private Excel.Worksheet worksheet;
        private Dictionary<string, int> headers;

        public ExcelFile(Excel.Application application, string filename)
        {
            this.application = application;
            this.worksheet = Open(filename);
            this.headers = GetHeaders();
        }

        public string GetValue(string column, int rowId)
        {
            return this.worksheet.Cells[rowId, this.headers[column]].Value2.ToString();
        }

        private Excel.Worksheet Open(string filename)
        {
            return this.application.Workbooks.Open(filename).Sheets[1];
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

        private string HeadersToString()
        {
            return string.Join(";", this.headers.Select(x => x.Key + '=' + x.Value));
        }
    }
}
