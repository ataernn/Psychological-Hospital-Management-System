using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psychological_Hospital_Management_System
{
    public static class CurrentUser
    {
        public static string UserId { get; set; }  // A property to store the user's ID.
        public static string UserName { get; set; } // A property to store the user's first name.
        public static string UserSurname { get; set; } // A property to store the user's surname.
    }
}
