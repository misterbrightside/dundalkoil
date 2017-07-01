using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DundalkOil
{
    class SkipList
    {
        string filename;
        Dictionary<String, String> skips;

        public SkipList(string filename)
        {
            this.filename = filename;
            this.skips = File.ReadLines(this.filename).ToDictionary(i => i);
        }

        public bool HasID(string id)
        {
            return this.skips.ContainsKey(id);
        }

        public int Size()
        {
            return this.skips.Count();
        }
    }
}
