using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhereIsMyFriend.Classes;

namespace WhereIsMyFriend.Classes
{
    class LoggedUser
    {

        private static LoggedUser instance;
        private LoggedUser() { }
        private UserData user;
        private List<UserData> Friends;
        private List<UserData> Requests;

        public static LoggedUser Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoggedUser();
                    Session s = new Session();
                    if (s.Contains("Id"))
                    {
                        //FacebookSession f = FacebookSessionCacheProvider.Current.GetSessionData();
                        instance.user = new UserData();
                        instance.user.Mail = (string)s.GetStringObject("Email");
                        instance.user.Id = (string)s.GetStringObject("Id");
                        instance.user.FacebookId = (string)s.GetStringObject("FacebookId");
                        instance.user.LinkedInId = (string)s.GetStringObject("LinkedInId");
                        instance.user.Name = (string)s.GetStringObject("Name");
                        instance.user.Password = (string)s.GetStringObject("Password");
                        instance.Friends = new List<UserData>();
                    }
                    else
                    {
                        instance.user = null;
                    }
                }
                return instance;
            }
        }
        //private FacebookSession session;
        //public async Task Authenticate()
        //{
        //    string message = String.Empty;
        //    try
        //    {
        //        //session = await App.FacebookSessionClient.LoginAsync("user_about_me,read_stream");
        //        App.AccessToken = session.AccessToken;
        //        App.FacebookId = session.FacebookId;     
        //    }
        //    catch (InvalidOperationException e)
        //    {
        //        message = "Login failed! Exception details: " + e.Message;
        //        MessageBox.Show(message);
        //    }
        //}
                 

        public void SetLoggedUser(UserData u)
        {
            this.user = u;
            Session session = new Session();
            session.SaveStringObject("Mail", u.Mail);
            session.SaveStringObject("FacebookId", u.FacebookId);
            session.SaveStringObject("Id", u.Id);
            session.SaveStringObject("LinkedInId", u.LinkedInId);
            session.SaveStringObject("Name", u.Name);
            session.SaveStringObject("Password", u.Password);
        }

        public UserData GetLoggedUser()
        {
            return this.user;
        }

        public void setFriends(List<UserData> f)
        {
            this.Friends= f;
        }
        public List<UserData> getFriends()
        {
            return this.Friends;
        }
        public void  deleteUserRequests(UserData u)
        {
            this.Requests.Remove(u);
        }
        public void setRequests(List<UserData> f)
        {
            this.Requests = f;
        }
        public List<UserData> getRequests()
        {
            return this.Requests;
        }
        
        public UserData RegisterUser()
        {
            this.user = new UserData();
            return this.user;
        }

        public async Task LogOut()
        {
            this.user = null;
            Session session = new Session();
            session.RemoveStringObject("Email");
            session.RemoveStringObject("Id");
            session.RemoveStringObject("LinkedInId");
            session.RemoveStringObject("Name");
            session.RemoveStringObject("Password");
            session.RemoveStringObject("AccessToken");
            session.RemoveStringObject("FacebookId");

        }

        
       
    }
}
    

