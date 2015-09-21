using System;
using System.Web;
using System.Web.Security;
using SimpleMVCAuthentication.Security.Principal;
using SimpleMVCAuthentication.Settings;

namespace SimpleMVCAuthentication.Security
{
    public abstract class SimpleAuthenticationManager : IAuthenticationManager
    {
        public abstract AuthenticationResult Authenticate(string Login, string Password);
        public abstract AuthenticationResult Authenticate(string Login);

        public HttpCookie CreateAuthCookie(User User, bool KeepLoggedIn)
        {
            HttpCookie cookie = new HttpCookie(SettingsManager.AuthenticationSettings.CookieName);

            cookie.Expires = DateTime.Now.AddDays(SettingsManager.AuthenticationSettings.CookieExpirationTime);

            //Пока используем FormsAuthenticationTicket

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(User.Identity.Name, true, 0);

            string cookieValue = FormsAuthentication.Encrypt(ticket);

            cookie.Value = cookieValue;

            return cookie;
        }

        public HttpCookie CreateSessionCookie(User User)
        {
            HttpCookie cookie = new HttpCookie(SettingsManager.AuthenticationSettings.SessionCookieName);
            return cookie;
        }
    }
}