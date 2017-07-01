using Microsoft.Office.Interop.Excel;
using System;
using System.Windows;

namespace DundalkOil
{
    class Uploader
    {
        private string debtorAllocFile;
        private string debtorEntryFile;
        private string saleDocFile;
        private string saleDocItemsFile;
        private string traderFile;
        private string url;
        private string skipFilePath;

        public Uploader(string url, string skipFilePath, string[] files)
        {
            //.excelApplication = new Application();
            OpenExcelFiles(files);
            this.url = url;
            this.skipFilePath = skipFilePath;
        }

        void OpenExcelFiles(string[] files)
        {
            this.debtorAllocFile = GetFileName(files, "DebtorAlloc.xlsx");
            this.debtorEntryFile = GetFileName(files, "DebtorEntry.xlsx");
            this.saleDocFile = GetFileName(files, "SaleDoc.xlsx");
            this.saleDocItemsFile = GetFileName(files, "SaleDocItem.xlsx");
            this.traderFile = GetFileName(files, "Trader.xlsx");
            MessageBox.Show(this.debtorAllocFile + " " + this.debtorEntryFile + " " + this.saleDocFile + " " + this.saleDocItemsFile + " " + this.traderFile);
        }

        private string GetFileName(string[] filenames, string filetype)
        {
            return Array.Find(filenames, f => f.Contains(filetype));
        }
    }
}
