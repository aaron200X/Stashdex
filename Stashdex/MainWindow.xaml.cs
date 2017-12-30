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

            Image itemImage = setImage(item.icon);
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
            g.MouseLeave += new MouseEventHandler(myPanel_MouseLeave);
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
        private void myPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            itemPreviewCanvas.Visibility = Visibility.Hidden;
        }

        private void myPanel_MouseMove(object sender, MouseEventArgs e)
        {
            itemPreviewCanvas.Visibility = Visibility.Visible;

            // Retrieve the coordinate of the mouse position.
            Point pt = e.GetPosition((UIElement)sender);

            // Clear the contents of the list used for hit test results.
            hitResultsList.Clear();

            // Set up a callback to receive the hit test result enumeration.
            VisualTreeHelper.HitTest(g, null,
                new HitTestResultCallback(MyHitTestResult),
                new PointHitTestParameters(pt));

            // Perform actions on the hit test results list.
            if (hitResultsList.Count > 0)
            {
                //Debug.Print(hitResultsList.Count.ToString());
                ////schmeißt alle nicht Images raus, damit ich an den Namen komme
                //if (hitResultsList[0].GetType().ToString() == "System.Windows.Controls.Canvas")
                //{
                //    hitResultsList.Remove(hitResultsList[0]);
                //}
                //else
                //{
                //    hitResultsList.Remove(hitResultsList[1]);
                //}

                for (int i = 0; i <= hitResultsList.Count(); i++)
                {
                    int index = -1;

                    if (hitResultsList[i].GetType().ToString().Contains("Image"))
                    {
                        Image img = (Image)hitResultsList[i];
                        index = Convert.ToInt32(img.Name.Split('_').Last());

                    } else if (hitResultsList[i].GetType().ToString().Contains("Canvas"))
                    {
                        Canvas canvas = (Canvas)hitResultsList[i];
                        index = Convert.ToInt32(canvas.Name.Split('_').Last());
                    }

                    if (index >= 0)
                    {
                        Item item = Stashes.stashes[0].items[index];
                        itemNameBox.Text = item.typeLine;

                        Thickness margin = itemPreviewCanvas.Margin;
                        margin.Left = Mouse.GetPosition(this).X + 10;
                        margin.Top = Mouse.GetPosition(this).Y + 10;
                        itemPreviewCanvas.Margin = margin;

                        fillItemView(item);
                        break;
                    }
                    
                }
            }

            //foreach (Image hitResult in hitResultsList)
            //{
            //    int index = Convert.ToInt32(hitResult.Name.Split('_').Last());
            //    //itemNameBox.Text = hitResult.Name;
            //    Item item = Stashes.stashes[0].items[index];
            //    itemNameBox.Text = item.typeLine;

            //    Thickness margin = itemPreviewCanvas.Margin;
            //    margin.Left = Mouse.GetPosition(this).X + 10;
            //    margin.Top = Mouse.GetPosition(this).Y + 10;
            //    itemPreviewCanvas.Margin = margin;

            //    fillItemView(item);

            //}
        }
    


         public void fillItemView(Item item)
        {
            //itemPreviewUpperBg.lo
            //Der Name ist bei Magicitems leer und das Itemtype übernimmt die Funktion
            if (string.IsNullOrEmpty(item.name))
            {
                nameLabel.Content = item.typeLine.Replace("<<set:MS>><<set:M>><<set:S>>", "").Trim();
                itemTypeLabel.Content = "";
            } else
            {
                nameLabel.Content = item.name.Replace("<<set:MS>><<set:M>><<set:S>>", "").Trim();
                itemTypeLabel.Content = item.typeLine.Replace("<<set:MS>><<set:M>><<set:S>>", "").Trim();
            }
            if (item?.explicitMods?.Count() >= 0)
            {
                explizitMods.Content = string.Join("\n", item.explicitMods);
            } else
            {
                explizitMods.Content = "";
            }

            //Get biggest width
            double biggestWidth = 0;
            
            for (int i = 0; i < itemPreviewCanvas.Children.Count; i++)
            {
                if (itemPreviewCanvas.Children[i].GetType().ToString().Contains("Label"))
                {
                    Label label = (Label)itemPreviewCanvas.Children[i];
                    if (biggestWidth == 0 || biggestWidth <= label.ActualWidth)
                    {
                        biggestWidth = label.ActualWidth;
                    }
                }
            }

            //Setzt die Borderfarbe
            Brush borderBrush = new SolidColorBrush();
            switch (item.frameType)
            {
                case 0:
                    previewBorder.BorderBrush = Brushes.White;
                    break;
                case 1:
                    previewBorder.BorderBrush = Brushes.Blue;
                    break;
                case 2:
                    previewBorder.BorderBrush = Brushes.Gold;
                    break;
                case 3:
                    previewBorder.BorderBrush = Brushes.Goldenrod;
                    break;
                case 4:
                    previewBorder.BorderBrush = Brushes.ForestGreen;
                    break;
                case 5:
                    previewBorder.BorderBrush = Brushes.Black;
                    break;
                case 6:
                    previewBorder.BorderBrush = Brushes.Gray;
                    break;
                case 7:
                    previewBorder.BorderBrush = Brushes.Green;
                    break;
                case 8:
                    previewBorder.BorderBrush = Brushes.Violet;
                    break;
                case 9:
                    
                    previewBorder.BorderBrush = Brushes.Black;
                    break;

            }
            

            

                //Setzt die Breite
                sizeDebug.Text = biggestWidth.ToString();
            
            itemPreviewCanvas.Width = biggestWidth + 20;
            previewBorder.Width = biggestWidth + 20;
            
            //nameLabel.Width = 600;
            //nameLabel.VerticalAlignment = VerticalAlignment.Center;
            //nameLabel.HorizontalAlignment = HorizontalAlignment.Center;

        }

        // Return the result of the hit test to the callback.
        public HitTestResultBehavior MyHitTestResult(HitTestResult result)
        {
            // Add the hit test result to the list that will be processed after the enumeration.
            hitResultsList.Add(result.VisualHit);

            // Set the behavior to return visuals at all z-order levels.
            return HitTestResultBehavior.Continue;
        }

        public Image setImage(string src)
        {
            Image image = new Image();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(src, UriKind.Absolute);
            bi.EndInit();
            image.Source = bi;
            return image;
        }
        
    }
}
