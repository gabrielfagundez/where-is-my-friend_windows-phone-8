﻿using System;
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
            TiltEffect.TiltableItems.Add(typeof(Grid)); 
            
            
        }

        private void HubTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            LoggedUser user = LoggedUser.Instance;
            UserData luser = user.GetLoggedUser();
            Uri LoggedUserFriends = new Uri("http://developmentpis.azurewebsites.net/api/Friends/GetAllFriends/" + luser.Id);
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

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Settings.xaml", UriKind.Relative));
        }

        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LoggedUser user = LoggedUser.Instance;
            List<UserData> requests = new List<UserData>();
            UserData usuario1 = new UserData();
            UserData usuario2 = new UserData();
            UserData usuario3 = new UserData();
            UserData usuario4 = new UserData();
            usuario1.Name = "Jhon Smith";
            usuario2.Name = "Tom Jones";
            usuario3.Name = "Jamie Oliver";
            usuario4.Name = "Amanda Jones";
            requests.Add(usuario1);
            requests.Add(usuario2);
            requests.Add(usuario3);
            requests.Add(usuario4);
            user.setRequests(requests);
            NavigationService.Navigate(new Uri("/LoggedMainPages/Requests.xaml", UriKind.Relative));
        }
    }
}