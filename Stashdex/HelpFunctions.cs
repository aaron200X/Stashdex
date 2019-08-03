using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Stashdex;

namespace Helpfunctions {
    public static class HelpFunctions {
       public static Regex getNumber1Regex = new Regex(@"\d{1,10}");
        public static Regex getNumber2Regex = new Regex(@"to (\d{1,10})");
        public static Regex getNumberFloat = new Regex(@"\d{1,10}\.\d{1,10}");

        public static List<Item> fillList(List<Item> list) {
            List<Item> newlist = new List<Item>();
            foreach (Item item in list) {
                newlist.Add(item);
            }
            return newlist;
        }
    }
}
