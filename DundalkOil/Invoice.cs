using System;
using System.Collections;
using System.Collections.Generic;

namespace DundalkOil
{
    class Invoice
    {
        private Dictionary<string, string> saleDoc;
        private ArrayList items;

        public Invoice()
        {
            this.saleDoc = new Dictionary<string, string>();
            this.items = new ArrayList();
        }
        
        public void Set(string field, string value)
        {
            this.saleDoc[field] = value;
        }

        public void AddItem(DocItem item)
        {
            items.Add(item);
        }

        public string GetID()
        {
            return this.saleDoc["ID"];
        }
    }
}