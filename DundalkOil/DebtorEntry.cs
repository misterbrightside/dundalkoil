using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DundalkOil
{
    class DebtorEntry
    {
        [JsonProperty]
        private Dictionary<string, string> debtorEntryFields;

        public DebtorEntry()
        {
            debtorEntryFields = new Dictionary<string, string>();
        }

        public void Set(string field, string value)
        {
            this.debtorEntryFields[field] = value;
        }

        public string GetID()
        {
            return this.debtorEntryFields["ID"];
        }

        public double GetAmountFree()
        {
            return Convert.ToDouble(this.debtorEntryFields["R$FRGAMOUNTFREE"]);
        }

        public double GetAmountAllocated()
        {
            return Convert.ToDouble(this.debtorEntryFields["R$FRGAMOUNTALLOCATED"]);
        }
    }
}