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

namespace WhereIsMyFriend.LoggedMainPages
{
    public partial class Requests : PhoneApplicationPage
    {
        DispatcherTimer newTimer = new DispatcherTimer();
        public Requests()
        {
            InitializeComponent();
            LoggedUser luser = LoggedUser.Instance;
            RequestsList.ItemsSource = luser.getRequests();
            newTimer.Interval = TimeSpan.FromSeconds(5);
             //Sub-routine OnTimerTick will be called at every 1 second
            newTimer.Tick += OnTimerTick;
             //starting the timer
            newTimer.Start();
        }

        void OnTimerTick(Object sender, EventArgs args)
        {
            //text box property is set to current system date.
            //ToString() converts the datetime value into text
            System.Diagnostics.Debug.WriteLine("Requests Update en f!");
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            LoggedUser user = LoggedUser.Instance;
            UserData luser = user.GetLoggedUser();
            Uri LoggedUserFriends = new Uri(App.webService + "/api/Solicitudes/GetAll/" + luser.Id);
            webClient.DownloadStringAsync(LoggedUserFriends);
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            List<RequestData> requestsList = JsonConvert.DeserializeObject<List<RequestData>>(e.Result);
            LoggedUser luser = LoggedUser.Instance;
            luser.setRequests(requestsList);
            RequestsList.ItemsSource = luser.getRequests();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            var deleteme = cmd.DataContext as RequestData;
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
            webClient.UploadStringCompleted += this.sendPostCompletedCancel;

            string json = "{\"IdUser\":\"" + LoggedUser.Instance.GetLoggedUser().Id +"\"," +
                              "\"IdSolicitud\":\"" + deleteme.SolicitudId + "\"}";

            webClient.UploadStringAsync((new Uri(App.webService + "/api/Solicitudes/Reject")), "POST", json);

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

            }
        }
        
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {

            newTimer.Stop();

            System.Diagnostics.Debug.WriteLine("Me fui de la pagina");

        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            var deleteme = cmd.DataContext as RequestData;
             var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
            webClient.UploadStringCompleted += this.sendPostCompletedAccept;

            string json = "{\"IdUser\":\"" + LoggedUser.Instance.GetLoggedUser().Id +"\"," +
                              "\"IdSolicitud\":\"" + deleteme.SolicitudId + "\"}";

            webClient.UploadStringAsync((new Uri(App.webService + "/api/Solicitudes/Accept")), "POST", json);
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

            }
        }
    }
}