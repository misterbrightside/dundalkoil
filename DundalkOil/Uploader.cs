using System;
using System.Windows;

namespace DundalkOil
{
    class Uploader
    {

        private string url;
        private InvoiceSorter invoiceSorter;

        public Uploader(string url, string skipFilePath, string[] files)
        {
            this.url = url;
            this.invoiceSorter = new InvoiceSorter(new SkipList(skipFilePath), files);
        }

        public void Init()
        {
            this.invoiceSorter.OpenExcelFiles();
            this.invoiceSorter.Test();
            this.invoiceSorter.BuildInvoices();
        }

        public void CleanUp()
        {
            this.invoiceSorter.CleanUp();
        }
    }
}