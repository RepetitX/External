using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMVCAuthentication.Security.Principal
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public UserRole(int Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
    }
}