using System;

namespace EasyReflection.Attributes
{
    public class BaseValidationAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public string TargetMemberName { get; set; }

        public BaseValidationAttribute(string DisplayName, string TargetMemberName)
        {
            this.DisplayName = DisplayName;
            this.TargetMemberName = TargetMemberName;
        }
    }
}