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
        private string filename;
        private Dictionary<String, String> skips;
        private StreamWriter streamWriter;

        public SkipList(string filename)
        {
            this.filename = filename;
            this.skips = File.ReadLines(this.filename).ToDictionary(i => i);
            this.streamWriter = new StreamWriter(this.filename, true);
        }

        public bool HasID(string id)
        {
            return this.skips.ContainsKey(id);
        }

        public int Size()
        {
            return this.skips.Count();
        }

        public void AddIDToSkip(string id)
        {
            if (!this.HasID(id))
            {
                Console.WriteLine("Adding " + id + " to the skip list.");
                this.streamWriter.WriteLine(id);
            }
        }

        public void CleanUp()
        {
            this.streamWriter.Close();
        }
    }
}
