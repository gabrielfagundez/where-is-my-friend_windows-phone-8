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
using WhereIsMyFriend.Resources;
using System.Reflection.Emit;
using Microsoft.Phone.Net.NetworkInformation;
namespace WhereIsMyFriend.LoggedMainPages
{
    public partial class Mapa : PhoneApplicationPage
    {
        bool firstTime;
        string latitud, longitud = string.Empty;
        private GeoCoordinate myOldPosition;
        DispatcherTimer newTimer = new DispatcherTimer();
        DispatcherTimer gpsTimer = new DispatcherTimer();
        public Mapa()
        {

            InitializeComponent();
            // Código de ejemplo para traducir ApplicationBar
            BuildLocalizedApplicationBar();
            newTimer.Interval = TimeSpan.FromSeconds(5);
            newTimer.Tick += OnTimerTick;
            newTimer.Start();


            gpsTimer.Interval = TimeSpan.FromSeconds(10);
            gpsTimer.Tick += OngpsTimerTick;
            gpsTimer.Start();

        }

        private async void OngpsTimerTick(object sender, EventArgs e)
        {
            try
            {
                var pos = await App.Geolocator.GetGeopositionAsync();
                var pos2 = ConvertGeocoordinate(pos.Coordinate);
                PointsHandler ph = PointsHandler.Instance;
                ph.myPosition = pos2;
                App.isGpsEnabled = true;
            }
            catch (Exception)
            {
                App.isGpsEnabled = false;//no esta activado el gps
            }


        }

        private void internaltimer()
        {
            // text box property is set to current system date.
            // ToString() converts the datetime value into text
            System.Diagnostics.Debug.WriteLine("Friends Update en mapa!");
            DibujarAmigos();
        }
        void OnTimerTick(Object sender, EventArgs args)
        {
            internaltimer();
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
            if (App.isGpsEnabled)
            {
                GeoCoordinate pos = PointsHandler.Instance.myPosition;

                // Create a small circle to mark the current location.
                Ellipse myCircle = new Ellipse();
                myCircle.Stroke = new SolidColorBrush(Colors.Blue);
                myCircle.Height = 30;
                myCircle.Width = 30;
                myCircle.Opacity = 50;

                Ellipse myCircle2 = new Ellipse();
                myCircle2.Fill = new SolidColorBrush(Colors.Blue);
                myCircle2.Height = 20;
                myCircle2.Width = 20;
                myCircle2.Opacity = 50;


                // Create a MapOverlay to contain the circle.
                MapOverlay myLocationOverlay = new MapOverlay();
                myLocationOverlay.Content = myCircle;
                myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
                myLocationOverlay.GeoCoordinate = pos;

                MapOverlay myLocationOverlay2 = new MapOverlay();
                myLocationOverlay2.Content = myCircle2;
                myLocationOverlay2.PositionOrigin = new Point(0.5, 0.5);
                myLocationOverlay2.GeoCoordinate = pos;

                // Create a MapLayer to contain the MapOverlay.
                MapLayer myLocationLayer = new MapLayer();
                myLocationLayer.Add(myLocationOverlay);
                myLocationLayer.Add(myLocationOverlay2);
                // Add the MapLayer to the Map.            
                this.mapWithMyLocation.Layers.Add(myLocationLayer);

                /*if (PointsHandler.Instance.myPosition != myOldPosition) //centrar en mapa 
                {
                    this.mapWithMyLocation.Center = PointsHandler.Instance.myPosition;
                    myOldPosition = PointsHandler.Instance.myPosition;
                }*/
            }
        }


        //******************************************************************************************       

        public void updateFriendsPosition()
        {
            clearMap();
            drawMyPosition();
            drawFriends();
        }

        //******************************************************************************************
        private void drawFriends()
        {
            PointsHandler ph = PointsHandler.Instance;
            List<KeyValuePair<string, nodo>> l = ph.allCoords();

            foreach (KeyValuePair<string, nodo> n_aux in l)
            {
                string name = n_aux.Value.name;
                GeoCoordinate geo = n_aux.Value.pos;
                Color color = n_aux.Value.color;

                Image img = new Image();
                img.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Assets/Images/pin2.png", UriKind.Relative));
                img.Name = n_aux.Value.name;
                img.MouseEnter += img_MouseEnter;

                /* Ellipse myCircle = new Ellipse();
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
                 myLocationOverlay2.GeoCoordinate = geo;*/

                // Create a MapOverlay to contain the img.
                MapOverlay myLocationOverlay3 = new MapOverlay();
                myLocationOverlay3.Content = img;
                myLocationOverlay3.PositionOrigin = new Point(0.5, 1);
                myLocationOverlay3.GeoCoordinate = geo;

                // Create a MapLayer to contain the MapOverlay.
                MapLayer friendsLayer = new MapLayer();
                /*friendsLayer.Add(myLocationOverlaycircle);
                friendsLayer.Add(myLocationOverlay2);*/
                friendsLayer.Add(myLocationOverlay3);

                mapWithMyLocation.Layers.Add(friendsLayer);


            }
        }

