using System;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Linq;
using System.Collections.Generic;

namespace DundalkOil
{
    class InvoiceSorter
    {
        private Excel.Application excelApplication;
        private ExcelFile debtorAllocFile;
        private ExcelFile debtorEntryFile;
        private ExcelFile saleDocFile;
        private ExcelFile saleDocItemsFile;
        private ExcelFile traderFile;
        private SkipList skipList;
        private string[] files;

        public InvoiceSorter(SkipList skipList, string[] files)
        {
            this.excelApplication = new Excel.Application();
            this.files = files;
            this.skipList = skipList;
        }

        public void Test()
        {
            var x = GetData();
            Console.WriteLine(x.Count);
        }

        public void OpenExcelFiles()
        {
            this.debtorAllocFile = new ExcelFile(this.excelApplication, this.GetFile(files, "DebtorAlloc.xlsx"));
            this.debtorEntryFile = new ExcelFile(this.excelApplication, this.GetFile(files, "DebtorEntry.xlsx"));
            this.saleDocFile = new ExcelFile(this.excelApplication, this.GetFile(files, "SaleDoc.xlsx"));
            this.saleDocItemsFile = new ExcelFile(this.excelApplication, this.GetFile(files, "SaleDocItem.xlsx"));
            this.traderFile = new ExcelFile(this.excelApplication, this.GetFile(files, "Trader.xlsx"));
        }

        private string GetFile(string[] filenames, string filetype)
        {
            return @Array.Find(filenames, f => f.Contains(filetype));
        }

        public Dictionary<String, Dictionary<String, String>> GetData()
        {
            Dictionary<String, Dictionary<String, String>> result = new Dictionary<string, Dictionary<string, string>>();

            var invoiceRows = this.saleDocFile.RowCount();
            var saleDocData = GetDataArray(this.saleDocFile);
            for (int i = 1; i < invoiceRows; i++)
            {
                var values = new Dictionary<String, String>();
                values["CUSTOMERID"] = saleDocData[i, this.saleDocFile.Headers()["CUSTOMERID"]].ToString();

                result[saleDocData[i, this.saleDocFile.Headers()["ID"]].ToString()] = values;
            }

            var saleDocItemsData = GetDataArray(this.saleDocItemsFile);
            var debtorAllocData = GetDataArray(this.debtorAllocFile);
            var debtorEntryData = GetDataArray(this.debtorEntryFile);
            var traderData = GetDataArray(this.traderFile);


            return result;
        }
    
        public object[,] GetDataArray(ExcelFile file)
        {
            int rows = file.RowCount();
            int columns = file.ColumnCount();
            var range = file.sheet().Range[file.sheet().Cells[2, 1], file.sheet().Cells[rows, columns]];
            object[,] data = range.Cells.Value2;
            range = null;
            return data;
        }

        public void BuildInvoices()
        {
            string[] saleDocIds = saleDocFile.GetAllForColumn("ID");
            var idsToProcess = saleDocIds.Where(i => !skipList.HasID(i));
            var invoices = idsToProcess.Select(i => BuildInvoiceObject(i)).ToArray();
        }

        public Object BuildInvoiceObject(string id)
        {
            return null;
        }

        public void CleanUp()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            excelApplication.Quit();
            Marshal.ReleaseComObject(excelApplication);
        }
    }
}
