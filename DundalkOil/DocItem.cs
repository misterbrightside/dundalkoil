using System.Collections.Generic;

namespace DundalkOil
{
    class DocItem
    {
        private Dictionary<string, string> itemFields;

        public DocItem()
        {
            itemFields = new Dictionary<string, string>();
        }

        public void Set(string field, string value)
        {
            this.itemFields[field] = value;
        }
    }
}