using System;

namespace EasyReflection.Attributes
{   
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class PropertyValidationAttribute : Attribute
    {      
        public string FilterPropertyDisplayName { get; set; }
        public string TargetPropertyName { get; set; }
        public PropertyValidationType Type { get; set; }

        public PropertyValidationAttribute(string FilterPropertyDisplayName, string TargetPropertyName, PropertyValidationType Type)
        {
            this.FilterPropertyDisplayName = FilterPropertyDisplayName;
            this.TargetPropertyName = TargetPropertyName;
            this.Type = Type;
        }
    }
}
