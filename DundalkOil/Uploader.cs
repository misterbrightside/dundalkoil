using System;
using System.Collections.Generic;
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
            Dictionary<string, Invoice> invoices = this.invoiceSorter.BuildInvoices();
            this.CleanUp();
            foreach (Invoice invoice in invoices.Values)
            {
                Console.WriteLine("is paid: " + invoice.IsPaid() + " left to pay: " + invoice.LeftToPay());
            }
        }

        public void CleanUp()
        {
            this.invoiceSorter.CleanUp();
        }
    }
}