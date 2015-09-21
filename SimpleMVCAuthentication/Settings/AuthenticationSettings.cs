using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMVCAuthentication.Settings
{
    public class AuthenticationSettings
    {
        public string CookieName { get; set; }
        public string SessionCookieName { get; set; }
        public int SessionTime { get; set; }
        public int CookieExpirationTime { get; set; }
    }
}
