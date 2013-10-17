using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.LocalizedResources;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Device.Location; // Provides the GeoCoordinate class.
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WhereIsMyFriend.Classes;
using Windows.Devices.Geolocation; //Provides the Geocoordinate class.

namespace WhereIsMyFriend.LoggedMainPages
{
    public partial class Mapa : PhoneApplicationPage
    {
        public Mapa()
        {
            InitializeComponent();
            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = "",
                LeftButtonContent = "Ok",
                Message = "Instrucciones:\n*desplegar simulador de posicionamiento para ver como cambia tu pos. actual(puntito azul)\n* el boton de test agrega algunos puntos, si apretetas varias veces el boton, se simula el movimiento de tus amigos\n*para agregar o actualizar posiciones de amigos usar la funcion  public void insert(string id, string nom, GeoCoordinate geo) q esta en la clase PointsHandler q es un singleton\n*para dejar de seguir a un amigo en el mapa esta la funcion deleteFriend(string id)",


            };
            messageBox.Show();
            GetCoordinate();

        }

        //******************************************************************************************
        private void zoomOUT_Click(object sender, RoutedEventArgs e)
        {
            mapWithMyLocation.ZoomLevel--;
        }



        private void zoomIN_Click(object sender, RoutedEventArgs e)
        {
            mapWithMyLocation.ZoomLevel++;

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

        public void GetCoordinate()
        {
            var watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High)
            {
                MovementThreshold = 1
            };
            watcher.PositionChanged += this.watcher_PositionChanged;
            watcher.Start();
        }
        //******************************************************************************************
        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var pos = e.Position.Location;
            PointsHandler ph = PointsHandler.Instance;
            ph.myPosition = pos;
            // Make my current location the center of the Map.
            this.mapWithMyLocation.Center = pos;
            clearMap();
            drawMyPosition();
            dragFriends();
        }

        public void updateFriednsPosition()
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
            updateFriednsPosition();
        }



    }
}