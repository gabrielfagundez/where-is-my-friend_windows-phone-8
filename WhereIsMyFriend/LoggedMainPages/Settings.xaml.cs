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
            await l.LogOut();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));

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