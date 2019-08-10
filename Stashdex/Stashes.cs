using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Specialized;


namespace Stashdex {
    public static class Stashes {
        public static List<Stash> stashes = new List<Stash>();
        public static List<Item> allFilteredItems = new List<Item>();
        public static List<string> absolutelyAllMods = new List<string>();
        static MainWindow win = (MainWindow)App.Current.MainWindow;

        /// <summary>
        /// Creates a List with all mods, that exists in your stash
        /// </summary>
        public static void getAbsolutelyAllMods() {
            foreach (Stash stash in stashes) {
                foreach (var item in stash.items) {
                    foreach (string mods in item.allModsDic.Keys) {
                        if (!absolutelyAllMods.Contains(mods)) {
                            absolutelyAllMods.Add(mods);
                        }
                    }
                }
            }
            //TODO Mods mit 2 Zahlen fixen
            absolutelyAllMods.Sort();
        }

        public static void getOnlineStashes(string name, string poeid) {
            jsonImport.clearStash();
            string response;
            int tabIndex = 0;
            //TODO, hole Anzahl der Tabs und loope durch
            string adress = $"https://pathofexile.com/character-window/get-stash-items?league=Legion&tabs=1&tabIndex={tabIndex}&accountName={name}";
            CookieContainer coocCont = new CookieContainer();
            Cookie cook = new Cookie("POESESSID", poeid) { Domain = "pathofexile.com" };
            tabIndex++;
            coocCont.Add(cook);
            //First try to get the number of Tabs
            using (BetterWebClient webClient = new BetterWebClient(coocCont)) {
                response = webClient.DownloadString(adress);
                jsonImport.import(response);
                //Getting all Tabs
                //DEBUG
                //while (tabIndex < stashes[0].numTabs) {
                while (tabIndex < 15) {
                    tabIndex++;
                    adress = $"https://pathofexile.com/character-window/get-stash-items?league=Legion&tabs=1&tabIndex={tabIndex}&accountName={name}";
                    response = webClient.DownloadString(adress);
                    jsonImport.import(response);
                    //TODO: Zeige Update im Form
                    win.statusLabel.Content = $"fetching tabs: {tabIndex + 1} / {stashes[0].numTabs}";
                }
                
            }
            win.statusLabel.Content = "";
        }

        public static void getOnlineStashes(string name, string poeid, int tabIndex) {
           
                jsonImport.clearStash();
                string response;
                //TODO, hole Anzahl der Tabs und loope durch
                string adress = $"https://pathofexile.com/character-window/get-stash-items?league=Legion&tabs=1&tabIndex={tabIndex}&accountName={name}";
                CookieContainer coocCont = new CookieContainer();
                Cookie cook = new Cookie("POESESSID", poeid) { Domain = "pathofexile.com" };
                coocCont.Add(cook);
            //TODO Webclient immer beibehalten
            using (BetterWebClient webClient = new BetterWebClient(coocCont)) {
                response = webClient.DownloadString(adress);

                jsonImport.import(response);
            }
        }

    }

}
