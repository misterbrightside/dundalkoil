using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace DundalkOil
{
    class Invoice
    {
        [JsonProperty]
        private Dictionary<string, string> saleDoc;
        [JsonProperty]
        private List<DocItem> items;
        [JsonProperty]
        private ArrayList debtorEntries;
        [JsonProperty]
        private Customer customer;
        [JsonProperty]
        private ArrayList debtorAllocs;
        [JsonProperty]
        private double total;
        [JsonProperty]
        private double paid;
        [JsonProperty]
        private double amountFree;
        [JsonProperty]
        private double amountAllocated;

        public Invoice()
        {
            this.saleDoc = new Dictionary<string, string>();
            this.items = new List<DocItem>();
            this.debtorEntries = new ArrayList();
            this.debtorAllocs = new ArrayList();
            this.total = 0;
            this.paid = 0;
            this.amountFree = 0;
            this.amountAllocated = 0;
        }
        
        public void Set(string field, string value)
        {
            this.saleDoc[field] = value;
        }

        public void SetCustomer(Customer customer)
        {
            this.customer = customer;
        }

        public void AddItem(DocItem item)
        {
            items.Add(item);
            this.total += item.GetTotalItemCost();
        }

        public int getYearPosted()
        {
            String date = this.saleDoc["POSTDATE"];
            String[] values = date.Split('/');
            return Convert.ToInt32(values[values.Length - 1]);
        }

        public string GetID()
        {
            return this.saleDoc["ID"];
        }

        public string GetCustomerID()
        {
            return this.saleDoc["CUSTOMERID"];
        }

        public void AddDebtorEntry(DebtorEntry debtorEntry)
        {
            debtorEntries.Add(debtorEntry);
            amountFree += debtorEntry.GetAmountFree();
            amountAllocated += debtorEntry.GetAmountAllocated();
        }

        public void AddDebtorAlloc(DebtorAlloc debtorAlloc)
        {
            debtorAllocs.Add(debtorAlloc);
            paid += debtorAlloc.GetPaidAmount();
        }

        public double LeftToPay()
        {
            return this.total - this.paid;
        }

        public bool IsPaid()
        {
            return this.amountAllocated == this.total || this.amountFree == this.total;
        }

        public bool Skip()
        {
            return this.LeftToPay() <= 1 || this.IsPaid();
        }
    }
}