using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.Threading;
using System.Windows.Threading;
using WhereIsMyFriend.Classes;

namespace WhereIsMyFriend.LoggedMainPages
{
    public partial class Splash : PhoneApplicationPage
    {
        private HttpWebRequest request;
        private int i = 0;
        private Uri a;
        DispatcherTimer newTimer = new DispatcherTimer();
        public Splash()
        {
            InitializeComponent();
            UserData user = LoggedUser.Instance.GetLoggedUser();
            if (user == null)
            {
                a = new Uri("/MainPage.xaml", UriKind.Relative);
            }
            else
            {
                a = new Uri("/LoggedMainPages/Menu.xaml", UriKind.Relative);
            }
            // timer interval specified as 1 second
            newTimer.Interval = TimeSpan.FromSeconds(1);
            // Sub-routine OnTimerTick will be called at every 1 second
            newTimer.Tick += OnTimerTick;
            // starting the timer
            newTimer.Start();

        }
                void OnTimerTick(Object sender, EventArgs args)
        {
            // text box property is set to current system date.
            // ToString() converts the datetime value into text
            i += 1;
            if (i == 1)
            {
                newTimer.Stop();
                NavigationService.Navigate(a);                
            }
        }

      
    }
}
