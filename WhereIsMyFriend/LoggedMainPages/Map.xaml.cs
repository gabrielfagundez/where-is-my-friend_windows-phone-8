using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Devices.Geolocation;
using WhereIsMyFriend.Classes;
using Newtonsoft.Json;
using System.Threading;

namespace WhereIsMyFriend.LoggedMainPages
{
    public partial class Map : PhoneApplicationPage
    {
        string latitud, longitud = string.Empty;

        public Map()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (App.Geolocator == null)
            {
                App.Geolocator = new Geolocator();
                App.Geolocator.DesiredAccuracy = PositionAccuracy.High;
                App.Geolocator.MovementThreshold = 100; // The units are meters.
                App.Geolocator.PositionChanged += geolocator_PositionChanged;
            }
            LatitudeTextBlock.Text = latitud;
            LongitudeTextBlock.Text = longitud;
        }

        protected override void OnRemovedFromJournal(System.Windows.Navigation.JournalEntryRemovedEventArgs e)
        {
            App.Geolocator.PositionChanged -= geolocator_PositionChanged;
            App.Geolocator = null;
        }

        void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {

            if (!App.RunningInBackground)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    latitud = args.Position.Coordinate.Latitude.ToString("0.00000");
                    longitud = args.Position.Coordinate.Longitude.ToString("0.00000");

                    LatitudeTextBlock.Text = args.Position.Coordinate.Latitude.ToString("0.00000");
                    LongitudeTextBlock.Text = args.Position.Coordinate.Longitude.ToString("0.00000");
                    var webClient = new WebClient();
                    webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                    webClient.UploadStringCompleted += this.sendPostCompleted1;
                    LoggedUser user = LoggedUser.Instance;
                    string json = "{\"Mail\":\"" + user.GetLoggedUser().Mail + "\"," +
                                    "\"Latitude\":\"" + latitud + "\"," +
                                      "\"Longitude\":\"" + longitud + "\"}";
                    System.Diagnostics.Debug.WriteLine(json);

                    webClient.UploadStringAsync((new Uri("http://developmentpis.azurewebsites.net/api/Geolocation/SetLocation/")), "POST", json);
                });
            }
            else
            {
                latitud = args.Position.Coordinate.Latitude.ToString("0.00000");
                longitud = args.Position.Coordinate.Longitude.ToString("0.00000");
                var webClient = new WebClient();
                webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                webClient.UploadStringCompleted += this.sendPostCompleted1;
                LoggedUser user = LoggedUser.Instance;
                string json = "{\"Mail\":\"" + user.GetLoggedUser().Mail + "\"," +
                                "\"Latitude\":\"" + latitud + "\"," +
                                  "\"Longitude\":\"" + longitud + "\"}";
                System.Diagnostics.Debug.WriteLine(json);

                webClient.UploadStringAsync((new Uri("http:http://developmentpis.azurewebsites.net/api/Geolocation/SetLocation/")), "POST", json);


                Microsoft.Phone.Shell.ShellToast toast = new Microsoft.Phone.Shell.ShellToast();
                toast.Content = latitud + longitud;
                toast.Title = "Location: ";
                toast.NavigationUri = new Uri("/LoggedMainPages/Map.xaml", UriKind.Relative);
                toast.Show();

            }
        }

        private void sendPostCompleted1(object sender, UploadStringCompletedEventArgs e)
        {
            if ((e.Error != null) && (e.Error.GetType().Name == "WebException"))
            {
                WebException we = (WebException)e.Error;
                HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                switch (response.StatusCode)
                {

                    case HttpStatusCode.NotFound: // 404
                        System.Diagnostics.Debug.WriteLine("Not found!");
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        System.Diagnostics.Debug.WriteLine("Not authorized!");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(e.Result.ToString());
            }
        }


        private void Check_Click(object sender, EventArgs e)
        {


            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
                LoggedUser user = LoggedUser.Instance;
                webClient.DownloadStringAsync(new Uri("http:http://developmentpis.azurewebsites.net/api/Friends/GetAllFriends/" + user.GetLoggedUser().Id)); 
               
            }
            catch (WebException webex)
            {
                HttpWebResponse webResp = (HttpWebResponse)webex.Response;

                switch (webResp.StatusCode)
                {
                    case HttpStatusCode.NotFound: // 404
                        break;
                    case HttpStatusCode.InternalServerError: // 500
                        break;
                    default:
                        break;
                }
            }
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if ((e.Error != null) && (e.Error.GetType().Name == "WebException"))
            {
                WebException we = (WebException)e.Error;
                HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                switch (response.StatusCode)
                {

                    case HttpStatusCode.NotFound: // 404
                        System.Diagnostics.Debug.WriteLine("Not found!");
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        System.Diagnostics.Debug.WriteLine("Not authorized!");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("{\"Amigos\":" + e.Result + "}");
                var p = JsonConvert.DeserializeObject<ListaAmigos>("{\"Amigos\":" + e.Result + "}");
                foreach (var friend in p.Amigos)
                {
                    System.Diagnostics.Debug.WriteLine(friend.Mail);
                } 
            }
        }

        // THREAD ******************************************************


       
        

       

        
    }
}