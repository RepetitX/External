using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyReflection.Attributes
{
    public class BaseValidationAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public string TargetPropertyName { get; set; }

        public BaseValidationAttribute(string DisplayName, string TargetPropertyName)
        {
            this.DisplayName = DisplayName;
            this.TargetPropertyName = TargetPropertyName;
        }
    }
}