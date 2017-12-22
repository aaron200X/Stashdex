using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stashdex.Stasht
{
    public class Item
    {
        public bool verified { get; set; }
        public string w { get; set; }
        public string h { get; set; }
        public int ilevel { get; set; }
        public string icon { get; set; }
        public string league { get; set; }
        public string id { get; set; }
        public Sockets[] sockets { get; set; }
        public string name { get; set; }
        public string typeline { get; set; }
        public bool identified { get; set; }
        //public Properties[] properties { get; set; }
        public string frametype { get; set; }
        //public Category category { get; set; }
        public string x { get; set; }
        public string y { get; set; }
        public string inventoryId { get; set; }
        //public string socketedItems { get; set; }



    }

    public class Sockets
    {
        public string group { get; set; }
        public string attr { get; set; }
        public string scolour { get; set; }
    }

    public class Properties
    {
        public string name { get; set; }
        public string[] values { get; set; }
        public string displayMode { get; set; }
        public string type { get; set; }
    }

    public class Category
    {
        public string Weapons { get; set; }
    }
}
