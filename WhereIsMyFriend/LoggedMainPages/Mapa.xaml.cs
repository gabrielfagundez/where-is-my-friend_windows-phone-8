using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.LocalizedResources;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel; //Provides the Geocoordinate class.
using System.Device.Location; // Provides the GeoCoordinate class.
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WhereIsMyFriend.Classes;
using Windows.Devices.Geolocation;

namespace WhereIsMyFriend.LoggedMainPages
{
    public partial class Mapa : PhoneApplicationPage
    {

        string latitud, longitud = string.Empty;
        DispatcherTimer newTimer = new DispatcherTimer();

        public Mapa()
        {
            
            InitializeComponent();
            // timer interval specified as 1 second
            newTimer.Interval = TimeSpan.FromSeconds(5);
            // Sub-routine OnTimerTick will be called at every 1 second
            newTimer.Tick += OnTimerTick;
            // starting the timer
            newTimer.Start();
            //CustomMessageBox messageBox = new CustomMessageBox()
           // {
            //    Caption = "",
            //    LeftButtonContent = "Ok",
                //Message = "Instrucciones:\n*desplegar simulador de posicionamiento para ver como cambia tu pos. actual(puntito azul)\n* el boton de test agrega algunos puntos, si apretetas varias veces el boton, se simula el movimiento de tus amigos\n*para agregar o actualizar posiciones de amigos usar la funcion  public void insert(string id, string nom, GeoCoordinate geo) q esta en la clase PointsHandler q es un singleton\n*para dejar de seguir a un amigo en el mapa esta la funcion deleteFriend(string id)",


          //  };
         //   messageBox.Show();
           // GetCoordinate();

        }

        void OnTimerTick(Object sender, EventArgs args)
        {
            // text box property is set to current system date.
            // ToString() converts the datetime value into text
            System.Diagnostics.Debug.WriteLine("Friends Update en mapa!");
            DibujarAmigos();
        }

        //******************************************************************************************

        private void zoomOUT_Click(object sender, RoutedEventArgs e)
        {
            if (mapWithMyLocation.ZoomLevel > 1)
            {
                mapWithMyLocation.ZoomLevel--;
            }
            
        }

        private void zoomIN_Click(object sender, RoutedEventArgs e)
        {
            if (mapWithMyLocation.ZoomLevel < 20)
            {
                mapWithMyLocation.ZoomLevel++;
            }
        }

        //******************************************************************************************
        private void drawMyPosition()
        {

            GeoCoordinate pos = PointsHandler.Instance.myPosition;

            // Create a small circle to mark the current location.
            Ellipse myCircle = new Ellipse();
            myCircle.Fill = new SolidColorBrush(Colors.Blue);
            myCircle.Height = 20;
            myCircle.Width = 20;
            myCircle.Opacity = 50;

            // Create a MapOverlay to contain the circle.
            MapOverlay myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = myCircle;
            myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay.GeoCoordinate = pos;

            // Create a MapLayer to contain the MapOverlay.
            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myLocationOverlay);
            // Add the MapLayer to the Map.            
            this.mapWithMyLocation.Layers.Add(myLocationLayer);
        }


        //******************************************************************************************       

        public void updateFriendsPosition()
        {
            clearMap();
            drawMyPosition();
            dragFriends();
        }

        //******************************************************************************************
        private void dragFriends()
        {
            PointsHandler ph = PointsHandler.Instance;
            List<KeyValuePair<string, nodo>> l = ph.allCoords();

            foreach (KeyValuePair<string, nodo> n_aux in l)
            {
                string name = n_aux.Value.name;
                GeoCoordinate geo = n_aux.Value.pos;
                Color color = n_aux.Value.color;

                Ellipse myCircle = new Ellipse();
                myCircle.Fill = new SolidColorBrush(color);
                myCircle.Height = 20;
                myCircle.Width = 20;
                myCircle.Opacity = 50;

                Pushpin pin = new Pushpin();
                pin.GeoCoordinate = geo;
                //define it's graphic properties 
                pin.Background = new SolidColorBrush(color);
                pin.Foreground = new SolidColorBrush(Colors.Black);
                //What to write on it
                pin.Content = name;

                // Create a MapOverlay to contain the circle.
                MapOverlay myLocationOverlaycircle = new MapOverlay();
                myLocationOverlaycircle.Content = myCircle;
                myLocationOverlaycircle.PositionOrigin = new Point(0.30, 0.20);
                myLocationOverlaycircle.GeoCoordinate = geo;

                // Create a MapOverlay to contain the Pushpin.
                MapOverlay myLocationOverlay2 = new MapOverlay();
                myLocationOverlay2.Content = pin;
                myLocationOverlay2.PositionOrigin = new Point(0, 1);
                myLocationOverlay2.GeoCoordinate = geo;

                // Create a MapLayer to contain the MapOverlay.
                MapLayer friendsLayer = new MapLayer();
                friendsLayer.Add(myLocationOverlaycircle);
                friendsLayer.Add(myLocationOverlay2);

                mapWithMyLocation.Layers.Add(friendsLayer);


            }
        }

