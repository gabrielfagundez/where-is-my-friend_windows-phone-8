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
using Newtonsoft.Json;
using WhereIsMyFriend.Resources;
using System.Windows.Media;

namespace WhereIsMyFriend.LoggedMainPages
{
    public partial class Menu : PhoneApplicationPage
    {
        
        public Menu()
        {
            InitializeComponent();
            TiltEffect.TiltableItems.Add(typeof(Grid));
            // Código de ejemplo para traducir ApplicationBar
            BuildLocalizedApplicationBar();
            
            
        }

        private void HubTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompletedFriends);
            LoggedUser user = LoggedUser.Instance;
            UserData luser = user.GetLoggedUser();
            Uri LoggedUserFriends = new Uri(App.webService + "/api/Friends/GetAllFriends/" + luser.Id);
            webClient.DownloadStringAsync(LoggedUserFriends);

        }
        void webClient_DownloadStringCompletedFriends(object sender, DownloadStringCompletedEventArgs e)
        {
            List<UserData> friendsList = JsonConvert.DeserializeObject<List<UserData>>(e.Result);
            LoggedUser luser = LoggedUser.Instance;
            luser.setFriends(friendsList);    


            NavigationService.Navigate(new Uri("/LoggedMainPages/Friends.xaml", UriKind.Relative));

        }

        private void HubTile_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Mapa.xaml", UriKind.Relative));
            App.Mapa = true;
        }

        private void PhoneApplicationPage_OrientationChanged_1(object sender, OrientationChangedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Mapa.xaml", UriKind.Relative));
        }

        private void Requests_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompletedRequests);
            LoggedUser user = LoggedUser.Instance;
            UserData luser = user.GetLoggedUser();
            Uri LoggedUserRequests = new Uri(App.webService + "/api/Solicitudes/GetAll/" + luser.Id);
            webClient.DownloadStringAsync(LoggedUserRequests);
          
        }
        void webClient_DownloadStringCompletedRequests(object sender, DownloadStringCompletedEventArgs e)
        {
            List<RequestData> requestsList = JsonConvert.DeserializeObject<List<RequestData>>(e.Result);
            LoggedUser luser = LoggedUser.Instance;
            luser.setRequests(requestsList);
            NavigationService.Navigate(new Uri("/LoggedMainPages/Requests.xaml", UriKind.Relative));

        }

        private void Settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Settings.xaml", UriKind.Relative));

        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton =
                new ApplicationBarIconButton(new
                Uri("/Assets/Images/Logout-32.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarLogoutButtonText;
            appBarButton.Click += this.Settings_Click;
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