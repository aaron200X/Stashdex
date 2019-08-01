using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stashdex
{
    public static class Stashes
    {
        public static List<Stash> stashes = new List<Stash>();

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
    }
}
