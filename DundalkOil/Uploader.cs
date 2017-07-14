using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace DundalkOil
{
    class Uploader
    {

        private string url;
        private InvoiceSorter invoiceSorter;
        private MemoryStream memoryStream;
        private DataContractJsonSerializer jsonSerializer;

        public Uploader(string url, string skipFilePath, string[] files)
        {
            this.url = url;
            this.memoryStream = new MemoryStream();
            this.jsonSerializer = new DataContractJsonSerializer(typeof(Invoice));
            this.invoiceSorter = new InvoiceSorter(new SkipList(skipFilePath), files);
        }

        public void Init()
        {
            this.invoiceSorter.OpenExcelFiles();
            Dictionary<string, Invoice> invoices = this.invoiceSorter.BuildInvoices();

            List<String> jsonInvoices = new List<String>(invoices.Count);
            foreach (Invoice invoice in invoices.Values)
            {
                if (invoice.Skip())
                {
                    invoiceSorter.AddIDToSkipFile(invoice.GetID());
                }
                else
                {
                    String json = JsonConvert.SerializeObject(invoice);
                    jsonInvoices.Add(json);
                }
            }
            this.CleanUp();
            System.IO.File.WriteAllLines("results.txt", jsonInvoices.Select(json => json));
        }

        public void CleanUp()
        {
            this.invoiceSorter.CleanUp();
        }
    }
}