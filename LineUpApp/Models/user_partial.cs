using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LineUpApp.Models
{
    public partial class user
    {
        public string _user_descriptor { get { return first_name + " " + last_name + "(" + username + ")";  } }
    }
}