using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using help = Helpfunctions.HelpFunctions;

namespace Stashdex {
    public class Socket {
        public int group { get; set; }
        public object attr { get; set; }
        public string sColour { get; set; }
    }

    public class Property {
        public string name { get; set; }
        public List<object> values { get; set; }
        public int displayMode { get; set; }
        public int type { get; set; }
    }

    public class Requirement {
        public string name { get; set; }
        public List<List<object>> values { get; set; }
        public int displayMode { get; set; }
    }

    public class AdditionalProperty {
        public string name { get; set; }
        public List<List<object>> values { get; set; }
        public int displayMode { get; set; }
        public double progress { get; set; }
    }

    public class Item {
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
        public List<string> filterMods = new List<string>();
        public List<string> allMods = new List<string>();
        public Dictionary<string, object> allModsDic = new Dictionary<string, object>();
        public bool isFiltered { get; set; }
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


        /// <summary>
        /// fills the extrafields that I threw in the Item Class.
        /// </summary>
        public void fillEverything() {
            fillAllMods();
            getFilterMods();
            if (filterMods != null) allMods?.AddRange(filterMods);
            fillAllDics();
        }

        /// <summary>
        /// Puts all mods in a new mod to easier work
        /// </summary>
        public void fillAllMods() {
            if (implicitMods != null) allMods?.AddRange(implicitMods);
            if (explicitMods != null) allMods?.AddRange(explicitMods);
            if (craftedMods != null) allMods?.AddRange(craftedMods);
            if (utilityMods != null) allMods?.AddRange(utilityMods);
            if (enchantMods != null) allMods?.AddRange(enchantMods);
        }

        /// <summary>
        /// creates the mods in form of a dictionary to easier handling the numbers
        /// </summary>
        public void fillAllDics() {
            object value1 = new object();
            object value2 = new object();
            object valueTogether = new object();
            //object oldValue;
            string key;
            foreach (string mod in allMods) {
                value1 = new object();
                value2 = new object();
                valueTogether = new object();

                if (help.getNumber2Regex.IsMatch(mod)) {
                    value2 = help.getNumber2Regex.Match(mod).Groups[1].Value;
                }
                key = mod;
                if (help.getNumberFloat.IsMatch(mod)) {
                    //FLOATS
                    value1 = help.getNumberFloat.Match(mod).Value;
                    key = key.Replace(value1.ToString(), "#");
                } else {
                    //NUMBERS
                    //Replace the numbers with the #
                    value1 = help.getNumber1Regex.Match(mod).Value;
                    
                    if (help.getNumber1Regex.IsMatch(value2.ToString())) {
                        key = key.Replace(value2.ToString(), "#");
                        value1 = help.getNumber1Regex.Match(mod).Value; ;
                    }
                    if (help.getNumber1Regex.IsMatch(value1.ToString())) {
                        key = key.Replace(value1.ToString(), "#");
                    }
                }
                if (help.getNumber1Regex.IsMatch(value1.ToString()) && help.getNumber2Regex.IsMatch(value2.ToString())) {
                    valueTogether = (Convert.ToInt16(value1) + Convert.ToInt16(value2) / 2);
                } else if (help.getNumber1Regex.IsMatch(value1.ToString())) {
                    try {
                        valueTogether = Convert.ToInt16(value1);
                    } catch (Exception) {
                        valueTogether = Convert.ToDouble(value1);
                    }
                    
                }

                if (allModsDic.ContainsKey(key)) {
                    //TODO - rechne die Werte zusammen
                    //allModsDic[key]
                } else {
                    allModsDic.Add(key, valueTogether);
                }
            }
        }

        public void getFilterMods() {
            int elementalResistance = 0;
            int allResistance = 0;
            if (allMods.Any()) {
                foreach (string mod in allMods) {
                    //Ele Resis
                    if (Regex.IsMatch(mod, "to (Fire|Cold|Lightning).*Resistance")) {
                        elementalResistance += Convert.ToInt16(help.getNumber1Regex.Match(mod).Value);
                    }
                    //All Resis
                    if (Regex.IsMatch(mod, "to (Fire|Cold|Lightning|Chaos).*Resistance")) {
                        allResistance += Convert.ToInt16(help.getNumber1Regex.Match(mod).Value);
                    }
                }
                if (elementalResistance >= 0) {
                    filterMods.Add($"+{elementalResistance}% Elemental Resistance");
                }

            }
        }

    }


    public class Stash {
        public int numTabs { get; set; }
        public int tabNumber { get; set; }
        public bool selected { get; set; }
        public List<Item> items { get; set; }
        public List<Item> filteredItems = new List<Item>();
        public bool quadLayout { get; set; }

    }


}
