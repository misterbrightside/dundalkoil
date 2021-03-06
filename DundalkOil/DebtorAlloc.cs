﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DundalkOil
{
    class DebtorAlloc
    {
        [JsonProperty]
        private Dictionary<string, string> debtorAllocFields;

        public DebtorAlloc()
        {
            this.debtorAllocFields = new Dictionary<string, string>();
        }

        internal void Set(string field, string value)
        {
            this.debtorAllocFields[field] = value;
        }

        public double GetPaidAmount()
        {
            return Convert.ToDouble(this.debtorAllocFields["FRGAMOUNT"]);
        }
    }
}