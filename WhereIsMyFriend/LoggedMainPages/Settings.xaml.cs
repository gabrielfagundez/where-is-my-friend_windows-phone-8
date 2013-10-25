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
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
        }

        private async void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            LoggedUser l = LoggedUser.Instance;
            await l.LogOut();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));

        }

      //private async void Click_Logout(object sender, RoutedEventArgs e)
      //  {
      //      LoggedUser l = LoggedUser.Instance;
      //      await l.LogOut();
      //      NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
      //  }
    }
}