        //******************************************************************************************
        private void img_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            updateFriendsPosition();
            Image im = (Image)sender;
            TextBox txt = new TextBox();
            txt.Text = im.Name;
            txt.IsReadOnly = true;
            txt.GotFocus += txt_GotFocus;
           

            MapOverlay myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = txt;
            //myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
            Point point = e.GetPosition(mapWithMyLocation);
            GeoCoordinate geo = mapWithMyLocation.ConvertViewportPointToGeoCoordinate(point);
            myLocationOverlay.GeoCoordinate = geo;

            // Create a MapLayer to contain the MapOverlay.
            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myLocationOverlay);
            // Add the MapLayer to the Map.            
            this.mapWithMyLocation.Layers.Add(myLocationLayer);

        }

        void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            mapWithMyLocation.Focus();
        }
        //******************************************************************************************
        private void clearMap()
        {
            mapWithMyLocation.Layers.Clear();
        }

        //******************************************************************************************


        private int i = 0;
        private void testFR_Click()
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
        // Inicializacion del mapa
        private bool IsNetworkAvailable()
        {
            if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.None)
                return false;
            else
                return true;
        }

        protected async override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (IsNetworkAvailable())
            {
                try
                {
                    LoggedUser lu = LoggedUser.Instance;
                    var webClient = new WebClient();
                    webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                    webClient.UploadStringCompleted += this.sendPostCompleted;
                    string json = "{\"Mail\":\"" + lu.GetLoggedUser().Mail + "\"}";
                    webClient.UploadStringAsync((new Uri(App.webService + "/api/Users/ResetAcceptBadge")), "POST", json);
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
            iniMap();
            newTimer.Start();
            gpsTimer.Start();
            firstTime = true;

            try
            {
                // Graficar mi posicion y setearla en el singleton
                clearMap();
                var pos = await App.Geolocator.GetGeopositionAsync();
                var pos2 = ConvertGeocoordinate(pos.Coordinate);
                PointsHandler ph = PointsHandler.Instance;
                ph.myPosition = pos2;
                // Make my current location the center of the Map.     
                App.isGpsEnabled = true;
                mapWithMyLocation.ZoomLevel = 2;

            }

            catch (Exception)
            {

                App.isGpsEnabled = false;//no esta activado el gps
            }
            updateFriendsPosition();
            DibujarAmigos();
        }

       
        private void sendPostCompleted(object sender, UploadStringCompletedEventArgs e)
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
                    case HttpStatusCode.BadRequest:
                        SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));
                        CustomMessageBox messageBox = new CustomMessageBox()
                        {
                            Message = AppResources.doubleReq,
                            LeftButtonContent = AppResources.OkTitle,
                            Background = mybrush,
                            IsFullScreen = false,
                        };


                        messageBox.Dismissed += (s1, e1) =>
                        {
                            switch (e1.Result)
                            {
                                case CustomMessageBoxResult.LeftButton:
                                    break;
                                case CustomMessageBoxResult.None:
                                    // Acción.
                                    break;
                                default:
                                    break;
                            }
                        };

                        messageBox.Show();
                        break;
                    default:
                        break;
                }
            }
            else
            {

            }
        }
     






        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            firstTime = false;
            App.Geolocator.PositionChanged -= geolocator_PositionChanged2;
            App.Geolocator = null;
            newTimer.Stop();
            gpsTimer.Stop();

            System.Diagnostics.Debug.WriteLine("Me fui de la pagina");

        }

        private void PhoneApplicationPage_OrientationChanged_1(object sender, OrientationChangedEventArgs e)
        {

            if ((e.Orientation == PageOrientation.PortraitUp) && !App.Mapa)
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


        public void DibujarAmigos()
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
                LoggedUser user = LoggedUser.Instance;
                webClient.DownloadStringAsync(new Uri(App.webService + "/api/Geolocation/GetLastFriendsLocationsById/" + user.GetLoggedUser().Id));

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
                ph.clearPines();
                int i = 0;
                foreach (var friend in p.Amigos)
                {
                    if (friend.Longitude != null && friend.Latitude != null)
                    {
                        double lat = double.Parse(friend.Latitude, System.Globalization.CultureInfo.InvariantCulture);//Convert.ToDouble(friend.Latitude);
                        double longi = double.Parse(friend.Longitude, System.Globalization.CultureInfo.InvariantCulture);//Convert.ToDouble(friend.Longitude);
                    
                        ph.insert(i.ToString(), friend.Name, new GeoCoordinate(lat, longi));
                        i++;
                    }
                    
                }
                if (firstTime)
                {
                    firstTime = false;
                    superIntern();
                }
                System.Diagnostics.Debug.WriteLine("update friend positions!");
                updateFriendsPosition();
            }
        }
        private void Add_Click(object sender, EventArgs e)
        {
            App.VengoDeMapa = true;
            NavigationService.Navigate(new Uri("/LoggedMainPages/Friends.xaml", UriKind.Relative));

        }

        private void superIntern(){
            PointsHandler ph = PointsHandler.Instance;
            List<KeyValuePair<string, nodo>> l = ph.allCoords();
            int p = 0;
            GeoCoordinate yo = null;
            if (App.isGpsEnabled)
            {
                yo = PointsHandler.Instance.myPosition;
                p = 1;
            }
            GeoCoordinate[] geoArr = new GeoCoordinate[l.Count + p];
            p = 0;
            foreach (KeyValuePair<string, nodo> n_aux in l)
            {
                geoArr[p] = n_aux.Value.pos;
                p++;
            }
            if (yo != null) geoArr[p] = yo;
            LocationRectangle setRect;
            if (geoArr.Count() > 0)
            {
                setRect = LocationRectangle.CreateBoundingRectangle(geoArr);
                mapWithMyLocation.SetView(setRect);
            }
        }

        private void superCenter(object sender, EventArgs e)
        {
            superIntern();
        }
        private void centerMe(object sender, EventArgs e)
        {
            if (App.isGpsEnabled)
            {
                mapWithMyLocation.SetView(PointsHandler.Instance.myPosition, mapWithMyLocation.ZoomLevel, MapAnimationKind.Parabolic);
            }

        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton =
                new ApplicationBarIconButton(new
                Uri("/Toolkit.Content/ApplicationBar.Add.png", UriKind.Relative));
            ApplicationBarIconButton centerButton =
                new ApplicationBarIconButton(new
                Uri("/Assets/Images/map.centerme.png", UriKind.Relative));
            ApplicationBarIconButton superCenterBottom =
               new ApplicationBarIconButton(new
               Uri("/Assets/Images/mapfriends.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarAddButtonText;
            centerButton.Text = AppResources.me;
            superCenterBottom.Text = AppResources.viewAll;
            superCenterBottom.Click += this.superCenter;
            appBarButton.Click += this.Add_Click;
            centerButton.Click += this.centerMe;
            ApplicationBar.Buttons.Add(appBarButton);
            ApplicationBar.Buttons.Add(centerButton);
            ApplicationBar.Buttons.Add(superCenterBottom);
            ApplicationBar.BackgroundColor = Color.FromArgb(255, 0, 175, 240);
            ApplicationBar.IsMenuEnabled = false;
            ApplicationBar.IsVisible = true;
            ApplicationBar.Opacity = (double)(.99);
            ApplicationBar.Mode = ApplicationBarMode.Default;

            
            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appBarMenuItem =
                new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        private void iniMap()
        {

            if (App.Geolocator == null)
            {
                //Si nunca se inicializo
                App.Geolocator = new Geolocator();
                App.Geolocator.DesiredAccuracy = PositionAccuracy.High;
                App.Geolocator.MovementThreshold = 15; // The units are meters.
                App.Geolocator.PositionChanged += geolocator_PositionChanged2;
            }

        }

        public void geolocator_PositionChanged2(Geolocator sender, PositionChangedEventArgs args)
        {
            App.isGpsEnabled = true;

            if (!App.RunningInBackground)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    System.Diagnostics.Debug.WriteLine("Actualizamos en background");
                    latitud = args.Position.Coordinate.Latitude.ToString("0.00000").Replace(",", ".");
                    longitud = args.Position.Coordinate.Longitude.ToString("0.00000").Replace(",", ".");

                    var pos = ConvertGeocoordinate(args.Position.Coordinate);
                    PointsHandler ph = PointsHandler.Instance;
                    ph.myPosition = pos;

                    var webClient = new WebClient();
                    webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                    webClient.UploadStringCompleted += this.sendPostCompleted1;
                    LoggedUser user = LoggedUser.Instance;
                    string json = "{\"Mail\":\"" + user.GetLoggedUser().Mail + "\"," +
                                    "\"Latitude\":\"" + latitud + "\"," +
                                      "\"Longitude\":\"" + longitud + "\"}";
                    System.Diagnostics.Debug.WriteLine(json);

                    webClient.UploadStringAsync((new Uri(App.webService + "/api/Geolocation/SetLocation/")), "POST", json);
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

                webClient.UploadStringAsync((new Uri(App.webService + "/api/Geolocation/SetLocation/")), "POST", json);




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
    }
}