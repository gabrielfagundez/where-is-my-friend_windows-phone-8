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
        DispatcherTimer newTimer = new DispatcherTimer();
        
        public Friends()
        {
            InitializeComponent();
            newTimer.Interval = TimeSpan.FromSeconds(5);
            // Sub-routine OnTimerTick will be called at every 1 second
            newTimer.Tick += OnTimerTick;
            // starting the timer
            newTimer.Start();
            //LoggedUser luser = LoggedUser.Instance;

            //FriendsList.ItemsSource = luser.getFriends();
            // Código de ejemplo para traducir ApplicationBar
            BuildLocalizedApplicationBar();


          
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

        }
        void OnTimerTick(Object sender, EventArgs args)
        {
            // text box property is set to current system date.
            // ToString() converts the datetime value into text
            System.Diagnostics.Debug.WriteLine("Friends Update en f!");
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            LoggedUser user = LoggedUser.Instance;
            UserData luser = user.GetLoggedUser();
            Uri LoggedUserFriends = new Uri(App.webService + "/api/Friends/GetAllFriends/" + luser.Id);
            webClient.DownloadStringAsync(LoggedUserFriends);
        }
        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            List<UserData> friendsList = JsonConvert.DeserializeObject<List<UserData>>(e.Result);
            FriendsList.ItemsSource = friendsList;
        }

        private void Select(object sender, SelectionChangedEventArgs e)
        {
            var selectedUser = FriendsList.SelectedItem as UserData;
            SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));            
           


            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = "",
                Message = AppResources.sendRequest1Title + selectedUser.Name + AppResources.sendRequest2Title,
                LeftButtonContent = AppResources.YesTitle,
                RightButtonContent = AppResources.NoTitle,
                Background = mybrush,
                IsFullScreen = false
            };

            
            messageBox.Dismissed += (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
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
       
           private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton =
                new ApplicationBarIconButton(new
                Uri("/Toolkit.Content/feature.search.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarSearchButtonText;
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

           protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
           {

               newTimer.Stop();

               System.Diagnostics.Debug.WriteLine("Me fui de la pagina");

           }
       
        
    }
}