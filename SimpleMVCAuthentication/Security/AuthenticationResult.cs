using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMVCAuthentication.Security.Principal;

namespace SimpleMVCAuthentication.Security
{
    public class AuthenticationResult
    {
        public AuthenticationStatus Status { get; set; }
        public User User { get; set; }

        public AuthenticationResult(AuthenticationStatus Status, User User)
        {
            this.Status = Status;
            this.User = User;
        }
    }
}