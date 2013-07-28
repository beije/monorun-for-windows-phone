using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Phone.Net.NetworkInformation;
using System.IO;


namespace monorun
{
    class ApiHandler
    {
        public Boolean isOnline;
        private string apiUrl = "http://10.0.0.11/monorun/monorun/api/api.php";
        private string playerid;
        private WebClient wc;

        public ApiHandler()
        {
            isOnline = CheckForInternetConnection();
            if (isOnline) 
            {
                wc = new WebClient();
            }
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
