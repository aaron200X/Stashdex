using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Stashdex
{
    /// <summary>
    /// Interaction logic for FilterOptionsWindow.xaml
    /// </summary>
    public partial class FilterOptionsWindow : Window
    {
        public FilterOptionsWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e) {
            Filter filter = new Filter();
            filter.filtername = filterModTxtBox.Text;

            if(numberMinTxtBox.Text != "") filter.minValue = Convert.ToInt16(numberMinTxtBox.Text);
            if (numberMaxTxtBox.Text != "") filter.maxValue = Convert.ToInt16(numberMaxTxtBox.Text);

            SearchFunctions.search(filter);
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        //TODO Programm LOESCHBUTTON

        private void buttonAdd_Click(object sender, RoutedEventArgs e) {
            //foreach ( Control x i)
               
            filterModTxtBox.Text = giveBackFilterBox(this);
        }


        /// <summary>
        /// gives back the selected Value from the filterbox
        /// </summary>
        /// <param name="filterWindow"></param>
        /// <returns></returns>
        public static string giveBackFilterBox(FilterOptionsWindow filterWindow) {
            ListBoxItem X = (ListBoxItem)filterWindow.filterBox.SelectedValue;
            if (X != null) {
                return X.Content.ToString();
            } else {
                return "";
            }
        }

        /// <summary>
        /// Collects all mods you found, to show them in your filter
        /// </summary>
        /// <param name="filterWindow"></param>
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
