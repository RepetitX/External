using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using EasyReflection.Attributes;

namespace EasyReflection.Validation
{
    public class CustomObjectValidator : IValidator
    {
        protected object validationObject;
        protected List<string> validationErrors;

        public bool Verbose { get; set; }

        public CustomObjectValidator(object ValidationObject)
        {
            validationObject = ValidationObject;
            validationErrors = new List<string>();
        }

        public bool Validate(object Object)
        {
            throw new System.NotImplementedException();
        }

        public string GetValidationResult(object Object)
        {
            throw new System.NotImplementedException();
        }

        protected string CheckProperty(PropertyInfo ValidationProperty, object Object)
        {
            object validationPropertyValue = ValidationProperty.GetValue(this, null);
            if (validationPropertyValue == null)
            {
                return "";
            }

            var attrs = ValidationProperty.GetCustomAttributes(typeof(PropertyValidationAttribute), false);
            if (attrs.Length == 0)
            {
                return "";
            }
            StringBuilder result = new StringBuilder();

            foreach (PropertyValidationAttribute attr in attrs)
            {
                if (!CheckPropertyByAttribute(validationPropertyValue, attr, Object))
                {
                    return attr.FilterPropertyDisplayName;
                }
            }
            return "";
        }

        protected bool CheckPropertyByAttribute(object ValidationPropertyValue, PropertyValidationAttribute Attribute,
            object Object)
        {
            object objectPropertyValue = ReflectionHelper.GetPropertyValue(Attribute.TargetPropertyName, Object);

            switch (Attribute.Type)
            {
                case PropertyValidationType.Equal:
                case PropertyValidationType.LessOrEqual:
                case PropertyValidationType.Less:
                case PropertyValidationType.MoreOrEqual:
                case PropertyValidationType.More:
                case PropertyValidationType.NotEqual:
                    if (objectPropertyValue is IComparable && ValidationPropertyValue is IComparable)
                    {
                        return GetComparsionResult((IComparable)objectPropertyValue, (IComparable)ValidationPropertyValue,
                            Attribute.Type);
                    }
                    return true;
            }
            return true;
        }

        protected bool GetComparsionResult(IComparable ObjectValue, IComparable FilterValue, PropertyValidationType Type)
        {
            switch (Type)
            {
                case PropertyValidationType.Equal:
                    return ObjectValue.CompareTo(FilterValue) != 0;
                case PropertyValidationType.LessOrEqual:
                    return ObjectValue.CompareTo(FilterValue) <= 0;
                case PropertyValidationType.Less:
                    return ObjectValue.CompareTo(FilterValue) < 0;
                case PropertyValidationType.MoreOrEqual:
                    return ObjectValue.CompareTo(FilterValue) >= 0;
                case PropertyValidationType.More:
                    return ObjectValue.CompareTo(FilterValue) > 0;
                case PropertyValidationType.NotEqual:
                    return ObjectValue.CompareTo(FilterValue) != 0;
                default:
                    return false;
            }
        }
    }
}
