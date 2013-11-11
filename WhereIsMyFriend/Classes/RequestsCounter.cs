using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereIsMyFriend.Classes
{
    class RequestsCounter
    {
        public event EventHandler PushReached;
        public int totalRequests;
        private static RequestsCounter instance;
        public static RequestsCounter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RequestsCounter();

                }
                return instance;
            }
        }
        public void Add()
        {
           OnPushReached(EventArgs.Empty);
        }
        protected virtual void OnPushReached(EventArgs e)
        {
            EventHandler handler = PushReached;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        
    }
    }
