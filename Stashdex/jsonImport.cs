using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;


namespace Stashdex
{
    class jsonImport
    {

        public static void import()
        {
            //importTxt.Text = File.ReadAllText("../../TextFile1.txt");
            Stash stash = JsonConvert.DeserializeObject<Stash>(File.ReadAllText("../../TextFile1.txt"));
            //Stash stash = JsonConvert.DeserializeObject<Stash>(importTxt.Text);
            Stashes.stashes.Add(stash);
        }

        public static void import(string name, string sessid)
        {
           
        }
    }
}
