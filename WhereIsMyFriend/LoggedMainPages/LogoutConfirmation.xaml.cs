using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WhereIsMyFriend.Classes;
using System.Windows.Media;
using WhereIsMyFriend.Resources;
using Microsoft.Phone.Net.NetworkInformation;

namespace WhereIsMyFriend.LoggedMainPages
{
    public partial class LogoutConfirmation : PhoneApplicationPage
    {
        public LogoutConfirmation()
        {
            InitializeComponent();
        }

        private bool IsNetworkAvailable()
        {
            if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.None)
                return false;
            else
                return true;
        }
        private async void Yes_Click(object sender, RoutedEventArgs e)
        {
            if (IsNetworkAvailable())
            {
                LoggedUser l = LoggedUser.Instance;
                var webClient = new WebClient();

                webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                webClient.UploadStringCompleted += this.sendPostCompleted;

                string json = "{\"Mail\":\"" + l.GetLoggedUser().Mail + "\"}";
                webClient.UploadStringAsync((new Uri(App.webService + "/api/Users/LogoutWhere")), "POST", json);
                await l.LogOut();
                //mapa*******************
                //App.Geolocator.PositionChanged -= geolocator_PositionChanged2;
                App.Geolocator = null;
                //mapa*******************
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            } 
            else
            {
                SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));
                CustomMessageBox messageBox = new CustomMessageBox()
                {
                    Caption = AppResources.NoInternetConnection,
                    Message = AppResources.NoInternetConnectionMessage,
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


            }

        
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
                    default:
                        break;
                }
            }
            else
            {
                

            }
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Menu.xaml", UriKind.Relative));
        }
    }
}