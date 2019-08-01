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

    public class Filter {
        string Filtername;
        int value;
    }

    public class SearchFunctions{
        public void search(List<Item> itemlist, List<FilterOptions> filter){

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
