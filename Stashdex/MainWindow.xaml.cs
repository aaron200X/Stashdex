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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Stashdex
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            jsonImport.import();
        }

        //private void ImportButton_Click(object sender, EventArgs e)
        //{
        //    jsonImport j = new jsonImport();
        //    j.Show();
        //}

        public void displayObject(Item item, int counter)
        {

            Canvas c = new Canvas();
            Brush b = new SolidColorBrush(Color.FromArgb(150, 20, 20, 100));
            c.Background = b;

            Image itemImage = new Image();
            BitmapImage bi2 = new BitmapImage();
            bi2.BeginInit();
            bi2.UriSource = new Uri(item.icon, UriKind.Absolute);
            bi2.EndInit();
            itemImage.Source = bi2;
            itemImage.Name = "itemImg" + counter;

            Grid.SetColumn(c, item.x);
            Grid.SetColumnSpan(c, item.w);
            Grid.SetRow(c, item.y);
            Grid.SetRowSpan(c, item.h);
            g.Children.Add(c);

            Grid.SetColumn(itemImage, item.x);
            Grid.SetColumnSpan(itemImage, item.w);
            Grid.SetRow(itemImage, item.y);
            Grid.SetRowSpan(itemImage, item.h);

            g.Children.Add(itemImage);
            g.MouseMove += new MouseEventHandler(myPanel_MouseMove);

        }

        void myPanel_MouseMove(object sender, MouseEventArgs e)
        {
            itemNameBox.Text = sender.GetType().ToString();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            int i = 0;
            foreach (Item item in Stashes.stashes[0].items)
            {
                displayObject(item, i);
                i++;
            }
        }
    }
}
