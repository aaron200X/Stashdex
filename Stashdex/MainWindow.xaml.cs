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
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace Stashdex
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        List<object> hitResultsList = new List<object>();
        List<Canvas> canvasList = new List<Canvas>();
        public int selectedStashNumber = 0;
        public Grid usedGrid;
        public Image usedBackground;
        public Regex nameReg = new Regex(@"\/([\w| |-]*).png");
        public bool isQuadtab = true; //TODO - get quadtab info

        public MainWindow() {
            try {

                InitializeComponent();

                //setStashSize(isQuadtab);

                itemPreviewCanvas.Visibility = Visibility.Hidden;

                //jsonImport.import();
            } catch (Exception ex) {
                //TODO - als eigene Funktion mit stacktrace
                Log.log(ex);
                throw;
            }

        }

        public void displayObject(Item item, int counter) {
            Canvas c = new Canvas();
            //Filtered items will shown yellow
            Brush b = new SolidColorBrush(Color.FromArgb(150, 20, 20, 100));
            if (!item.identified) {
                b = new SolidColorBrush(Color.FromArgb(150, 150, 0, 0));
            }
            if (item.isFiltered) {
                b = new SolidColorBrush(Color.FromArgb(150, 255, 255, 0));
            }

            c.Background = b;
            c.Name = "itemCanvas_" + counter;

            //TODO Donwload image and use a small variant for quadtabs
            Image itemImage = setImage(item, isQuadtab);
            itemImage.Name = "itemImg_" + counter;

            Grid.SetColumn(c, item.x);
            Grid.SetColumnSpan(c, item.w);
            Grid.SetRow(c, item.y);
            Grid.SetRowSpan(c, item.h);

            usedGrid.Children.Add(c);
            c.Children.Add(itemImage);
            if (isQuadtab) { 
                itemImage.Width = item.w * 24;
                itemImage.Height = item.h * 24;
            }
            usedGrid.MouseMove += new MouseEventHandler(myPanel_MouseMove);
            usedGrid.MouseLeave += new MouseEventHandler(myPanel_MouseLeave);

            canvasList.Add(c);
        }
    

        void setStashSize(bool isQuadtab) {
            if (isQuadtab) {
                usedGrid = quadGrid;
                usedBackground = stashQuadBackground;
            } else {
                usedGrid = normalGrid;
                usedBackground = stashBackground;
            }
            stashQuadBackground.Opacity = 0;
            stashBackground.Opacity = 0;
            usedBackground.Opacity = 100;
        }

        /// <summary>
        /// Den Grid säubern
        /// </summary>
        /// <param name="counter"></param>
        public void deleteAllCanvas(){
            foreach(Canvas c in canvasList) {
                usedGrid.Children.Remove(c);
            }
            canvasList.Clear();
            
        }

        public void displayAllItems(Stash stash) {
            deleteAllCanvas();
            int i = 0;
            isQuadtab = stash.quadLayout;
            setStashSize(isQuadtab);
            foreach (Item item in stash.items) {
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
            VisualTreeHelper.HitTest(usedGrid, null,
                new HitTestResultCallback(MyHitTestResult),
                new PointHitTestParameters(pt));

            // Perform actions on the hit test results list.
            if (hitResultsList.Count > 0)
            {
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
                        Item item = Stashes.stashes[selectedStashNumber].items[index];

                        Thickness margin = itemPreviewCanvas.Margin;
                        margin.Left = Mouse.GetPosition(this).X + 10;
                        margin.Top = Mouse.GetPosition(this).Y + 10;
                        itemPreviewCanvas.Margin = margin;

                        fillItemView(item);
                        break;
                    }
                    
                }
            }
            
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
            //explizitMods.Content = item.filterMods;

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
            
            itemPreviewCanvas.Width = biggestWidth + 20;
            previewBorder.Width = biggestWidth + 20;

        }

        // Return the result of the hit test to the callback.
        public HitTestResultBehavior MyHitTestResult(HitTestResult result)
        {
            // Add the hit test result to the list that will be processed after the enumeration.
            hitResultsList.Add(result.VisualHit);

            // Set the behavior to return visuals at all z-order levels.
            return HitTestResultBehavior.Continue;
        }

        public string downloadImage(Item item, string itemName, bool isQuadtab) {
            try {
                WebClient webClient = new WebClient();
                if (!Directory.Exists("pics\\items")) {
                    Directory.CreateDirectory(@"pics\items");
                }
                if (!File.Exists($@"pics\\items\{itemName}.png")) {
                    webClient.DownloadFile(item.icon, $@"pics\items\{itemName}.png");
                }

            } catch (Exception ex) {
                Debug.Write(ex.Message);
            }
            string onlyThePath = System.IO.Path.GetFullPath($@"pics\items\");
            string fullPath = System.IO.Path.GetFullPath($@"pics\items\{itemName}.png");
            //string smallFilePath = Resizer(fullPath, itemName, onlyThePath, item);
            
                return fullPath;


        }

        public string Resizer(string fullPath, string itemName, string onlyThePath, Item item) {
            string newFilename = $"{itemName}.png";
            if (!File.Exists($@"{onlyThePath}\{newFilename}"))
            using (System.Drawing.Image original = System.Drawing.Image.FromFile(fullPath)) {
                int newHeight = Convert.ToInt16(item.h * 34);
                int newWidth = Convert.ToInt16(item.w * 34);

                    if (original.Height % 2 != 0){
                        newHeight += 1;
                    }
                    if (original.Width % 2 != 0) {
                        newWidth += 1;
                    }

                    using (System.Drawing.Bitmap newPic = new System.Drawing.Bitmap(newWidth, newHeight)) {
                    using (System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(newPic)) {
                        gr.DrawImage(original, 0, 0, (newWidth), (newHeight));
                         /* Put new file path here */
                        //TODO richtiger Name + richtiger Ordner
                        newPic.Save($@"{onlyThePath}\{newFilename}", System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
            return $@"{onlyThePath}\{newFilename}";
        }


        public Image setImage(Item item, bool isQuadtab)
        {
            Image image = new Image();
            BitmapImage bi = new BitmapImage();
            string itemName = nameReg.Match(item.icon).Groups[1].Value;
            string fullPath = downloadImage(item, itemName, isQuadtab);
            bi.BeginInit();
            //bi.UriSource = new Uri(src, UriKind.Absolute);
            bi.UriSource = new Uri(fullPath, UriKind.Absolute);
            //bi.UriSource = new Uri($@"D:\Programme\visual_Studio Projekte\Stashdex\Stashdex\pics\items\{itemName}", UriKind.RelativeOrAbsolute);
            bi.EndInit();
            image.Source = bi;
            return image;
        }


        public void fillStashList() {
            listBoxStashes.Items.Clear();
            foreach (var stash in Stashes.stashes) {
                ListBoxItem listBoxI = new ListBoxItem();
                if (stash.hasFilteredItem) {
                    listBoxI.Content = $"*** {stash.n} ***";
                } else {
                    listBoxI.Content = stash.n;
                }

                

                
                int grayScale = (int)((stash.colour.r * .3) + (stash.colour.g * .59) + (stash.colour.b * .11));

                //Original A = 255
                Brush bru = new SolidColorBrush(Color.FromArgb(100, Convert.ToByte(stash.colour.r), Convert.ToByte(stash.colour.g), Convert.ToByte(stash.colour.b)));
                Brush boarderbruYellow = new SolidColorBrush(Color.FromArgb(150, 255, 255, 0));
                Brush boarderbruDark = new SolidColorBrush(Color.FromArgb(150, 50, 50, 0));

                listBoxI.Background = bru;
                if (stash.hasFilteredItem) {
                    listBoxI.BorderBrush = boarderbruYellow;
                    
                    listBoxI.BorderThickness = new Thickness(5);
                }
                
                if (grayScale <= 100) {
                    //TODO set the right Textcolor
                }
                listBoxStashes.Items.Add(listBoxI);
            }
            
        }

        private void filterOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO preventing multiple Windows 
            FilterOptionsWindow filterWindow = new FilterOptionsWindow();
            FilterOptionsWindow.fillFilterBox(filterWindow);
            
            filterWindow.Show();
        }

        private void button_Click(object sender, RoutedEventArgs e) {
            Stashes.getOnlineStashes(nameTxtBox.Text, poeidPwBox.Password);
            displayAllItems(Stashes.stashes[0]);
            Stashes.fillTheStashesAttributes();
            fillStashList();
        }

        private void localButton_Click(object sender, RoutedEventArgs e) {
            Stashes.getOnlineStashes(nameTxtBox.Text, poeidPwBox.Password, true);
            displayAllItems(Stashes.stashes[0]);
            Stashes.fillTheStashesAttributes();
            fillStashList();
        }

        private void listBoxStashes_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            //Fix the selectedIndex
            if (listBoxStashes.SelectedIndex != -1) {
                selectedStashNumber = listBoxStashes.SelectedIndex;
            }
            
            displayAllItems(Stashes.stashes[selectedStashNumber]);
            
        }
    }
}
