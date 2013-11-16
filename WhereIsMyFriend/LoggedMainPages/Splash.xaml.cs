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
using Microsoft.Phone.Notification;
using System.Windows.Media;
using WhereIsMyFriend.Resources;

namespace WhereIsMyFriend.LoggedMainPages
{
    public partial class Splash : PhoneApplicationPage
    {
        private HttpWebRequest request;
        string channelName = "ToastSampleChannel";
        HttpNotificationChannel pushChannel = null;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            pushChannel = HttpNotificationChannel.Find(channelName);
            if (pushChannel != null)
            {
                // The channel was already open, so just register for all the events.
                pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                // Register for this notification only if you need to receive the notifications while your application is running.
                pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);
            }
        }

        void PushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(pushChannel.ChannelUri.ToString());
            try
            {

                System.Diagnostics.Debug.WriteLine(pushChannel.ChannelUri.ToString());
                var webClient = new WebClient();
                webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                LoggedUser l = LoggedUser.Instance;

                string json = "{\"Mail\":\"" + l.GetLoggedUser().Mail +"\"," +
                                                   "\"DeviceId\":\"" + pushChannel.ChannelUri.ToString() + "\"," + "\"Platform\":\"" + "wp" + "\"}";
                webClient.UploadStringAsync((new Uri(App.webService + "/api/Users/ChangeDeviceId")), "POST", json);

            }
            catch (Exception)
            {

                throw;
            }

            
        }
        void PushChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            // Error handling logic for your particular application would be here.
            Dispatcher.BeginInvoke(() =>
                MessageBox.Show(String.Format("A push notification {0} error occurred.  {1} ({2}) {3}",
                    e.ErrorType, e.Message, e.ErrorCode, e.ErrorAdditionalData))
                    );
        }

        void PushChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            //ShellToast toast = new ShellToast();
            //toast.Title = "Background Agent Sample";
            //toast.Content = "hola que carajo";
            //toast.Show();

            string caption;
            string message;
            string relativeUri = string.Empty;
            if (e.Collection.ContainsKey("wp:Text1"))
            {
                caption = e.Collection["wp:Text1"];
            }
            else caption = "";
            if (e.Collection.ContainsKey("wp:Text2"))
            {
                message = e.Collection["wp:Text2"];
            }
            else message = "";
            if (App.RunningInBackground)
            {
                ShellToast toast2 = new ShellToast();
                toast2.Title = "WIMF";
                toast2.Content = message;
                toast2.NavigationUri = new Uri(e.Collection["wp:Param"], UriKind.Relative);
                toast2.Show();
            }
            else
            {


                if (e.Collection["wp:Param"].Equals("/LoggedMainPages/Requests.xaml"))
                {
                    RequestsCounter rc = RequestsCounter.Instance;
                    rc.Add();
                }


                // Display a dialog of all the fields in the toast.
                Dispatcher.BeginInvoke(() =>
                {




                    SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));
                    CustomMessageBox messageBox = new CustomMessageBox()
                    {
                        Caption = caption,
                        Message = message.ToString(),
                        LeftButtonContent = AppResources.ViewTitle,
                        RightButtonContent = AppResources.CancelTitle,
                        Background = mybrush,
                        IsFullScreen = false
                    };


                    messageBox.Dismissed += (s1, e1) =>
                    {
                        switch (e1.Result)
                        {
                            case CustomMessageBoxResult.LeftButton:
                                NavigationService.Navigate(new Uri(e.Collection["wp:Param"], UriKind.Relative));
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





                });
            }

        }

      
    }
}
