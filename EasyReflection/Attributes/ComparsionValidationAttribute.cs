using System;

namespace EasyReflection.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ComparsionValidationAttribute : BaseValidationAttribute
    {
        public ComparsionValidationType Type { get; set; }

        public ComparsionValidationAttribute(string DisplayName, string TargetPropertyName,
            ComparsionValidationType Type)
            : base(DisplayName, TargetPropertyName)
        {
            this.Type = Type;
        }
    }
}