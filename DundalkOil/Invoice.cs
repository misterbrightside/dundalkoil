using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace DundalkOil
{
    class Invoice
    {
        private Dictionary<string, string> saleDoc;
        private List<DocItem> items;
        private ArrayList debtorEntries;
        private Customer customer;
        private ArrayList debtorAllocs;
        private double total;
        private double paid;
        private double amountFree;

        public Invoice()
        {
            this.saleDoc = new Dictionary<string, string>();
            this.items = new List<DocItem>();
            this.debtorEntries = new ArrayList();
            this.debtorAllocs = new ArrayList();
            this.total = 0;
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
        }

        public void AddDebtorAlloc(DebtorAlloc debtorAlloc)
        {
            debtorAllocs.Add(debtorAlloc);
        }

        public double Total()
        {
            return this.total;
        }
    }
}