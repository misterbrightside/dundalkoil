using System.Collections.Generic;

namespace DundalkOil
{
    class Customer
    {
        private Dictionary<string, string> customerFields;

        public Customer()
        {
            customerFields = new Dictionary<string, string>();
        }

        public void Set(string field, string value)
        {
            this.customerFields[field] = value;
        }

        public string GetID()
        {
            return this.customerFields["ID"];
        }
    }
}