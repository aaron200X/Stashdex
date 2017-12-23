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
            //Stash stash = JsonConvert.DeserializeObject<Stash>(importTxt.Text);
            Stashes.stashes.Add(stash);
        }

        public static void import(string name, string sessid)
        {
            WebRequest webrequest = WebRequest.Create("https://pathofexile.com/character-window/get-stash-items?league=Abyss&tabs=1&tabIndex=0&accountName=" + name);
            webrequest.Method = "POST";
            var postData = "poesessid=" + sessid;
            var data = Encoding.ASCII.GetBytes(postData);
            webrequest.ContentLength = data.Length;
            
            
            using (var stream = webrequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            Cookie cookie = new Cookie("POESESSID", sessid);
            HttpWebResponse response = (HttpWebResponse)webrequest.GetResponse();
            response.Cookies.Add(new Cookie("POESESSID", sessid));


            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        }
    }
}
