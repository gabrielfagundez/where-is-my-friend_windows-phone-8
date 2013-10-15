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
using WhereIsMyFriend.Classes;
using Newtonsoft.Json;
using Microsoft.Phone.Notification;
using System.Text;
using System.Windows.Media;

namespace WhereIsMyFriend
{
    public partial class MainPage : PhoneApplicationPage
    {
        private UserData UsuarioLogin;
        // Constructor
        public MainPage()
        {
            ///// Holds the push channel that is created or found.
            //HttpNotificationChannel pushChannel;

            //// The name of our push channel.
            //string channelName = "ToastSampleChannel";

            InitializeComponent();
            Loaded += MainPage_Loaded;
            MailIngresado.Text = "nuevo@gmail.com"; 
            PassIngresado.Password = "abcdefgh";
            // Try to find the push channel.
            //pushChannel = HttpNotificationChannel.Find(channelName);

            // If the channel was not found, then create a new connection to the push service.
            //if (pushChannel == null)
            //{
            //    pushChannel = new HttpNotificationChannel(channelName);

            //    // Register for all the events before attempting to open the channel.
            //    pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
            //    pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);
            //    pushChannel.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>(PushChannel_HttpNotificationReceived);

            //    // Register for this notification only if you need to receive the notifications while your application is running.
            //    pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

            //    pushChannel.Open();

            //    // Bind this new channel for toast events.
            //    pushChannel.BindToShellToast();

            //}
            //else
            //{
            //    // The channel was already open, so just register for all the events.
            //    pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
            //    pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

            //    // Register for this notification only if you need to receive the notifications while your application is running.
            //    pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

            //    // Display the URI for testing purposes. Normally, the URI would be passed back to your web service at this point.
            //    System.Diagnostics.Debug.WriteLine(pushChannel.ChannelUri.ToString());
            //    MessageBox.Show(String.Format("Channel Uri is {0}",
            //        pushChannel.ChannelUri.ToString()));

            //}


            // Código de ejemplo para traducir ApplicationBar
            //BuildLocalizedApplicationBar();
        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            while ((this.NavigationService.BackStack != null) && (this.NavigationService.BackStack.Any()))
            {
                this.NavigationService.RemoveBackEntry();
            }
        }
        private void Click_check(object sender, EventArgs e)

        {


            try
            {
                var webClient = new WebClient();
                webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                webClient.UploadStringCompleted += this.sendPostCompleted;

                string json = "{\"Email\":\"" + MailIngresado.Text + "\"," +
                                  "\"Password\":\"" + PassIngresado.Password + "\"}";

                webClient.UploadStringAsync((new Uri("http://testpis.azurewebsites.net/api/Login/")), "POST", json);
            }
            catch (WebException webex)
            {
                HttpWebResponse webResp = (HttpWebResponse)webex.Response;

                switch (webResp.StatusCode)
                {
                    case HttpStatusCode.NotFound: // 404
                        break;
                    case HttpStatusCode.InternalServerError: // 500
                        break;
                    default:
                        break;
                }
            }
        }

        private void sendPostCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if ((e.Error != null) && (e.Error.GetType().Name == "WebException"))
            {
                WebException we = (WebException)e.Error;
                HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                switch (response.StatusCode)
                {

                    case HttpStatusCode.NotFound: // 404
                        System.Diagnostics.Debug.WriteLine("Not found!");
                        ErrorBlock.Text = "That email is not registered";
                        ErrorBlock.Visibility = System.Windows.Visibility.Visible;
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        System.Diagnostics.Debug.WriteLine("Not authorized!");
                        ErrorBlock.Text = "The password is not correct";
                        ErrorBlock.Visibility = System.Windows.Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                LoggedUser user = LoggedUser.Instance;
                user.SetLoggedUser(JsonConvert.DeserializeObject<UserData>(e.Result));
                ErrorBlock.Visibility = System.Windows.Visibility.Collapsed;
                NavigationService.Navigate(new Uri("/LoggedMainPages/Menu.xaml", UriKind.Relative));
            }
        }
        //void PushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        //{

        //    Dispatcher.BeginInvoke(() =>
        //    {
        //        // Display the new URI for testing purposes.   Normally, the URI would be passed back to your web service at this point.
        //        System.Diagnostics.Debug.WriteLine(e.ChannelUri.ToString());
        //        MessageBox.Show(String.Format("Channel Uri is {0}",
        //            e.ChannelUri.ToString()));

        //    });
        //}
        //void PushChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        //{
        //    // Error handling logic for your particular application would be here.
        //    Dispatcher.BeginInvoke(() =>
        //        MessageBox.Show(String.Format("A push notification {0} error occurred.  {1} ({2}) {3}",
        //            e.ErrorType, e.Message, e.ErrorCode, e.ErrorAdditionalData))
        //            );
        //}
        //void PushChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        //{
        //    //StringBuilder message = new StringBuilder();
        //    //string relativeUri = string.Empty;

        //    //message.AppendFormat("Received Toast {0}:\n", DateTime.Now.ToShortTimeString());

        //    //// Parse out the information that was part of the message.
        //    //foreach (string key in e.Collection.Keys)
        //    //{
        //    //    message.AppendFormat("{0}: {1}\n", key, e.Collection[key]);

        //    //    if (string.Compare(
        //    //        key,
        //    //        "wp:Param",
        //    //        System.Globalization.CultureInfo.InvariantCulture,
        //    //        System.Globalization.CompareOptions.IgnoreCase) == 0)
        //    //    {
        //    //        relativeUri = e.Collection[key];
        //    //    }
        //    //}
         



        //    //// Display a dialog of all the fields in the toast.
        //    //Dispatcher.BeginInvoke(() => MessageBox.Show(message.ToString()));

        //}
        //void PushChannel_HttpNotificationReceived(object sender, HttpNotificationEventArgs e)
        //{
        //    string message;

        //    using (System.IO.StreamReader reader = new System.IO.StreamReader(e.Notification.Body))
        //    {
        //        message = reader.ReadToEnd();
        //    }
        //    SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));



        //    CustomMessageBox messageBox = new CustomMessageBox()
        //    {
        //        Caption = "",
        //        Message = message + " wants to know where you are",
        //        LeftButtonContent = "Accept",
        //        RightButtonContent = "Cancel",
        //        Background = mybrush,
        //        IsFullScreen = false
        //    };


        //    messageBox.Dismissed += (s1, e1) =>
        //    {
        //        switch (e1.Result)
        //        {
        //            case CustomMessageBoxResult.LeftButton:
        //                // Acción.
        //                break;
        //            case CustomMessageBoxResult.RightButton:
        //                // Acción.
        //                break;
        //            case CustomMessageBoxResult.None:
        //                // Acción.
        //                break;
        //            default:
        //                break;
        //        }
        //    };


        //    Dispatcher.BeginInvoke(() =>
        //        messageBox.Show());
        //}




    }
}
