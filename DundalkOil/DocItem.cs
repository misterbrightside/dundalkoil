using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace DundalkOil
{
    [DataContract]
    class DocItem
    {
        [DataMember]
        private Dictionary<string, string> itemFields;

        public DocItem()
        {
            itemFields = new Dictionary<string, string>();
        }

        public void Set(string field, string value)
        {
            this.itemFields[field] = value;
        }

        public double GetTotalItemCost()
        {
            double value = 0;
            try
            {
                value = Convert.ToDouble(this.itemFields["FRGAMOUNTVATEXC"]) + Convert.ToDouble(this.itemFields["FRGVATAMOUNT"]);
            }
            catch(FormatException)
            {
                MessageBox.Show("Unable to convert " + this.itemFields["FRGAMOUNTVATEXC"] + "/" + this.itemFields["FRGVATAMOUNT"] + " to a float.");
            }
            return value;
        }
    }
}