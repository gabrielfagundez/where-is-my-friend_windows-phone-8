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
using WhereIsMyFriend.Resources;
using System.Windows.Media;

namespace WhereIsMyFriend.LoggedMainPages
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();

            // Código de ejemplo para traducir ApplicationBar
            BuildLocalizedApplicationBar();

        }

        private async void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            LoggedUser l = LoggedUser.Instance;
            //await l.LogOut();
            var webClient = new WebClient();
            
            webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
            webClient.UploadStringCompleted += this.sendPostCompleted;

            string json = "{\"Mail\":\"" + l.GetLoggedUser().Mail + "\"}";
            webClient.UploadStringAsync((new Uri(App.webService + "/api/Users/LogoutWhere")), "POST", json);
            await l.LogOut();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
          

        }

        private  void sendPostCompleted(object sender, UploadStringCompletedEventArgs e)
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
        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton =
                new ApplicationBarIconButton(new
                Uri("/Assets/images/Logout-32.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarLogoutButtonText;
            appBarButton.Click += this.ApplicationBarIconButton_Click;
            ApplicationBar.Buttons.Add(appBarButton);
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
    }
}