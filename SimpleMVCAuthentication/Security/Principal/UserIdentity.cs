using System.Security.Principal;
using System.Web.Security;

namespace SimpleMVCAuthentication.Security.Principal 
{
    public class UserIdentity : IIdentity
    {
        public int UserAccountId { get; private set; }
        //public UserGroupCode UserGroup { get; private set; }
        public string Name { get; private set; }
        public string AuthenticationType { get; private set; }
        public bool IsAuthenticated { get; private set; }

        /*public UserIdentity(UserAccount Account)
        {
            AuthenticationType = "UserAccount";
            if (Account != null)
            {
                UserAccountId = Account.Id;
                Name = Account.Name;
                IsAuthenticated = true;
                UserGroup = Account.UserGroup.Code;
            }
            else
            {
                UserAccountId = 0;
                Name = "anonym";
                UserGroup = UserGroupCode.Anonymous;
                IsAuthenticated = false;
            }
        }*/

        public UserIdentity(int UserAccountId, string Name, int UserGroupId)
        {
            this.Name = Name;
            IsAuthenticated = (Name != "anonym");
            this.UserAccountId = UserAccountId;
        }
    }
}
