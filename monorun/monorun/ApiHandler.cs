using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Phone.Net.NetworkInformation;
using System.IO;
using Newtonsoft.Json;

namespace monorun
{
    class ApiHandler
    {
        public Boolean isOnline;
        private string apiUrl = "http://10.0.0.11/monorun/monorun/api/api.php";
        private string playerid;
        private List<Highscore> highscores;
        private WebClient wc;

        public ApiHandler()
        {
            isOnline = CheckForInternetConnection();
            if (isOnline) 
            {
                //doRequest("get");
            }
        }
        public void postResult( int score, string username )
        {
            String url = apiUrl + "?do=put&playerid=" + playerid + "&username=" + username + "&score=" + score+"&sourceid=3";
            makeRequest(url, postResultCallback);

        }
        public void doRequest(string type)
        {
            if (!isOnline)
            {
                return;
            }
            switch( type )
            {
                case "register":
                    makeRequest(apiUrl + "?do=register", registerPlayer);
                break;
                case "get":
                    makeRequest(apiUrl + "?do=get", populateHighscore);  
                break;
            }
        
        }
        private void populateHighscore(Object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                highscores = JsonConvert.DeserializeObject<List<Highscore>>((string)e.Result);
            }
        }

        private void registerPlayer( Object sender, DownloadStringCompletedEventArgs e )
        {
            if (!e.Cancelled && e.Error == null)
            {
                this.playerid = (string)e.Result;
                System.Diagnostics.Debug.WriteLine(playerid);
            }
        }
        private void postResultCallback(Object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                System.Diagnostics.Debug.WriteLine((string)e.Result);
            }
        }
        private void makeRequest( String url, Action<Object, DownloadStringCompletedEventArgs> callback )
        {
            WebClient client = new WebClient();
            Uri uri = new Uri(url);
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(callback);
            client.DownloadStringAsync(uri);
            
        }
        private Boolean CheckForInternetConnection()
        {
            return (NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.None);
        }

    }
}
