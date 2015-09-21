using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SimpleMVCAuthentication.Security;
using SimpleMVCAuthentication.Settings;

namespace SimpleMVCAuthentication.Modules
{
    public abstract class SimpleAuthenticationModule : IAuthenticationModule
    {
        protected abstract IAuthenticationManager AuthenticationManager { get; set; }

        public void Init(HttpApplication context)
        {
            context.LogRequest += OnLogRequest;
            context.AuthenticateRequest += AuthenticateRequest;
        }

        private void AuthenticateRequest(object sender, EventArgs e)
        {
            //SettingsManager.
        }

        public void OnLogRequest(Object source, EventArgs e)
        {
            //custom logging logic can go here
        }

        public void Dispose()
        {

        }
    }
}