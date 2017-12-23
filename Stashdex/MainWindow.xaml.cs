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
using System.Windows.Media;

namespace Stashdex
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<object> hitResultsList = new List<object>();

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
            c.Name = "itemCanvas_" + counter;

            Image itemImage = new Image();
            BitmapImage bi2 = new BitmapImage();
            bi2.BeginInit();
            bi2.UriSource = new Uri(item.icon, UriKind.Absolute);
            bi2.EndInit();
            itemImage.Source = bi2;
            itemImage.Name = "itemImg_" + counter;

            Grid.SetColumn(c, item.x);
            Grid.SetColumnSpan(c, item.w);
            Grid.SetRow(c, item.y);
            Grid.SetRowSpan(c, item.h);

            Grid.SetColumn(itemImage, item.x);
            Grid.SetColumnSpan(itemImage, item.w);
            Grid.SetRow(itemImage, item.y);
            Grid.SetRowSpan(itemImage, item.h);
            g.Children.Add(c);
            c.Children.Add(itemImage);

            g.MouseMove += new MouseEventHandler(myPanel_MouseMove);

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

        private void myPanel_MouseMove(object sender, MouseEventArgs e)
        {
            // Retrieve the coordinate of the mouse position.
            Point pt = e.GetPosition((UIElement)sender);
            pt.X = pt.X;
            pt.Y = pt.Y;
            xyKoordinate.Text = pt.ToString();

            // Clear the contents of the list used for hit test results.
            hitResultsList.Clear();

            // Set up a callback to receive the hit test result enumeration.
            VisualTreeHelper.HitTest(g, null,
                new HitTestResultCallback(MyHitTestResult),
                new PointHitTestParameters(pt));

            // Perform actions on the hit test results list.
            if (hitResultsList.Count > 0)
            {
                Debug.Print(hitResultsList.Count.ToString());
                //schmeißt alle nicht Images raus, damit ich an den Namen komme
                if (hitResultsList[0].GetType().ToString() == "System.Windows.Controls.Canvas")
                {
                    hitResultsList.Remove(hitResultsList[0]);
                }
                else
                {
                    hitResultsList.Remove(hitResultsList[1]);
                }


                foreach (Image hitResult in hitResultsList)
                {
                    int index = Convert.ToInt32(hitResult.Name.Split('_').Last());
                    //itemNameBox.Text = hitResult.Name;

                    itemNameBox.Text = Stashes.stashes[0].items[index].typeLine;
                }
            }
        }

        // Return the result of the hit test to the callback.
        public HitTestResultBehavior MyHitTestResult(HitTestResult result)
        {
            // Add the hit test result to the list that will be processed after the enumeration.
            hitResultsList.Add(result.VisualHit);

            // Set the behavior to return visuals at all z-order levels.
            return HitTestResultBehavior.Continue;
        }

        private void importButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
