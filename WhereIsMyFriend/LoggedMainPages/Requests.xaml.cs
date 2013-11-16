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
using System.Windows.Threading;
using WhereIsMyFriend.Resources;
using System.Windows.Media;
using Microsoft.Phone.Net.NetworkInformation;

namespace WhereIsMyFriend.LoggedMainPages
{
    public partial class Requests : PhoneApplicationPage
    {
        DispatcherTimer newTimer = new DispatcherTimer();
        public Requests()
        {
            InitializeComponent();
            LoggedUser luser = LoggedUser.Instance;
            RequestsList.ItemsSource = luser.getRequests().Requests;
            newTimer.Interval = TimeSpan.FromSeconds(5);
             //Sub-routine OnTimerTick will be called at every 1 second
            newTimer.Tick += OnTimerTick;
             //starting the timer
            newTimer.Start();
            func();
        }
        private void func()
        {
            if (IsNetworkAvailable())
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Requests Update en f!");
                    WebClient webClient = new WebClient();
                    webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
                    LoggedUser user = LoggedUser.Instance;
                    UserData luser = user.GetLoggedUser();
                    Uri LoggedUserFriends = new Uri(App.webService + "/api/Solicitudes/GetAll/" + luser.Id);
                    webClient.DownloadStringAsync(LoggedUserFriends);
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
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (IsNetworkAvailable())
            {
                try
                {
                    LoggedUser lu = LoggedUser.Instance;
                    var webClient = new WebClient();
                    webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                    webClient.UploadStringCompleted += this.sendPostCompleted;
                    string json = "{\"Mail\":\"" + lu.GetLoggedUser().Mail + "\"}";
                    webClient.UploadStringAsync((new Uri(App.webService + "/api/Users/ResetBadge")), "POST", json);
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
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        System.Diagnostics.Debug.WriteLine("Not authorized!");
                        break;
                    case HttpStatusCode.BadRequest:
                        SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));
                        CustomMessageBox messageBox = new CustomMessageBox()
                        {
                            Message = AppResources.doubleReq,
                            LeftButtonContent = AppResources.OkTitle,
                            Background = mybrush,
                            IsFullScreen = false,
                        };


                        messageBox.Dismissed += (s1, e1) =>
                        {
                            switch (e1.Result)
                            {
                                case CustomMessageBoxResult.LeftButton:
                                    break;
                                case CustomMessageBoxResult.None:
                                    // Acción.
                                    break;
                                default:
                                    break;
                            }
                        };

                        messageBox.Show();
                        break;
                    default:
                        break;
                }
            }
            else
            {

            }
        }

        void OnTimerTick(Object sender, EventArgs args)
        {
            func();  
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if ((e.Error != null) && (e.Error.GetType().Name == "WebException"))
            {
                WebException we = (WebException)e.Error;
                HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                switch (response.StatusCode)
                {

                    case HttpStatusCode.NotFound: // 404
                        System.Diagnostics.Debug.WriteLine("Not found!");
                        ProgressB.IsIndeterminate = false;
                        Connecting.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        System.Diagnostics.Debug.WriteLine("Not authorized!");
                        ProgressB.IsIndeterminate = false;
                        Connecting.Visibility = System.Windows.Visibility.Collapsed;

                        break;
                    default:
                        break;
                }
            }
            else
            {
                List<RequestData> requestsList = JsonConvert.DeserializeObject<List<RequestData>>(e.Result);
                LoggedUser luser = LoggedUser.Instance;
                RequestsList r = new RequestsList();
                r.Requests = requestsList;
                luser.setRequests(r);
                RequestsList.ItemsSource = luser.getRequests().Requests;
                ProgressB.IsIndeterminate = false;
                Connecting.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsNetworkAvailable())
            {
                try
                {
                    ProgressB.IsIndeterminate = true;
                    Connecting.Visibility = System.Windows.Visibility.Visible;
                    Button cmd = (Button)sender;
                    var deleteme = cmd.DataContext as RequestData;
                    var webClient = new WebClient();
                    webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                    webClient.UploadStringCompleted += this.sendPostCompletedCancel;

                    string json = "{\"IdUser\":\"" + LoggedUser.Instance.GetLoggedUser().Id + "\"," +
                                      "\"IdSolicitud\":\"" + deleteme.SolicitudId + "\"}";

                    webClient.UploadStringAsync((new Uri(App.webService + "/api/Solicitudes/Reject")), "POST", json);
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
            else
            {
                SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));
                CustomMessageBox messageBox = new CustomMessageBox()
                {
                    Caption = AppResources.NoInternetConnection,
                    Message = AppResources.NoInternetConnectionMessage,
                    LeftButtonContent = AppResources.OkTitle,
                    Background = mybrush,
                    IsFullScreen = false,
                };


                messageBox.Dismissed += (s1, e1) =>
                {
                    switch (e1.Result)
                    {
                        case CustomMessageBoxResult.LeftButton:
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
        private void sendPostCompletedCancel(object sender, UploadStringCompletedEventArgs e)
        {
            if ((e.Error != null) && (e.Error.GetType().Name == "WebException"))
            {

                WebException we = (WebException)e.Error;
                HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                switch (response.StatusCode)
                {

                    case HttpStatusCode.NotFound: // 404
                        System.Diagnostics.Debug.WriteLine("Not found!");
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        System.Diagnostics.Debug.WriteLine("Not authorized!");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                func();
            }
        }
        
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            newTimer.Stop();
        }
        private bool IsNetworkAvailable()
        {
            if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.None)
                return false;
            else
                return true;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {

            if (IsNetworkAvailable())
            {
                try
                {
                    ProgressB.IsIndeterminate = true;
                    Connecting.Visibility = System.Windows.Visibility.Visible;
                    Button cmd = (Button)sender;
                    var deleteme = cmd.DataContext as RequestData;
                    var webClient = new WebClient();
                    webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                    webClient.UploadStringCompleted += this.sendPostCompletedAccept;

                    string json = "{\"IdUser\":\"" + LoggedUser.Instance.GetLoggedUser().Id + "\"," +
                                      "\"IdSolicitud\":\"" + deleteme.SolicitudId + "\"}";

                    webClient.UploadStringAsync((new Uri(App.webService + "/api/Solicitudes/Accept")), "POST", json);
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
            else
            {
                SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));
                CustomMessageBox messageBox = new CustomMessageBox()
                {
                    Caption = AppResources.NoInternetConnection,
                    Message = AppResources.NoInternetConnectionMessage,
                    LeftButtonContent = AppResources.OkTitle,
                    Background = mybrush,
                    IsFullScreen = false,
                };


                messageBox.Dismissed += (s1, e1) =>
                {
                    switch (e1.Result)
                    {
                        case CustomMessageBoxResult.LeftButton:
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

        private void sendPostCompletedAccept(object sender, UploadStringCompletedEventArgs e)
        {
            if ((e.Error != null) && (e.Error.GetType().Name == "WebException"))
            {
              
                WebException we = (WebException)e.Error;
                HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                switch (response.StatusCode)
                {

                    case HttpStatusCode.NotFound: // 404
                        System.Diagnostics.Debug.WriteLine("Not found!");
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        System.Diagnostics.Debug.WriteLine("Not authorized!");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                func();
            }
        }
    }
}