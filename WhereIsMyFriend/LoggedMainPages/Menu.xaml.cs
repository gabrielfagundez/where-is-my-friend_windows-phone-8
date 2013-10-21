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

namespace WhereIsMyFriend.LoggedMainPages
{
    public partial class Menu : PhoneApplicationPage
    {
        
        public Menu()
        {
            InitializeComponent();         
            
            
        }

        private void HubTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            LoggedUser user = LoggedUser.Instance;
            UserData luser = user.GetLoggedUser();
            Uri LoggedUserFriends = new Uri("http://testingpis.azurewebsites.net/api/Friends/GetAllFriends/" + luser.Id);
            webClient.DownloadStringAsync(LoggedUserFriends);

        }
        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
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
    }
}