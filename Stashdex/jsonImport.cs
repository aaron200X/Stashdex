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
            //TODO beim durchgehen der Stashes, die Nummer mitzählen und die ID eintragen
            Stash stash = JsonConvert.DeserializeObject<Stash>(File.ReadAllText("../../TextFile1.txt"));
            //KOmme nicht an die Items bei der Erstellung ran. laufe also anschließend durch
            foreach (Item item in stash.items) {
                item.fillEverything();
            }
            //Stash stash = JsonConvert.DeserializeObject<Stash>(importTxt.Text);
            Stashes.stashes.Add(stash);
        }
        
    }
}
