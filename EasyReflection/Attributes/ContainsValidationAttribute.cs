using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyReflection.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ContainsValidationAttribute : BaseValidationAttribute
    {
        public string InnerPropertyName { get; set; }

        public ContainsValidationAttribute(string DisplayName, string TargetPropertyName)
            : base(DisplayName, TargetPropertyName)
        {

        }
    }
}
