using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WhereIsMyFriend.Resources;
using System.IO;
using System.Windows.Threading;
using Newtonsoft.Json;
using WhereIsMyFriend.Classes;
using System.Windows.Media; 



namespace WhereIsMyFriend.LoggedMainPages
{
    public partial class Friends : PhoneApplicationPage
    {
        
        public Friends()
        {
            InitializeComponent();
            LoggedUser luser = LoggedUser.Instance;
            FriendsList.ItemsSource = luser.getFriends();

          
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

        }
      

        private void Select(object sender, SelectionChangedEventArgs e)
        {
            var selectedUser = FriendsList.SelectedItem as UserData;
            SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));            
           


            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = "",
                Message = " Do you want to send " + selectedUser.Name +" a request to know where he is?",
                LeftButtonContent = "Ok",
                RightButtonContent = "Cancel",
                Background = mybrush,
                IsFullScreen = false
            };

            
            messageBox.Dismissed += (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        // Acción.
                        break;
                    case CustomMessageBoxResult.RightButton:
                        // Acción.
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
}