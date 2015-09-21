using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SimpleMVCAuthentication.Security.Principal;

namespace SimpleMVCAuthentication.Security
{
    public interface IAuthenticationManager
    {
        AuthenticationResult Authenticate(string Login, string Password);
        AuthenticationResult Authenticate(string Login);

        HttpCookie CreateAuthCookie(User User, bool KeepLoggedIn);
        HttpCookie CreateSessionCookie(User User);
    }
}
