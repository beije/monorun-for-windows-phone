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
		public Highscore LatestHighscore = new Highscore();

        private string serverName = "dev.monorun.com";
        private string apiPath = "/api/api.php";
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

		public void updateScore( String username, Action callback ) 
		{
			if (isOnline == false)
			{
				callback();
				return;
			}

			if (LatestHighscore.secretkey == "") return;
			if (username.Trim() == "") return;

			String url = apiUrl + "?do=update&id=" + LatestHighscore.id + "&username=" + HttpUtility.UrlEncode(username.Trim()) + "&secretkey=" + LatestHighscore.secretkey;
			LatestHighscore.username = username;

			Action<Object, DownloadStringCompletedEventArgs> cb = (o, e) =>
			{
				if (!e.Cancelled && e.Error == null)
				{
					callback();
				}
			};

			makeRequest(url, cb);


		}

        public void postResult( int score, string username, Action callback )
        {
            if (playerid == "" || isOnline == false ) {
				callback();
				return;
			}

            String url = apiUrl + "?do=put&playerid=" + playerid + "&score=" + score + "&sourceid=3";

            Action<Object, DownloadStringCompletedEventArgs> cb = (o, e) =>
             {
                 if (!e.Cancelled && e.Error == null)
                 {
					 try
					 {
						 System.Diagnostics.Debug.WriteLine("Score registered: " + (String)e.Result);
						 LatestHighscore = JsonConvert.DeserializeObject<Highscore>( (string) e.Result );
					 }
					 catch (Exception)
					 {
						 // Something is wrong with the API, turn off the connection
						 isOnline = false;
					 }
					 callback();
                 }
             };

            makeRequest(url, cb);
        }

		public void getSessionId(Action callback)
		{
			if ( isOnline == false )
			{
				callback();
				return;
			}

			String url = apiUrl + "?do=register";

			Action<Object, DownloadStringCompletedEventArgs> cb = (o, e) =>
			{
				if (!e.Cancelled && e.Error == null)
				{
					try
					{
						playerid = JsonConvert.DeserializeObject<String>((string)e.Result);
					}
					catch (Exception)
					{
						// Something is wrong with the API, turn off the connection
						isOnline = false;
					}
					callback();
				}
			};

			makeRequest(url, cb);
		}

		public void getHighscore(Action<List<Highscore>> callback)
		{
			if (isOnline == false)
			{
				callback(new List<Highscore>());
				return;
			}

			String url = apiUrl + "?do=get";

			Action<Object, DownloadStringCompletedEventArgs> cb = (o, e) =>
			{
				if (!e.Cancelled && e.Error == null)
				{
					try
					{
						List<Highscore> list = JsonConvert.DeserializeObject<List<Highscore>>((String)e.Result);
						callback(list);
					}
					catch (Exception)
					{
						// Something is wrong with the API, turn off the connection
						isOnline = false;
						callback(new List<Highscore>());
					}
					
				}
			};

			makeRequest(url, cb);
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
