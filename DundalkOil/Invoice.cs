using System;
using System.Collections;
using System.Collections.Generic;

namespace DundalkOil
{
    class Invoice
    {
        private Dictionary<string, string> saleDoc;
        private ArrayList items;
        private ArrayList debtorEntries;
        private Customer customer;
        private ArrayList debtorAllocs;

        public Invoice()
        {
            this.saleDoc = new Dictionary<string, string>();
            this.items = new ArrayList();
            this.debtorEntries = new ArrayList();
            this.debtorAllocs = new ArrayList();
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
    }
}