        //******************************************************************************************
        private void clearMap()
        {
            mapWithMyLocation.Layers.Clear();
        }

        //******************************************************************************************


        private int i = 0;
        private void testFR_Click(object sender, RoutedEventArgs e)
        {

            PointsHandler ph = PointsHandler.Instance;
            double increment = (1E-4) * i;
            ph.insert("1", "juan", new GeoCoordinate(-34.9597 + increment, -54.9688 + increment));
            ph.insert("2", "luis", new GeoCoordinate(-34.96987 + increment, -54.968817 + increment));
            ph.insert("3", "maria", new GeoCoordinate(-34.9398 + increment, -54.968837 + increment));
            ph.insert("4", "andrea", new GeoCoordinate(-34.9496 + increment, -54.9687 + increment));
            i++;
            updateFriendsPosition();            
        }

        // Inicializacion del mapa

        protected async override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (App.Geolocator == null)
            {
                //Si nunca se inicializo
                App.Geolocator = new Geolocator();
                App.Geolocator.DesiredAccuracy = PositionAccuracy.High;
                App.Geolocator.MovementThreshold = 15; // The units are meters.
                App.Geolocator.PositionChanged += geolocator_PositionChanged;
            }
            
            // Graficar mi posicion y setearla en el singleton
            var pos = await App.Geolocator.GetGeopositionAsync(); 
            var pos2 = ConvertGeocoordinate(pos.Coordinate);
            PointsHandler ph = PointsHandler.Instance;
            ph.myPosition = pos2;
            // Make my current location the center of the Map.            
            this.mapWithMyLocation.Center = pos2;
            clearMap();
            drawMyPosition();
            dragFriends();
            DibujarAmigos();
            //Funcion que inicia el thread
           // Dibujar();
        }

        

        protected override void OnRemovedFromJournal(System.Windows.Navigation.JournalEntryRemovedEventArgs e)
        {
            App.Geolocator.PositionChanged -= geolocator_PositionChanged;
            App.Geolocator = null;
        }

        
        public static GeoCoordinate ConvertGeocoordinate(Geocoordinate geocoordinate)
        {
            return new GeoCoordinate
                (
                geocoordinate.Latitude,
                geocoordinate.Longitude,
                geocoordinate.Altitude ?? Double.NaN,
                geocoordinate.Accuracy,
                geocoordinate.AltitudeAccuracy ?? Double.NaN,
                geocoordinate.Speed ?? Double.NaN,
                geocoordinate.Heading ?? Double.NaN
                );
        }
        

        void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {

            if (!App.RunningInBackground)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    System.Diagnostics.Debug.WriteLine("Actualizamos en background");
                    latitud = args.Position.Coordinate.Latitude.ToString("0.00000");
                    longitud = args.Position.Coordinate.Longitude.ToString("0.00000");

                    var pos = ConvertGeocoordinate(args.Position.Coordinate);
                    PointsHandler ph = PointsHandler.Instance;
                    ph.myPosition = pos;
                    // Make my current location the center of the Map.
                    this.mapWithMyLocation.Center = pos;
                    clearMap();
                    drawMyPosition();
                    dragFriends();

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
                System.Diagnostics.Debug.WriteLine("Actualizamos en de frente");
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

                webClient.UploadStringAsync((new Uri("http://developmentpis.azurewebsites.net/api/Geolocation/SetLocation/")), "POST", json);


                Microsoft.Phone.Shell.ShellToast toast = new Microsoft.Phone.Shell.ShellToast();
                toast.Content = latitud + longitud;
                toast.Title = "Location: ";
                toast.NavigationUri = new Uri("/LoggedMainPages/Mapa.xaml", UriKind.Relative);
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

        private void PhoneApplicationPage_OrientationChanged_1(object sender, OrientationChangedEventArgs e)
        {   
            
            if ( (e.Orientation == PageOrientation.PortraitUp) && !App.Mapa )
            {
                App.Mapa = false;
                NavigationService.Navigate(new Uri("/LoggedMainPages/Menu.xaml", UriKind.Relative));
                
            }        
        
        }       

        private void PhoneApplicationPage_BackKeyPress_1(object sender, CancelEventArgs e)
        {
            App.Mapa = false;           
        }



        // Cuando me voy de la pagina apago el thread de actualizacion de puntos
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            
            newTimer.Stop();
           
            System.Diagnostics.Debug.WriteLine("Me fui de la pagina");

        }

        public void DibujarAmigos()
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
                LoggedUser user = LoggedUser.Instance;
                webClient.DownloadStringAsync(new Uri("http://developmentpis.azurewebsites.net/api/Geolocation/GetLastFriendsLocationsById/" + user.GetLoggedUser().Id));

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
                PointsHandler ph = PointsHandler.Instance;
                int i = 0;
                foreach (var friend in p.Amigos)
                {
                    ph.insert(i.ToString(), friend.Mail, new GeoCoordinate(Convert.ToDouble(friend.Latitude), Convert.ToDouble(friend.Longitude)));
                    i++;
                }
                System.Diagnostics.Debug.WriteLine("update friend positions!");
                updateFriendsPosition();            
            }
        }  
    }
}