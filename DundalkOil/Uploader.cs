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
            this.invoiceSorter.CleanUp();
        }
    }
}
