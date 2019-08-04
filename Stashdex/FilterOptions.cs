using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace Stashdex {

    public class Filter {
        public string filtername { get; set; }
        public int minValue = -99999;
        public int maxValue = 99999;

    }

    /// <summary>
    /// Mainfunction that gives all Items back, that are found
    /// </summary>
    public class SearchFunctions {
        static MainWindow win = (MainWindow)App.Current.MainWindow;
        //static List<Item> filteredItems = new List<Item>();
        static List<Item> lastFilteredItems = new List<Item>();
        static List<Item> currentfilteredItems = new List<Item>();

        public static void search(List<Filter> filterlist) {
            lastFilteredItems = new List<Item>();
            currentfilteredItems = new List<Item>();
            //TODO - Arbeite mit FIlterliste
            foreach (var filter in filterlist) {
                lastFilteredItems.Clear();
                lastFilteredItems = Helpfunctions.HelpFunctions.fillList(currentfilteredItems);
                currentfilteredItems.Clear();
                if (filter.filtername != "") {
                    foreach (Stash stash in Stashes.stashes) {
                        foreach (var item in stash.items) {
                            item.isFiltered = false;
                            foreach (string mods in item.allModsDic.Keys) {
                                //foreach
                                if (mods == filter.filtername) {
                                    //stash.filteredItems.Add(item);
                                    if ((filter.minValue != -99999 || filter.maxValue != 99999)) {
                                        //Todo Working on other content Types
                                        //min or Max value filled
                                        if (filter.maxValue >= Convert.ToInt16(item.allModsDic[mods]) && filter.minValue <= Convert.ToInt16(item.allModsDic[mods])) {
                                            if (filterlist.IndexOf(filter) == 0) {
                                                lastFilteredItems.Add(item);
                                                currentfilteredItems.Add(item);
                                            }  else if (lastFilteredItems.Contains(item)) {
                                                currentfilteredItems.Add(item);
                                            }
                                        }
                                    } else {
                                        //min and max value not filled
                                        if (filterlist.IndexOf(filter) == 0) {
                                            lastFilteredItems.Add(item);
                                            currentfilteredItems.Add(item);
                                        } else if (lastFilteredItems.Contains(item)) {
                                            currentfilteredItems.Add(item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (Item item in currentfilteredItems) {
                item.isFiltered = true;
            }

            win.displayAllItems();
        }



    }

}
