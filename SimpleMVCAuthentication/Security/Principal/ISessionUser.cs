using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMVCAuthentication.Security.Principal
{
    public interface ISessionUser
    {
        User User { get; }
    }
}
