using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Specialized;
using System.IO;

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

        public static void getOnlineStashes(string name, string poeid, bool getLocalStash = false) {
            jsonImport.clearStash();
            string response = "";
            int tabIndex = 0;
            //TODO, hole Anzahl der Tabs und loope durch
            string adress = $"https://pathofexile.com/character-window/get-stash-items?league=Legion&tabs=1&tabIndex={tabIndex}&accountName={name}";
            CookieContainer coocCont = new CookieContainer();
            Cookie cook = new Cookie("POESESSID", poeid) { Domain = "pathofexile.com" };
            coocCont.Add(cook);
            //First try to get the number of Tabs
            using (BetterWebClient webClient = new BetterWebClient(coocCont)) {
                try {
                    if (!getLocalStash) {
                        response = webClient.DownloadString(adress);
                    } else {
                        response = loadStashLocal(tabIndex);
                    }

                    jsonImport.import(response);
                    if (!getLocalStash) {
                        saveStashesLocal(tabIndex, response);
                    }
                } catch (Exception ex) {
                    Log.log(ex);
                    throw;
                }

              
                tabIndex++;
                //Getting all Tabs
                //DEBUG
                while (tabIndex < stashes[0].numTabs) {
                //while (tabIndex < 15) {
                    response = "";
                    adress = $"https://pathofexile.com/character-window/get-stash-items?league=Legion&tabs=1&tabIndex={tabIndex}&accountName={name}";
                    if (!getLocalStash) {
                            try {
                                response = webClient.DownloadString(adress);

                            } catch (Exception ex) {
                            Console.Write(ex.Message);
                            System.Threading.Thread.Sleep(100000);
                            response = webClient.DownloadString(adress);
                        }
                    } else {
                        response = loadStashLocal(tabIndex);
                    }
                    jsonImport.import(response);
                    //TODO: Zeige Update im Form
                    win.statusLabel.Content = $"fetching tabs: {tabIndex + 1} / {stashes[0].numTabs}";
                    saveStashesLocal(tabIndex, response);
                    tabIndex++;

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

        public static void saveStashesLocal(int tabIndex, string response) {
            if (!Directory.Exists("Stashes")) {
                Directory.CreateDirectory("Stashes");
            }

            using (StreamWriter sw = File.CreateText($"Stashes/S{tabIndex}.txt")) {
                sw.Write(response);                
            } 
        }

        public static string loadStashLocal(int tabIndex) {
            if (File.Exists($"Stashes/S{tabIndex}.txt")) {
                return File.ReadAllText($"Stashes/S{tabIndex}.txt");
            } else {
                return "";
            }         
        }

        public static void fillTheStashesAttributes() {
            foreach (var stash in stashes) {
                stash.n = stash.tabs[stashes.IndexOf(stash)].n;
                stash.colour = stash.tabs[stashes.IndexOf(stash)].colour;
                stash.i = stash.tabs[stashes.IndexOf(stash)].i;
            }
        }

    }

}
