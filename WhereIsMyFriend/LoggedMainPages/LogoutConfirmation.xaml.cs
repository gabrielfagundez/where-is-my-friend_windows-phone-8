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

namespace WhereIsMyFriend.LoggedMainPages
{
    public partial class LogoutConfirmation : PhoneApplicationPage
    {
        public LogoutConfirmation()
        {
            InitializeComponent();
        }

        private async void Yes_Click(object sender, RoutedEventArgs e)
        {
            LoggedUser l = LoggedUser.Instance;
            var webClient = new WebClient();

            webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
            webClient.UploadStringCompleted += this.sendPostCompleted;

            string json = "{\"Mail\":\"" + l.GetLoggedUser().Mail + "\"}";
            webClient.UploadStringAsync((new Uri(App.webService + "/api/Users/LogoutWhere")), "POST", json);
            await l.LogOut();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative)); 

        
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