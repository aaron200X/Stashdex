using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stashdex
{
    public class Socket
    {
        public int group { get; set; }
        public object attr { get; set; }
        public string sColour { get; set; }
    }

    public class Property
    {
        public string name { get; set; }
        public List<object> values { get; set; }
        public int displayMode { get; set; }
        public int type { get; set; }
    }

    public class Requirement
    {
        public string name { get; set; }
        public List<List<object>> values { get; set; }
        public int displayMode { get; set; }
    }

    public class AdditionalProperty
    {
        public string name { get; set; }
        public List<List<object>> values { get; set; }
        public int displayMode { get; set; }
        public double progress { get; set; }
    }

    public class Item
    {
        public bool verified { get; set; }
        public int w { get; set; }
        public int h { get; set; }
        public int ilvl { get; set; }
        public string icon { get; set; }
        public string league { get; set; }
        public string id { get; set; }
        public List<Socket> sockets { get; set; }
        public string name { get; set; }
        public string typeLine { get; set; }
        public bool identified { get; set; }
        public List<Property> properties { get; set; }
        public List<Requirement> requirements { get; set; }
        public List<string> explicitMods { get; set; }
        public int frameType { get; set; }
        public object category { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public string inventoryId { get; set; }
        public List<object> socketedItems { get; set; }
        public bool? support { get; set; }
        public bool? corrupted { get; set; }
        public string secDescrText { get; set; }
        public string descrText { get; set; }
        public List<string> implicitMods { get; set; }
        public List<AdditionalProperty> additionalProperties { get; set; }
        public List<string> craftedMods { get; set; }
        public List<string> utilityMods { get; set; }
        public List<string> enchantMods { get; set; }

        //public string name
        //{
        //    get { return name.Replace("<<set:MS>><<set:M>><<set:S>>", "").Trim(); }
        //    //set { name = value; }
        //}

        //public string _typeLine
        //{
        //    get { return _typeLine.Replace("<<set:MS>><<set:M>><<set:S>>", "").Trim(); }
        //    set { _typeLine = value.Replace("<<set:MS>><<set:M>><<set:S>>", "").Trim(); }
        //}

    }

    public class Stash
    {
        public int numTabs { get; set; }
        public List<Item> items { get; set; }
        public bool quadLayout { get; set; }
    }
}
