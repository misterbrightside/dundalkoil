using System.Collections.Generic;

namespace DundalkOil
{
    class DebtorEntry
    {
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
    }
}