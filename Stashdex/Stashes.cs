﻿using System;
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


    }

    /// <summary>
    /// A (slightly) better version of .Net's default <see cref="WebClient"/>.
    /// The extra features include:
    /// ability to disable automatic redirect handling,
    /// sessions through a cookie container,
    /// indicate to the webserver that GZip compression can be used,
    /// exposure of the HTTP status code of the last request,
    /// exposure of any response header of the last request,
    /// ability to modify the request before it is send.
    /// </summary>
    /// <seealso cref="System.Net.WebClient" />
    public class BetterWebClient : WebClient {
        private WebRequest _request = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="BetterWebClient" /> class.
        /// </summary>
        /// <param name="cookies">The cookies. If set to <c>null</c> a container will be created.</param>
        /// <param name="autoRedirect">if set to <c>true</c> the client should handle the redirect automatically. Default value is <c>true</c></param>
        public BetterWebClient(CookieContainer cookies = null, bool autoRedirect = true) {
            CookieContainer = cookies ?? new CookieContainer();
            AutoRedirect = autoRedirect;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to automatically redirect when a 301 or 302 is returned by the request.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the client should handle the redirect automatically; otherwise, <c>false</c>.
        /// </value>
        public bool AutoRedirect { get; set; }

        /// <summary>
        /// Gets or sets the cookie container. This contains all the cookies for all the requests.
        /// </summary>
        /// <value>
        /// The cookie container.
        /// </value>
        public CookieContainer CookieContainer { get; set; }

        /// <summary>
        /// Gets the cookies header (Set-Cookie) of the last request.
        /// </summary>
        /// <value>
        /// The cookies or <c>null</c>.
        /// </value>
        public string Cookies {
            get { return GetHeaderValue("Set-Cookie"); }
        }

        /// <summary>
        /// Gets the location header for the last request.
        /// </summary>
        /// <value>
        /// The location or <c>null</c>.
        /// </value>
        public string Location {
            get { return GetHeaderValue("Location"); }
        }

        /// <summary>
        /// Gets the status code. When no request is present, <see cref="HttpStatusCode.Gone"/> will be returned.
        /// </summary>
        /// <value>
        /// The status code or <see cref="HttpStatusCode.Gone"/>.
        /// </value>
        public HttpStatusCode StatusCode {
            get {
                var result = HttpStatusCode.Gone;

                if (_request != null) {
                    var response = base.GetWebResponse(_request) as HttpWebResponse;

                    if (response != null) {
                        result = response.StatusCode;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets or sets the setup that is called before the request is done.
        /// </summary>
        /// <value>
        /// The setup.
        /// </value>
        public Action<HttpWebRequest> Setup { get; set; }

        /// <summary>
        /// Gets the header value.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        /// <returns>The value.</returns>
        public string GetHeaderValue(string headerName) {
            if (_request != null) {
                return base.GetWebResponse(_request)?.Headers?[headerName];
            }

            return null;
        }

        /// <summary>
        /// Returns a <see cref="T:System.Net.WebRequest" /> object for the specified resource.
        /// </summary>
        /// <param name="address">A <see cref="T:System.Uri" /> that identifies the resource to request.</param>
        /// <returns>
        /// A new <see cref="T:System.Net.WebRequest" /> object for the specified resource.
        /// </returns>
        protected override WebRequest GetWebRequest(Uri address) {
            _request = base.GetWebRequest(address);

            var httpRequest = _request as HttpWebRequest;

            if (_request != null) {
                httpRequest.AllowAutoRedirect = AutoRedirect;
                httpRequest.CookieContainer = CookieContainer;
                httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                Setup?.Invoke(httpRequest);
            }

            return _request;
        }
    }
}
