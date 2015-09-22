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

        public HttpCookie CreateAuthCookie(User User, bool KeepLoggedIn)
        {
            HttpCookie cookie = new HttpCookie(SettingsManager.AuthenticationSettings.CookieName);

            cookie.Expires = DateTime.Now.AddDays(SettingsManager.AuthenticationSettings.DaysToExpiration);

            cookie.Value = EncryptUser(User);

            return cookie;
        }

        public User AuthenticateRequest(HttpContext Context)
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
                return user;
            }
            //Проверка не пройдена, убираем пользователя

            cookie.Expires = DateTime.Now;
            Context.Response.Cookies.Add(cookie);

            Context.User = User.Anonymous;

            return User.Anonymous;
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

        protected string EncryptUser(User User)
        {
            //Пока используем FormsAuthenticationTicket

            string userData = string.Format("UserId={0};DisplayName={1}", User.UserIdentity.UserId,
                User.UserIdentity.DisplayName);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, User.Identity.Name, DateTime.Now,
                DateTime.Now.AddDays(SettingsManager.AuthenticationSettings.DaysToExpiration), true, userData);

            return FormsAuthentication.Encrypt(ticket);
        }

        protected User DecryptUser(string EncryptedUser)
        {
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(EncryptedUser);

            if (ticket == null)
            {
                return User.Anonymous;
            }
            //И здесь по-индусски пока
            string[] userData = ticket.UserData.Split(';');

            int userId = int.Parse(userData[0].Split('=')[1]);
            string displayName = userData[1].Split('=')[1];

            return new User(ticket.Name, userId, displayName);
        }
    }
}