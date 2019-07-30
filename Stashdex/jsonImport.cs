using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace Stashdex
{
    class jsonImport
    {

        public static void import()
        {
            //importTxt.Text = File.ReadAllText("../../TextFile1.txt");
            Stash stash = JsonConvert.DeserializeObject<Stash>(File.ReadAllText("../../TextFile1.txt"));
            foreach (Item item in stash.items) {
                item.allMods = item.getAllmods();
                item.filterMods = item.getFilterMods();
                if (item.filterMods != null) item.allMods?.AddRange(item.filterMods);
            }
            //Stash stash = JsonConvert.DeserializeObject<Stash>(importTxt.Text);
            Stashes.stashes.Add(stash);
        }
        
    }
}
