using System;
using System.Web;
using System.Web.Security;
using SimpleMVCAuthentication.Security.Principal;
using SimpleMVCAuthentication.Settings;

namespace SimpleMVCAuthentication.Security
{
    public abstract class SimpleAuthenticationHandler : IAuthenticationHandler 
    {
        public abstract AuthenticationResult Authenticate(string Login, string Password);
        public abstract AuthenticationResult Authenticate(string Login);

        public void LogOut(HttpContextBase Context)
        {
            HttpCookie authCookie = Context.Request.Cookies[SettingsManager.AuthenticationSettings.CookieName];
            if (authCookie != null)
            {
                authCookie.Expires = DateTime.Now;
                Context.Response.SetCookie(authCookie);
            }

            HttpCookie sessionCookie = Context.Request.Cookies[SettingsManager.AuthenticationSettings.SessionCookieName];

            if (sessionCookie != null)
            {
                sessionCookie.Expires = DateTime.Now;
                Context.Response.SetCookie(sessionCookie);
            }
        }
        
        public User AuthenticateRequest(HttpContextBase Context)
        {
            HttpCookie cookie = Context.Request.Cookies[SettingsManager.AuthenticationSettings.CookieName];

            User user;
            if (cookie == null)
            {
                user = User.Anonymous;
            }
            else
            {
                user = DecryptUser(cookie.Value);
            }
            Context.User = user;

            if (!user.Identity.IsAuthenticated)
            {
                return user;
            }
            //Проверка данных пользователя

            HttpCookie sessionCookie = Context.Request.Cookies[SettingsManager.AuthenticationSettings.SessionCookieName];

            if (sessionCookie != null && sessionCookie.Expires > DateTime.Now)
            {
                //Проверка пока не нужна
                return user;
            }
            //Нужна проверка

            AuthenticationResult result = Authenticate(user.Identity.Name);

            if (result.Status == AuthenticationStatus.Success)
            {
                //Надо обновить cookie, на случай если права изменились  
                UpdateCookie(Context.Response, cookie, result.User);
                return user;
            }
            //Проверка не пройдена, убираем пользователя

            cookie.Expires = DateTime.Now;
            Context.Response.Cookies.Add(cookie);

            Context.User = User.Anonymous;

            return User.Anonymous;
        }

        public HttpCookie CreateAuthCookie(User User, bool KeepLoggedIn)
        {
            HttpCookie cookie = new HttpCookie(SettingsManager.AuthenticationSettings.CookieName);

            if (KeepLoggedIn)
            {
                cookie.Expires = DateTime.Now.AddDays(SettingsManager.AuthenticationSettings.DaysToExpiration);
            }

            cookie.Value = EncryptUser(User, KeepLoggedIn);

            return cookie;
        }

        protected void UpdateCookie(HttpResponseBase Response, HttpCookie cookie, User User)
        {
            SimpleAuthenticationTicket ticket = new SimpleAuthenticationTicket(cookie.Value);
            //Если KeepLoggedIn, то продляем
            if (ticket.KeepLoggedIn)
            {
                ticket.ExpirationDate = DateTime.Now.AddDays(SettingsManager.AuthenticationSettings.DaysToExpiration);
                cookie.Expires = ticket.ExpirationDate;
            }

            ticket.User = User;

            cookie.Value = ticket.Encrypt();

            Response.SetCookie(cookie);
        }

        public HttpCookie CreateSessionCookie(User User)
        {
            if (string.IsNullOrWhiteSpace(SettingsManager.AuthenticationSettings.SessionCookieName) ||
                SettingsManager.AuthenticationSettings.SessionTimeOut <= 0)
            {
                return null;
            }
            HttpCookie cookie = new HttpCookie(SettingsManager.AuthenticationSettings.SessionCookieName);

            cookie.Expires = DateTime.Now.AddMinutes(SettingsManager.AuthenticationSettings.SessionTimeOut);

            return cookie;
        }

        public void SetCookies(HttpResponseBase Response, User User, bool KeepLoggedIn)
        {
            HttpCookie cookie = CreateAuthCookie(User, KeepLoggedIn);
            Response.SetCookie(cookie);

            cookie = CreateSessionCookie(User);
            if (cookie != null)
            {
                Response.SetCookie(cookie);
            }
        }

        protected string EncryptUser(User User, bool KeepLoggedIn)
        {
            SimpleAuthenticationTicket ticket = new SimpleAuthenticationTicket(User, DateTime.Now,
                DateTime.Now.AddDays(SettingsManager.AuthenticationSettings.DaysToExpiration), KeepLoggedIn);

            return ticket.Encrypt();
        }

        protected User DecryptUser(string EncryptedUser)
        {
            SimpleAuthenticationTicket ticket = new SimpleAuthenticationTicket(EncryptedUser);

            return ticket.User;
        }
    }
}