using Newtonsoft.Json;
using System.Collections.Generic;

namespace DundalkOil
{
    class Customer
    {
        [JsonProperty]
        private Dictionary<string, string> customer;

        public Customer()
        {
            customer = new Dictionary<string, string>();
        }

        public void Set(string field, string value)
        {
            this.customer[field] = value;
        }

        public string GetID()
        {
            return this.customer["ID"];
        }
    }
}