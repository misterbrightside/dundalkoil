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

        public InvoiceSorter(SkipList skipList, string[] files)
        {
            this.excelApplication = new Excel.Application();
            OpenExcelFiles(files);
            this.skipList = skipList;
            MessageBox.Show(this.saleDocFile.GetValue("CUSTOMERTEXT", 10));
        }

        void OpenExcelFiles(string[] files)
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

        public void CleanUp()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            excelApplication.Quit();
            Marshal.ReleaseComObject(excelApplication);
        }
    }
}
