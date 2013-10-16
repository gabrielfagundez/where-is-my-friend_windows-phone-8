using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereIsMyFriend.Classes
{

    public class FriendData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string FacebookId { get; set; }
        public string LinkedInId { get; set; }
    }
    public class RootObject
    {
        public List<FriendData> MyBlogList { get; set; }
    }
   
}
