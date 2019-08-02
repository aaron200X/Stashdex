using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace Stashdex {
    public class FilterOptions {
        List<Filter> filter;
    }

    public class filteredItems {
        //Stash stash;
        

        void getItem() {

        }
    }

    public class Filter {
        public string filtername { get; set; }
        int value { get; set; }
    }

    /// <summary>
    /// Mainfunction that gives all Items back, that are found
    /// </summary>
    public class SearchFunctions {
        public static void search(Filter filter) {
            foreach (Stash stash in Stashes.stashes) {
                foreach (var item in stash.items) {
                    item.isFiltered = false;
                    foreach (string mods in item.allModsDic.Keys) {
                        if (mods.Contains(filter.filtername)) {
                            //stash.filteredItems.Add(item);
                            item.isFiltered = true;
                        }
                    }
                }
            }

            //TODO FIND THE REAL WINDOW AND RESET THE DISPLAYED ITEMS
            MainWindow window = new MainWindow();
            
            window.displayAllItems(); 
        }

        public static string giveBackFilterBox(FilterOptionsWindow filterWindow) {
            ListBoxItem X = (ListBoxItem)filterWindow.filterBox.SelectedValue;
            return X.Content.ToString();
        }

        public static void fillFilterBox(FilterOptionsWindow filterWindow) {
            Stashes.getAbsolutelyAllMods();

            foreach (string mod in Stashes.absolutelyAllMods) {
                ListBoxItem item = new ListBoxItem();
                item.Content = mod;
                filterWindow.filterBox.Items.Add(item);
            }

        }

    }

}
