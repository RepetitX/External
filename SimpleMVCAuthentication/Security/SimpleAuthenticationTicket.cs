using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using SimpleMVCAuthentication.Security.Principal;
using SimpleMVCAuthentication.Settings;

namespace SimpleMVCAuthentication.Security
{
    public class SimpleAuthenticationTicket
    {
        public User User { get; set; }
        public bool KeepLoggedIn { get;set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public SimpleAuthenticationTicket(User User, DateTime IssueDate, DateTime ExpirationDate, bool KeepLoggedIn)
        {
            this.User = User;
            this.KeepLoggedIn = KeepLoggedIn;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
        }

        public SimpleAuthenticationTicket(string Token)
        {
            //Пока используем Forms

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(Token);

            if (ticket == null)
            {
                User = User.Anonymous;
            }
            //И здесь по-индусски пока
            string[] userData = ticket.UserData.Split(';');

            int userId = int.Parse(userData[0].Split('=')[1]);
            string displayName = userData[1].Split('=')[1];

            IssueDate = ticket.IssueDate;
            ExpirationDate = ticket.Expiration;

            User = new User(ticket.Name, userId, displayName);
            KeepLoggedIn = ticket.IsPersistent;
        }

        public string Encrypt()
        {
            string userData = string.Format("UserId={0};DisplayName={1}", User.UserIdentity.UserId,
               User.UserIdentity.DisplayName);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, User.Identity.Name, IssueDate,
                ExpirationDate, KeepLoggedIn, userData);

            return FormsAuthentication.Encrypt(ticket);
        }
    }
}
