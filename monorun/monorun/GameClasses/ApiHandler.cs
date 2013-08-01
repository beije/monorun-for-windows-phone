using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Phone.Net.NetworkInformation;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace monorun
{
    public class ApiHandler
    {
        public Boolean isOnline;
        private string serverName = "10.0.0.11";
        private string apiPath = "/monorun/monorun/api/api.php";
        private string apiProtocol = "http";
        private string apiUrl = "";
        private string playerid;

        public ApiHandler( Action<Object, DownloadStringCompletedEventArgs> apiAvailableCallback )
        {
            apiUrl = apiProtocol + "://" + serverName + apiPath;
            isOnline = CheckForInternetConnection();
            playerid = "";
            if (isOnline) 
            {
                if (apiAvailableCallback != null)
                {
                    // Check that the API is available
                    makeRequest(apiUrl + "?", apiAvailableCallback);
                }
            }
        }
        public void setConnectionState(Boolean state) 
        {
            isOnline = state;
        }
        public void postResult( int score, string username )
        {
            if (playerid == "") return;

            String url = apiUrl + "?do=put&playerid=" + playerid + "&username=" + HttpUtility.UrlEncode(username) + "&score=" + score + "&sourceid=3";

            Action<Object, DownloadStringCompletedEventArgs> cb = (o, e) =>
             {
                 if (!e.Cancelled && e.Error == null)
                 {
                     System.Diagnostics.Debug.WriteLine("Score registered: " + (String) e.Result);
                 }
             };

            makeRequest(url, cb);
        }

        public void doRequest(string type, Action<Object, DownloadStringCompletedEventArgs> callback)
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
                    makeRequest(apiUrl + "?do=get", callback);  
                break;
            }
        
        }

        private void registerPlayer(Object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                try
                {
                    playerid = JsonConvert.DeserializeObject<String>((string)e.Result);
                }
                catch (Exception exc) { }
 
                System.Diagnostics.Debug.WriteLine(playerid);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Request failed");
                //System.Diagnostics.Debug.WriteLine(e.Error);
            }
        }
        
        private void makeRequest( String url, Action<Object, DownloadStringCompletedEventArgs> callback )
        {
            WebClient client = new WebClient();
            Uri uri = new Uri(url);
            if (callback != null) 
            {
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(callback);
            }
            client.DownloadStringAsync(uri);
        }
        private Boolean CheckForInternetConnection()
        {
            return (NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.None);
        }

    }
}
