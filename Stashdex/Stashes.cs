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

            string adress = $"https://pathofexile.com/character-window/get-stash-items?league=Legion&tabs=1&tabIndex=11&accountName={name}";
            using (WebClient webClient = new WebClient()) {
                string response = Encoding.UTF8.GetString(webClient.UploadValues(adress, new NameValueCollection() {
                { "POESESSID", poeid }
            }));

                Console.Write(response);

                //    WebClient wClient = new WebClient();
                //wClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");



                //StreamReader reader = new StreamReader(data);
                //string HtmlResult = wc.UploadString(URI, myParameters);
                //string s = reader.ReadToEnd();
                //return new List<Stash>();
            }
        }
    }
}
