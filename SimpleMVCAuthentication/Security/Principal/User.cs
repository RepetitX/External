using System.Security.Principal;

namespace SimpleMVCAuthentication.Security.Principal
{
    public class User : IPrincipal
    {
        public bool IsInRole(string role)
        {
            return false; //so far
        }

        public IIdentity Identity { get; private set; }

        public User(string Name, int Id)
        {
            Identity = new UserIdentity(Name, Id);
        }

        public User(string Name, bool IsAuthenticated)
        {
            Identity = new UserIdentity(Name, IsAuthenticated);
        }

        public static User Anonymous
        {
            get { return new User("anonymous", false); }
        }
    }
}