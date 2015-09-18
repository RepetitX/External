using System.Security.Principal;

namespace SimpleMVCAuthentication.Security.Principal
{
    public class User : IPrincipal
    {
        public bool IsInRole(string role)
        {
            return false;//so far
        }

        public IIdentity Identity { get; private set; }                     
    }
}