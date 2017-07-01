using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace DundalkOil
{
    class Uploader
    {
        private Excel.Application excelApplication;
        private Excel.Workbook debtorAllocFile;
        private Excel.Workbook debtorEntryFile;
        private Excel.Workbook saleDocFile;
        private Excel.Workbook saleDocItemsFile;
        private Excel.Workbook traderFile;
        private string url;
        private SkipList skipFilePath;

        public Uploader(string url, string skipFilePath, string[] files)
        {
            this.excelApplication = new Excel.Application();
            OpenExcelFiles(files);
            this.url = url;
            this.skipFilePath = new SkipList(skipFilePath);
        }

        void OpenExcelFiles(string[] files)
        {
            this.debtorAllocFile = this.excelApplication.Workbooks.Open(@GetFileName(files, "DebtorAlloc.xlsx"));
            this.debtorEntryFile = this.excelApplication.Workbooks.Open(@GetFileName(files, "DebtorEntry.xlsx"));
            this.saleDocFile = this.excelApplication.Workbooks.Open(@GetFileName(files, "SaleDoc.xlsx"));
            this.saleDocItemsFile = this.excelApplication.Workbooks.Open(@GetFileName(files, "SaleDocItem.xlsx"));
            this.traderFile = this.excelApplication.Workbooks.Open(@GetFileName(files, "Trader.xlsx"));
        }

        private string GetFileName(string[] filenames, string filetype)
        {
            return Array.Find(filenames, f => f.Contains(filetype));
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
