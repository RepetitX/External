using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SimpleMVCAuthentication.Security.Principal;

namespace SimpleMVCAuthentication.Security
{
    public interface IAuthenticationHandler
    {
        AuthenticationResult Authenticate(string Login, string Password);
        AuthenticationResult Authenticate(string Login);
        User AuthenticateRequest(HttpContext Context);

        HttpCookie CreateAuthCookie(User User, bool KeepLoggedIn);
        HttpCookie CreateSessionCookie(User User);

        /*void SaveSessionData<T>(T Data);
        T LoadSessionData<T>(User User);*/
    }
}