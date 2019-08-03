using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace Stashdex {
    //public class FilterOptions {
    //    List<Filter> filter;
    //}

    //public class filteredItems {
    //    //Stash stash;


    //    //void getItem() {

    //    //}
    //}

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

        public static void search(Filter filter) {

            if (filter.filtername != "") {
                foreach (Stash stash in Stashes.stashes) {
                    foreach (var item in stash.items) {
                        item.isFiltered = false;
                        foreach (string mods in item.allModsDic.Keys) {
                            if (mods == filter.filtername) {
                                //stash.filteredItems.Add(item);
                                if ((filter.minValue != -99999 || filter.maxValue != 99999)) {
                                    //Todo Working on other content Types
                                    if (filter.maxValue >= Convert.ToInt16(item.allModsDic[mods]) && filter.minValue <= Convert.ToInt16(item.allModsDic[mods])) {
                                        item.isFiltered = true;
                                    }                                   
                                } else {
                                    item.isFiltered = true;
                                }
                            }
                        }
                    }
                }
            }
            win.displayAllItems();
        }

     

    }

}
