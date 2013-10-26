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
    public partial class Requests : PhoneApplicationPage
    {
        public Requests()
        {
            InitializeComponent();
            LoggedUser luser = LoggedUser.Instance;
            RequestsList.ItemsSource = luser.getRequests(); 
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //Button cmd = (Button)sender;
            //var deleteme = cmd.DataContext as UserData;
            //LoggedUser luser = LoggedUser.Instance;
            //luser.deleteUserFriends(deleteme);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            //Button cmd = (Button)sender;
            //var deleteme = cmd.DataContext as UserData;
        }
    }
}