using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Net.Http;

namespace DundalkOil
{
    class Uploader
    {

        private string url;
        private InvoiceSorter invoiceSorter;
        private MemoryStream memoryStream;
        private DataContractJsonSerializer jsonSerializer;
        private static readonly HttpClient client = new HttpClient();

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
            DateTime thisYear = DateTime.Now;
            foreach (Invoice invoice in invoices.Values)
            {
                if (invoice.Skip() || invoice.getYearPosted() < thisYear.Year - 2)
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

            //var content = await client.PostAsync(this.url + "wp-json/beakon-invoices/v1/invoices");
        }

        public void CleanUp()
        {
            this.invoiceSorter.CleanUp();
        }
    }
}